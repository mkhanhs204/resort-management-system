using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.Services.TableFoodOrders;
using PhucNguyenResort.ViewModels.TableFoodOrders;
using System.Security.Claims;

namespace PhucNguyenResort.Web.Controllers;

[Authorize(Roles = "F&B Staff")]
public class TableFoodOrdersController : Controller
{
    private readonly ITableFoodOrderService _tableFoodOrderService;

    public TableFoodOrdersController(ITableFoodOrderService tableFoodOrderService)
    {
        _tableFoodOrderService = tableFoodOrderService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int reservationId)
    {
        var model = await _tableFoodOrderService.BuildCreateOrderAsync(reservationId);
        if (model == null)
        {
            TempData["ErrorMessage"] = "Table reservation was not found or is not seated.";
            return RedirectToAction("TableReservations", "Dashboard");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TableFoodOrderCreateViewModel model)
    {
        var staffUserId = GetCurrentUserId();
        if (!staffUserId.HasValue)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            var invalidModel = await ReloadCreateModelAsync(model);
            return invalidModel == null ? RedirectToAction("TableReservations", "Dashboard") : View(invalidModel);
        }

        var result = await _tableFoodOrderService.CreateTableFoodOrderAsync(model, staffUserId.Value);
        if (result.Status == FoodOrderCreateStatus.Created && result.FoodOrderId.HasValue)
        {
            TempData["SuccessMessage"] = "Table food order created successfully.";
            return RedirectToAction(nameof(ByReservation), new { reservationId = model.ReservationId });
        }

        ModelState.AddModelError(string.Empty, GetCreateErrorMessage(result.Status, result.Message));
        var reloadedModel = await ReloadCreateModelAsync(model);
        return reloadedModel == null ? RedirectToAction("TableReservations", "Dashboard") : View(reloadedModel);
    }

    [HttpGet]
    public async Task<IActionResult> ByReservation(int reservationId)
    {
        var checkout = await _tableFoodOrderService.BuildCheckoutAsync(reservationId);
        if (checkout == null)
        {
            return NotFound();
        }

        ViewBag.Checkout = checkout;
        var orders = await _tableFoodOrderService.GetOrdersByReservationAsync(reservationId);
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int reservationId)
    {
        var model = await _tableFoodOrderService.BuildCheckoutAsync(reservationId);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChargeToRoom(int reservationId, int? bookingId)
    {
        if (!bookingId.HasValue)
        {
            TempData["ErrorMessage"] = "Please confirm the room booking before charging this table bill.";
            return RedirectToAction(nameof(Checkout), new { reservationId });
        }

        var result = await _tableFoodOrderService.ChargeToRoomAsync(reservationId, bookingId.Value);
        TempData[result.Status == TableFoodOrderActionStatus.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Status == TableFoodOrderActionStatus.Success
                ? "Table food orders were charged to the room bill."
                : result.Message ?? "Unable to charge table food orders to room.";

        return RedirectToAction(nameof(Checkout), new { reservationId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayAtRestaurant(int reservationId)
    {
        var result = await _tableFoodOrderService.PayAtRestaurantAsync(reservationId);
        TempData[result.Status == TableFoodOrderActionStatus.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Status == TableFoodOrderActionStatus.Success
                ? "Table food orders were marked as paid at restaurant."
                : result.Message ?? "Unable to mark table food orders as paid.";

        return RedirectToAction(nameof(Checkout), new { reservationId });
    }

    private async Task<TableFoodOrderCreateViewModel?> ReloadCreateModelAsync(TableFoodOrderCreateViewModel postedModel)
    {
        var freshModel = await _tableFoodOrderService.BuildCreateOrderAsync(postedModel.ReservationId);
        if (freshModel == null)
        {
            TempData["ErrorMessage"] = "Table reservation was not found or is no longer seated.";
            return null;
        }

        freshModel.CustomerNote = postedModel.CustomerNote;

        var postedItems = postedModel.Items.ToDictionary(item => item.MenuItemId);
        foreach (var item in freshModel.Items)
        {
            if (postedItems.TryGetValue(item.MenuItemId, out var postedItem))
            {
                item.Quantity = postedItem.Quantity;
                item.ItemNote = postedItem.ItemNote;
            }
        }

        return freshModel;
    }

    private static string GetCreateErrorMessage(FoodOrderCreateStatus status, string? fallbackMessage)
    {
        return status switch
        {
            FoodOrderCreateStatus.Forbidden => "You are not allowed to create this table food order.",
            FoodOrderCreateStatus.ReservationNotFound => "Table reservation was not found.",
            FoodOrderCreateStatus.ReservationNotSeated => "The table must be seated before creating a food order.",
            FoodOrderCreateStatus.NoItems => "Please select at least one menu item.",
            FoodOrderCreateStatus.InvalidQuantity => "Menu item quantity is invalid.",
            FoodOrderCreateStatus.MenuItemUnavailable => "One or more selected menu items are unavailable.",
            FoodOrderCreateStatus.MenuItemNotFound => "One or more selected menu items do not exist.",
            FoodOrderCreateStatus.DuplicateMenuItem => "Duplicate menu items are not allowed in the same order.",
            FoodOrderCreateStatus.InvalidNote => "Notes must not exceed 500 characters.",
            _ => fallbackMessage ?? "Unable to create table food order."
        };
    }

    private int? GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
