using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Controllers;

public class FoodOrdersController : Controller
{
    private readonly IFoodOrderService _foodOrderService;

    public FoodOrdersController(IFoodOrderService foodOrderService)
    {
        _foodOrderService = foodOrderService;
    }

    [HttpGet]
    public async Task<IActionResult> RoomService(int? bookingId)
    {
        if (!bookingId.HasValue)
        {
            return View(new RoomServiceOrderViewModel());
        }

        var viewModel = await _foodOrderService.BuildRoomServiceOrderAsync(bookingId.Value);
        if (viewModel == null)
        {
            ModelState.AddModelError(nameof(RoomServiceOrderViewModel.BookingId), "Booking was not found.");
            return View(new RoomServiceOrderViewModel { BookingId = bookingId.Value });
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RoomService(RoomServiceOrderViewModel viewModel)
    {
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

        var result = await _foodOrderService.CreateFoodOrderAsync(request);
        if (result.Status == FoodOrderCreateStatus.Created)
        {
            TempData["SuccessMessage"] = "Food order has been sent to the kitchen.";
            return RedirectToAction(nameof(Details), new { id = result.FoodOrderId });
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Food order could not be created.");

        var rebuiltViewModel = await _foodOrderService.BuildRoomServiceOrderAsync(viewModel.BookingId)
            ?? new RoomServiceOrderViewModel { BookingId = viewModel.BookingId };
        ApplySubmittedQuantities(rebuiltViewModel, viewModel);

        return View(rebuiltViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var foodOrder = await _foodOrderService.GetFoodOrderDetailAsync(id);
        if (foodOrder == null)
        {
            return NotFound();
        }

        return View(foodOrder);
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
