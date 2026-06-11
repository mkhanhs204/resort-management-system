using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.FoodOrders;

namespace PhucNguyenResort.Services.FoodOrders;

public class FoodOrderService : IFoodOrderService
{
    private readonly ResortDbContext _context;

    public FoodOrderService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request)
    {
        if (request.Items.Count == 0)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.NoItems,
                "Food order must contain at least one menu item.");
        }

        if (request.Items.Any(item => item.Quantity <= 0))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidQuantity,
                "Food order item quantity must be greater than zero.");
        }

        var bookingExists = await _context.Bookings
            .AsNoTracking()
            .AnyAsync(booking => booking.BookingId == request.BookingId);

        if (!bookingExists)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.BookingNotFound,
                "Booking was not found.");
        }

        var requestedItems = request.Items
            .GroupBy(item => item.MenuItemId)
            .Select(group => new FoodOrderItemRequest
            {
                MenuItemId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        if (requestedItems.Any(item => item.Quantity > 1000))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidQuantity,
                "Food order item quantity must not exceed 1000.");
        }

        var menuItemIds = requestedItems
            .Select(item => item.MenuItemId)
            .ToList();

        var menuItems = await _context.MenuItems
            .AsNoTracking()
            .Where(item =>
                menuItemIds.Contains(item.MenuItemId)
                && item.IsAvailable
                && !item.IsDeleted)
            .ToDictionaryAsync(item => item.MenuItemId);

        if (menuItems.Count != menuItemIds.Count)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.MenuItemUnavailable,
                "One or more menu items are unavailable or discontinued.");
        }

        var foodOrder = new FoodOrder
        {
            BookingId = request.BookingId,
            OrderStatus = FoodOrderStatus.Pending,
            OrderedAt = DateTime.Now
        };

        foreach (var requestedItem in requestedItems)
        {
            var menuItem = menuItems[requestedItem.MenuItemId];
            var orderItem = new FoodOrderItem
            {
                MenuItemId = menuItem.MenuItemId,
                Quantity = requestedItem.Quantity,
                // Snapshot the selling price at order time; later menu price changes must not affect this order.
                UnitPrice = menuItem.Price
            };

            foodOrder.FoodOrderItems.Add(orderItem);
        }

        foodOrder.TotalAmount = foodOrder.FoodOrderItems
            .Sum(item => item.Quantity * item.UnitPrice);

        _context.FoodOrders.Add(foodOrder);
        await _context.SaveChangesAsync();

        return FoodOrderCreateResult.Created(foodOrder.FoodOrderId, foodOrder.TotalAmount.Value);
    }
}
