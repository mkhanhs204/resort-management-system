using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Constants;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.ViewModels.FoodOrders;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace PhucNguyenResort.Controllers;

public class FoodOrdersController : Controller
{
    private readonly IFoodOrderService _foodOrderService;
    private readonly ResortDbContext _context;

    public FoodOrdersController(IFoodOrderService foodOrderService, ResortDbContext context)
    {
        _foodOrderService = foodOrderService;
        _context = context;
    }

    /// <summary>
    /// Menu Categories landing page — the main entry point for Room Service.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> RoomService()
    {
        int? userId = TryGetCurrentUserId(out var uid) ? uid : null;
        var viewModel = await _foodOrderService.BuildMenuCategoriesAsync(null, userId);
        return View("MenuCategories", viewModel);
    }

    /// <summary>
    /// Menu items page — browse and order food, optionally filtered by category.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> OrderMenu(int? bookingId, int? categoryId)
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            var menuOnly = await _foodOrderService.BuildRoomServiceMenuAsync(
                "Sign in with a checked-in booking to place a room service order.");
            menuOnly.ActiveCategoryId = categoryId;
            return View("RoomService", menuOnly);
        }

        if (!bookingId.HasValue)
        {
            var activeBooking = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == currentUserId && b.BookingStatus == BookingStatus.CheckedIn)
                .OrderByDescending(b => b.CheckInDate)
                .FirstOrDefaultAsync();

            if (activeBooking != null)
            {
                bookingId = activeBooking.BookingId;
            }
            else
            {
                var menuOnly = await _foodOrderService.BuildRoomServiceMenuAsync(
                    "You do not currently have a checked-in booking for room service ordering.");
                menuOnly.ActiveCategoryId = categoryId;
                return View("RoomService", menuOnly);
            }
        }

        var viewModel = await _foodOrderService.BuildRoomServiceOrderAsync(bookingId.Value, currentUserId);
        if (viewModel == null)
        {
            ModelState.AddModelError(
                nameof(RoomServiceOrderViewModel.BookingId),
                "Booking was not found, is not checked in, or does not belong to your account.");

            var menuOnlyViewModel = await _foodOrderService.BuildRoomServiceMenuAsync();
            menuOnlyViewModel.BookingId = bookingId.Value;
            menuOnlyViewModel.ActiveCategoryId = categoryId;
            return View("RoomService", menuOnlyViewModel);
        }

        viewModel.ActiveCategoryId = categoryId;
        if (categoryId.HasValue)
        {
            var category = await _context.MenuCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId.Value);
            viewModel.ActiveCategoryName = category?.CategoryName;
        }

        return View("RoomService", viewModel);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Customer)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OrderMenu(RoomServiceOrderViewModel viewModel)
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var request = new FoodOrderCreateRequest
        {
            BookingId = viewModel.BookingId,
            BookingDetailId = viewModel.BookingDetailId,
            RequestedDeliveryAt = viewModel.RequestedDeliveryAt,
            CustomerNote = viewModel.CustomerNote,
            Items = viewModel.MenuItems
                .Select(item => new FoodOrderItemRequest
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    ItemNote = item.ItemNote
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

        return View("RoomService", rebuiltViewModel);
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

    [Authorize(Roles = AppRoles.Customer)]
    public async Task<IActionResult> MyOrders()
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var orders = await _foodOrderService.GetFoodOrdersForCustomerAsync(currentUserId);
        return View(orders);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Customer)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        if (!TryGetCurrentUserId(out var currentUserId))
        {
            return Challenge();
        }

        var result = await _foodOrderService.CancelFoodOrderByCustomerAsync(id, currentUserId);

        if (result.Status == FoodOrderActionStatus.Success)
        {
            TempData["SuccessMessage"] = "Your room service order has been cancelled.";
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "The order could not be cancelled.";
        }

        return RedirectToAction(nameof(MyOrders));
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
            FoodOrderCreateStatus.RoomNotSelected => "Please select a delivery room.",
            FoodOrderCreateStatus.BookingRoomNotFound => "The selected delivery room does not belong to this booking.",
            FoodOrderCreateStatus.BookingRoomNotAssigned => "The selected delivery room has not been assigned yet.",
            FoodOrderCreateStatus.InvalidDeliveryTime => "Requested delivery time is invalid.",
            FoodOrderCreateStatus.InvalidNote => "Order notes must not exceed 500 characters.",
            _ => result.Message ?? "Food order could not be created."
        };
    }

    private static void ApplySubmittedQuantities(RoomServiceOrderViewModel target, RoomServiceOrderViewModel source)
    {
        target.BookingDetailId = source.BookingDetailId;
        target.RequestedDeliveryAt = source.RequestedDeliveryAt;
        target.CustomerNote = source.CustomerNote;
        var quantities = source.MenuItems.ToDictionary(item => item.MenuItemId, item => item.Quantity);
        var notes = source.MenuItems.ToDictionary(item => item.MenuItemId, item => item.ItemNote);

        foreach (var item in target.MenuItems)
        {
            if (quantities.TryGetValue(item.MenuItemId, out var quantity))
            {
                item.Quantity = quantity;
            }

            if (notes.TryGetValue(item.MenuItemId, out var note))
            {
                item.ItemNote = note;
            }
        }
    }
}
