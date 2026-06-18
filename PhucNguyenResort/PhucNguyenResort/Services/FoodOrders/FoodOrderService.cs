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

    public async Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId)
    {
        var booking = await _context.Bookings
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            return null;
        }

        var viewModel = new RoomServiceOrderViewModel
        {
            BookingId = booking.BookingId,
            CustomerName = booking.User?.FullName ?? string.Empty,
            RoomNumber = booking.BookingDetails != null ? string.Join(", ", booking.BookingDetails.Where(bd => bd.Room != null).Select(bd => bd.Room!.RoomNumber)) : string.Empty,
            BookingStatus = booking.BookingStatus,
            CanPlaceOrder = booking.BookingStatus == BookingStatus.CheckedIn
        };

        if (!viewModel.CanPlaceOrder)
        {
            viewModel.Message = "Room service orders are only available for checked-in bookings.";
            return viewModel;
        }

        viewModel.MenuItems = await _context.MenuItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.IsAvailable && !item.IsDeleted)
            .OrderBy(item => item.Category == null ? string.Empty : item.Category.CategoryName)
            .ThenBy(item => item.ItemName)
            .Select(item => new RoomServiceMenuItemViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                CategoryName = item.Category == null
                    ? "Uncategorized"
                    : item.Category.CategoryName,
                Price = item.Price
            })
            .ToListAsync();

        return viewModel;
    }

    public async Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request)
    {
        if (request.Items.Any(item => item.Quantity < 0))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidQuantity,
                "Food order item quantity must not be negative.");
        }

        var requestedItems = request.Items
            .Where(item => item.Quantity > 0)
            .GroupBy(item => item.MenuItemId)
            .Select(group => new FoodOrderItemRequest
            {
                MenuItemId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        if (requestedItems.Count == 0)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.NoItems,
                "Food order must contain at least one menu item.");
        }

        if (requestedItems.Any(item => item.Quantity > 1000))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidQuantity,
                "Food order item quantity must not exceed 1000.");
        }

        var booking = await _context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(booking => booking.BookingId == request.BookingId);

        if (booking == null)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.BookingNotFound,
                "Booking was not found.");
        }

        if (booking.BookingStatus != BookingStatus.CheckedIn)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.BookingNotCheckedIn,
                "Room service orders are only available for checked-in bookings.");
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
            OrderedAt = DateTime.Now,
            Kot = new Kot
            {
                Status = KotStatus.Pending,
                CreatedAt = DateTime.Now
            }
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

    public async Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId)
    {
        var foodOrder = await _context.FoodOrders
            .AsNoTracking()
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.User)
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.BookingDetails)
                    .ThenInclude(bd => bd.Room)
            .Include(order => order.Kot)
            .Include(order => order.FoodOrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
                    .ThenInclude(menuItem => menuItem!.Category)
            .FirstOrDefaultAsync(order => order.FoodOrderId == foodOrderId);

        if (foodOrder == null)
        {
            return null;
        }

        return new FoodOrderDetailViewModel
        {
            FoodOrderId = foodOrder.FoodOrderId,
            BookingId = foodOrder.BookingId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.Booking?.BookingDetails != null ? string.Join(", ", foodOrder.Booking.BookingDetails.Where(bd => bd.Room != null).Select(bd => bd.Room!.RoomNumber)) : string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            Items = foodOrder.FoodOrderItems
                .OrderBy(item => item.MenuItem?.Category?.CategoryName)
                .ThenBy(item => item.MenuItem?.ItemName)
                .Select(item => new FoodOrderDetailItemViewModel
                {
                    ItemName = item.MenuItem?.ItemName ?? string.Empty,
                    CategoryName = item.MenuItem?.Category?.CategoryName ?? "Uncategorized",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                })
                .ToList()
        };
    }
}
