using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Constants;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.ViewModels.FoodOrders;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers;

public class FoodOrdersController : Controller
{
    private readonly IFoodOrderService _foodOrderService;

    public FoodOrdersController(IFoodOrderService foodOrderService)
    {
        _foodOrderService = foodOrderService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Customer)]
    public async Task<IActionResult> RoomService(int? bookingId)
    {
        if (!bookingId.HasValue)
        {
            return View(new RoomServiceOrderViewModel());
        }

        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var viewModel = await _foodOrderService.BuildRoomServiceOrderAsync(bookingId.Value, currentUserId);
        if (viewModel == null)
        {
            ModelState.AddModelError(
                nameof(RoomServiceOrderViewModel.BookingId),
                "Booking was not found, is not checked in, or does not belong to your account.");

            return View(new RoomServiceOrderViewModel { BookingId = bookingId.Value });
        }

        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Customer)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoomService(RoomServiceOrderViewModel viewModel)
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var request = new FoodOrderCreateRequest
        {
            BookingId = viewModel.BookingId,
            Items = viewModel.MenuItems
                .Select(item => new FoodOrderItemRequest
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                })
                .ToList()
        };

        var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);
        if (result.Status == FoodOrderCreateStatus.Created)
        {
            TempData["SuccessMessage"] = "Food order has been sent to the kitchen.";
            return RedirectToAction(nameof(Details), new { id = result.FoodOrderId });
        }

        ModelState.AddModelError(string.Empty, GetCreateOrderErrorMessage(result));

        var rebuiltViewModel = await _foodOrderService.BuildRoomServiceOrderAsync(viewModel.BookingId, currentUserId)
            ?? new RoomServiceOrderViewModel { BookingId = viewModel.BookingId };
        ApplySubmittedQuantities(rebuiltViewModel, viewModel);

        return View(rebuiltViewModel);
    }

    [Authorize(Roles = AppRoles.Customer)]
    public async Task<IActionResult> Details(int id)
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var foodOrder = await _foodOrderService.GetFoodOrderDetailAsync(id, currentUserId);
        if (foodOrder == null)
        {
            return NotFound();
        }

        return View(foodOrder);
    }

    private bool TryGetCurrentUserId(out int currentUserId)
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out currentUserId);
    }

    private static string GetCreateOrderErrorMessage(FoodOrderCreateResult result)
    {
        return result.Status switch
        {
            FoodOrderCreateStatus.Forbidden => "This booking does not belong to your account.",
            FoodOrderCreateStatus.BookingNotFound => "Booking was not found.",
            FoodOrderCreateStatus.BookingNotCheckedIn => "Room service orders are only available for checked-in bookings.",
            FoodOrderCreateStatus.NoItems => "Please select at least one menu item.",
            FoodOrderCreateStatus.InvalidQuantity => "Menu item quantity must be between 0 and 1000.",
            FoodOrderCreateStatus.MenuItemUnavailable => "One or more menu items are unavailable or discontinued.",
            FoodOrderCreateStatus.MenuItemNotFound => "One or more menu items do not exist.",
            FoodOrderCreateStatus.DuplicateMenuItem => "Duplicate menu items are not allowed in the same order.",
            _ => result.Message ?? "Food order could not be created."
        };
    }

    private static void ApplySubmittedQuantities(RoomServiceOrderViewModel target, RoomServiceOrderViewModel source)
    {
        var quantities = source.MenuItems.ToDictionary(item => item.MenuItemId, item => item.Quantity);

        foreach (var item in target.MenuItems)
        {
            if (quantities.TryGetValue(item.MenuItemId, out var quantity))
            {
                item.Quantity = quantity;
            }
        }
    }
}
