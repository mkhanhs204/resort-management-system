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
        return await BuildRoomServiceOrderAsync(bookingId, null);
    }

    public async Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId, int currentUserId)
    {
        return await BuildRoomServiceOrderAsync(bookingId, (int?)currentUserId);
    }

    private async Task<RoomServiceOrderViewModel?> BuildRoomServiceOrderAsync(int bookingId, int? currentUserId)
    {
        var booking = await _context.Bookings
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            return null;
        }

        if (currentUserId.HasValue && booking.UserId != currentUserId.Value)
        {
            return null;
        }

        var viewModel = new RoomServiceOrderViewModel
        {
            BookingId = booking.BookingId,
            CustomerName = booking.User?.FullName ?? string.Empty,
            RoomNumber = booking.Room?.RoomNumber ?? string.Empty,
            BookingStatus = booking.BookingStatus,
            CanPlaceOrder = booking.BookingStatus == BookingStatus.CheckedIn
        };

        if (!viewModel.CanPlaceOrder)
        {
            return null;
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
                ImageUrl = item.ImageUrl,
                Price = item.Price
            })
            .ToListAsync();

        return viewModel;
    }

    public async Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request, int currentUserId)
    {
        if (request.Items.Any(item => item.Quantity < 0))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidQuantity,
                "Food order item quantity must not be negative.");
        }

        if (request.Items.GroupBy(i => i.MenuItemId).Any(g => g.Count() > 1))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.DuplicateMenuItem,
                "Duplicate menu items are not allowed in the same order.");
        }

        var requestedItems = request.Items
            .Where(item => item.Quantity > 0)
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

        if (booking.UserId != currentUserId)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.Forbidden,
                "You are not authorized to place an order for this booking.");
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
            .Where(item => menuItemIds.Contains(item.MenuItemId))
            .ToDictionaryAsync(item => item.MenuItemId);

        if (menuItemIds.Any(id => !menuItems.ContainsKey(id)))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.MenuItemNotFound,
                "One or more menu items do not exist.");
        }

        if (menuItems.Values.Any(item => !item.IsAvailable || item.IsDeleted))
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
            IsChargedToRoom = false,
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
        return await GetFoodOrderDetailAsync(foodOrderId, null);
    }

    public async Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId, int currentUserId)
    {
        return await GetFoodOrderDetailAsync(foodOrderId, (int?)currentUserId);
    }

    private async Task<FoodOrderDetailViewModel?> GetFoodOrderDetailAsync(int foodOrderId, int? currentUserId)
    {
        var query = _context.FoodOrders
            .AsNoTracking()
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.User)
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.Room)
            .Include(order => order.Kot)
            .Include(order => order.FoodOrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
                    .ThenInclude(menuItem => menuItem!.Category)
            .Where(order => order.FoodOrderId == foodOrderId);

        if (currentUserId.HasValue)
        {
            query = query.Where(order => order.Booking != null && order.Booking.UserId == currentUserId.Value);
        }

        var foodOrder = await query.FirstOrDefaultAsync();

        if (foodOrder == null)
        {
            return null;
        }

        return new FoodOrderDetailViewModel
        {
            FoodOrderId = foodOrder.FoodOrderId,
            BookingId = foodOrder.BookingId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.Booking?.Room?.RoomNumber ?? string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            IsChargedToRoom = foodOrder.IsChargedToRoom,
            ChargedAt = foodOrder.ChargedAt,
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

    public async Task<List<FoodOrderDetailViewModel>> GetAllFoodOrdersAsync()
    {
        var foodOrders = await _context.FoodOrders
            .AsNoTracking()
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.User)
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.Room)
            .Include(order => order.Kot)
            .Include(order => order.FoodOrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
                    .ThenInclude(menuItem => menuItem!.Category)
            .OrderByDescending(order => order.OrderedAt)
            .ToListAsync();

        return foodOrders.Select(foodOrder => new FoodOrderDetailViewModel
        {
            FoodOrderId = foodOrder.FoodOrderId,
            BookingId = foodOrder.BookingId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.Booking?.Room?.RoomNumber ?? string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            IsChargedToRoom = foodOrder.IsChargedToRoom,
            ChargedAt = foodOrder.ChargedAt,
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
        }).ToList();
    }

    // [UC66] F&B Staff cap nhat trang thai don hang
    public async Task<FoodOrderActionResult> UpdateOrderStatusAsync(int foodOrderId, FoodOrderStatus newStatus)
    {
        var foodOrder = await _context.FoodOrders
            .Include(o => o.Kot)
            .FirstOrDefaultAsync(o => o.FoodOrderId == foodOrderId);

        if (foodOrder == null)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.NotFound, "Food order not found.");
        }

        var currentStatus = foodOrder.OrderStatus;
        bool isValidTransition = false;

        switch (currentStatus)
        {
            case FoodOrderStatus.Pending:
                // From Pending, can only move to Cooking or Cancelled
                if (newStatus == FoodOrderStatus.Cooking || newStatus == FoodOrderStatus.Cancelled)
                {
                    isValidTransition = true;
                }
                break;

            case FoodOrderStatus.Cooking:
                // From Cooking, can only move to Delivered
                if (newStatus == FoodOrderStatus.Delivered)
                {
                    // Dual Status Model check (ADR-002): Kitchen must be done
                    if (foodOrder.Kot != null && (foodOrder.Kot.Status == KotStatus.Ready || foodOrder.Kot.Status == KotStatus.Completed))
                    {
                        isValidTransition = true;
                    }
                }
                break;

            case FoodOrderStatus.Delivered:
            case FoodOrderStatus.Cancelled:
                // Terminal states, no transitions allowed
                break;
        }

        if (!isValidTransition)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, 
                $"Cannot transition from {currentStatus} to {newStatus} or constraints not met.");
        }

        foodOrder.OrderStatus = newStatus;
        await _context.SaveChangesAsync();

        return FoodOrderActionResult.Succeeded();
    }

    // [UC69] Ghi no vao tien phong
    public async Task<FoodOrderActionResult> ChargeToRoomAsync(int foodOrderId)
    {
        var foodOrder = await _context.FoodOrders
            .FirstOrDefaultAsync(o => o.FoodOrderId == foodOrderId);

        if (foodOrder == null)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.NotFound, "Food order not found.");
        }

        if (foodOrder.OrderStatus == FoodOrderStatus.Cancelled)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, "Cannot charge a cancelled order.");
        }

        if (foodOrder.OrderStatus != FoodOrderStatus.Delivered)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, "Order must be delivered before charging to room.");
        }

        if (foodOrder.IsChargedToRoom)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.AlreadyCharged, "Order has already been charged to room.");
        }

        var folio = await _context.GuestFolios
            .FirstOrDefaultAsync(f => f.BookingId == foodOrder.BookingId);

        if (folio == null)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.NoFolio, "No guest folio found for this booking.");
        }

        // Apply charge
        foodOrder.IsChargedToRoom = true;
        foodOrder.ChargedAt = DateTime.Now;

        folio.TotalFoodCharges += foodOrder.TotalAmount ?? 0;

        var transaction = new FolioTransaction
        {
            FolioId = folio.FolioId,
            TransactionType = FolioTransactionType.FoodCharge,
            ReferenceId = foodOrder.FoodOrderId,
            Amount = foodOrder.TotalAmount ?? 0,
            CreatedAt = DateTime.Now
        };

        _context.FolioTransactions.Add(transaction);

        await _context.SaveChangesAsync();

        return FoodOrderActionResult.Succeeded();
    }
}
