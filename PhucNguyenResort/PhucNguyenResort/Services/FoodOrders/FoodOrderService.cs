using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Email;
using PhucNguyenResort.Services.Notifications;
using PhucNguyenResort.ViewModels.FoodOrders;
using System.Net;

namespace PhucNguyenResort.Services.FoodOrders;

public class FoodOrderService : IFoodOrderService
{
    private const int NoteMaxLength = 500;

    private readonly ResortDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly INotificationService _notificationService;

    public FoodOrderService(ResortDbContext context, IEmailSender emailSender, INotificationService notificationService)
    {
        _context = context;
        _emailSender = emailSender;
        _notificationService = notificationService;
    }

    public async Task<MenuCategoryBrowseViewModel> BuildMenuCategoriesAsync(int? bookingId, int? userId)
    {
        var viewModel = new MenuCategoryBrowseViewModel();

        // If user is logged in and has a checked-in booking, populate booking info
        if (userId.HasValue)
        {
            var resolvedBookingId = bookingId;
            if (!resolvedBookingId.HasValue)
            {
                var activeBooking = await _context.Bookings
                    .AsNoTracking()
                    .Where(b => b.UserId == userId.Value && b.BookingStatus == BookingStatus.CheckedIn)
                    .OrderByDescending(b => b.CheckInDate)
                    .FirstOrDefaultAsync();

                resolvedBookingId = activeBooking?.BookingId;
            }

            if (resolvedBookingId.HasValue)
            {
                var booking = await _context.Bookings
                    .AsNoTracking()
                    .Include(b => b.User)
                    .Include(b => b.BookingDetails).ThenInclude(bd => bd.Room)
                    .FirstOrDefaultAsync(b => b.BookingId == resolvedBookingId.Value && b.UserId == userId.Value);

                if (booking != null)
                {
                    viewModel.BookingId = booking.BookingId;
                    viewModel.CustomerName = booking.User?.FullName ?? string.Empty;
                    viewModel.BookingStatus = booking.BookingStatus;
                    viewModel.CanPlaceOrder = booking.BookingStatus == BookingStatus.CheckedIn;
                    viewModel.RoomNumber = string.Join(", ",
                        booking.BookingDetails
                            .Where(bd => bd.RoomId.HasValue && bd.Room != null)
                            .Select(bd => bd.Room!.RoomNumber));
                }
            }
        }

        if (!viewModel.CanPlaceOrder && !userId.HasValue)
        {
            viewModel.Message = "Sign in with a checked-in booking to place a room service order.";
        }
        else if (!viewModel.CanPlaceOrder && viewModel.BookingId == 0)
        {
            viewModel.Message = "You do not currently have a checked-in booking for room service ordering.";
        }

        // Get categories with available item count and first item's image as cover
        viewModel.Categories = await _context.MenuCategories
            .AsNoTracking()
            .Select(c => new MenuCategoryCardViewModel
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ItemCount = c.MenuItems.Count(mi => mi.IsAvailable && !mi.IsDeleted),
                ImageUrl = c.MenuItems
                    .Where(mi => mi.IsAvailable && !mi.IsDeleted && mi.ImageUrl != null)
                    .OrderBy(mi => mi.ItemName)
                    .Select(mi => mi.ImageUrl)
                    .FirstOrDefault()
            })
            .Where(c => c.ItemCount > 0)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        return viewModel;
    }

    public async Task<RoomServiceOrderViewModel> BuildRoomServiceMenuAsync(string? message = null)
    {
        return new RoomServiceOrderViewModel
        {
            CanPlaceOrder = false,
            Message = message,
            MenuItems = await GetAvailableMenuItemsAsync()
        };
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
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.RoomCategory)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
        {
            return null;
        }

        if (currentUserId.HasValue && booking.UserId != currentUserId.Value)
        {
            return null;
        }

        var roomOptions = booking.BookingDetails
            .Where(bd => bd.RoomId.HasValue && bd.Room != null)
            .OrderBy(bd => bd.Room!.RoomNumber)
            .Select(bd => new RoomServiceRoomOptionViewModel
            {
                BookingDetailId = bd.BookingDetailId,
                RoomId = bd.RoomId!.Value,
                RoomNumber = bd.Room!.RoomNumber ?? string.Empty,
                CategoryName = bd.RoomCategory?.CategoryName ?? string.Empty
            })
            .ToList();

        var viewModel = new RoomServiceOrderViewModel
        {
            BookingId = booking.BookingId,
            CustomerName = booking.User?.FullName ?? string.Empty,
            RoomNumber = string.Join(", ", roomOptions.Select(room => room.RoomNumber)),
            BookingStatus = booking.BookingStatus,
            CanPlaceOrder = booking.BookingStatus == BookingStatus.CheckedIn,
            RoomOptions = roomOptions
        };

        if (!viewModel.CanPlaceOrder)
        {
            return null;
        }

        if (viewModel.RoomOptions.Count == 0)
        {
            viewModel.CanPlaceOrder = false;
            viewModel.Message = "This booking does not have an assigned room yet, so room service is not available.";
            return viewModel;
        }

        if (viewModel.RoomOptions.Count == 1)
        {
            viewModel.BookingDetailId = viewModel.RoomOptions[0].BookingDetailId;
        }

        viewModel.MenuItems = await GetAvailableMenuItemsAsync();

        return viewModel;
    }

    private async Task<List<RoomServiceMenuItemViewModel>> GetAvailableMenuItemsAsync()
    {
        return await _context.MenuItems
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
    }

    public async Task<FoodOrderCreateResult> CreateFoodOrderAsync(FoodOrderCreateRequest request, int currentUserId)
    {
        var normalizedCustomerNote = NormalizeNote(request.CustomerNote);
        if (normalizedCustomerNote?.Length > NoteMaxLength)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidNote,
                "Customer note must not exceed 500 characters.");
        }

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

        if (requestedItems.Any(item => NormalizeNote(item.ItemNote)?.Length > NoteMaxLength))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidNote,
                "Food order item notes must not exceed 500 characters.");
        }

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

        if (request.RequestedDeliveryAt.HasValue)
        {
            var requestedDeliveryAt = request.RequestedDeliveryAt.Value;
            if (requestedDeliveryAt <= DateTime.Now)
            {
                return FoodOrderCreateResult.Failed(
                    FoodOrderCreateStatus.InvalidDeliveryTime,
                    "Requested delivery time must be later than the current time.");
            }

            if (requestedDeliveryAt.Date > booking.CheckOutDate.Date)
            {
                return FoodOrderCreateResult.Failed(
                    FoodOrderCreateStatus.InvalidDeliveryTime,
                    "Requested delivery time must not be after the booking checkout date.");
            }
        }

        if (!request.BookingDetailId.HasValue)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.RoomNotSelected,
                "A delivery room must be selected for room service orders.");
        }

        var bookingDetail = await _context.BookingDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(detail => detail.BookingDetailId == request.BookingDetailId.Value);

        if (bookingDetail == null || bookingDetail.BookingId != request.BookingId)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.BookingRoomNotFound,
                "The selected delivery room does not belong to this booking.");
        }

        if (!bookingDetail.RoomId.HasValue)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.BookingRoomNotAssigned,
                "The selected booking room has not been assigned to a physical room.");
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
            OrderType = FoodOrderType.RoomService,
            BookingId = request.BookingId,
            BookingDetailId = request.BookingDetailId,
            OrderStatus = FoodOrderStatus.Pending,
            OrderedAt = DateTime.Now,
            RequestedDeliveryAt = request.RequestedDeliveryAt,
            CustomerNote = normalizedCustomerNote,
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
                UnitPrice = menuItem.Price,
                ItemNote = NormalizeNote(requestedItem.ItemNote)
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
            .Include(order => order.BookingDetail)
                .ThenInclude(bookingDetail => bookingDetail!.Room)
            .Include(order => order.TableReservation)
                .ThenInclude(reservation => reservation!.Table)
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
            OrderType = foodOrder.OrderType,
            BookingId = foodOrder.BookingId,
            TableReservationId = foodOrder.TableReservationId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.BookingDetail?.Room?.RoomNumber ?? string.Empty,
            TableName = foodOrder.TableReservation?.Table?.TableName ?? string.Empty,
            TableNumber = foodOrder.TableReservation?.Table?.TableNumber ?? string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            RequestedDeliveryAt = foodOrder.RequestedDeliveryAt,
            CustomerNote = foodOrder.CustomerNote,
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
                    UnitPrice = item.UnitPrice,
                    ItemNote = item.ItemNote
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
            .Include(order => order.BookingDetail)
                .ThenInclude(bookingDetail => bookingDetail!.Room)
            .Include(order => order.TableReservation)
                .ThenInclude(reservation => reservation!.Table)
            .Include(order => order.Kot)
            .Include(order => order.FoodOrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
                    .ThenInclude(menuItem => menuItem!.Category)
            .OrderByDescending(order => order.OrderedAt)
            .ToListAsync();

        return foodOrders.Select(foodOrder => new FoodOrderDetailViewModel
        {
            FoodOrderId = foodOrder.FoodOrderId,
            OrderType = foodOrder.OrderType,
            BookingId = foodOrder.BookingId,
            TableReservationId = foodOrder.TableReservationId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.BookingDetail?.Room?.RoomNumber ?? string.Empty,
            TableName = foodOrder.TableReservation?.Table?.TableName ?? string.Empty,
            TableNumber = foodOrder.TableReservation?.Table?.TableNumber ?? string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            RequestedDeliveryAt = foodOrder.RequestedDeliveryAt,
            CustomerNote = foodOrder.CustomerNote,
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
                    UnitPrice = item.UnitPrice,
                    ItemNote = item.ItemNote
                })
                .ToList()
        }).ToList();
    }

    public async Task<List<FoodOrderDetailViewModel>> GetFoodOrdersForCustomerAsync(int currentUserId)
    {
        var foodOrders = await _context.FoodOrders
            .AsNoTracking()
            .Include(order => order.Booking)
                .ThenInclude(booking => booking!.User)
            .Include(order => order.BookingDetail)
                .ThenInclude(bookingDetail => bookingDetail!.Room)
            .Include(order => order.TableReservation)
                .ThenInclude(reservation => reservation!.Table)
            .Include(order => order.Kot)
            .Include(order => order.FoodOrderItems)
                .ThenInclude(orderItem => orderItem.MenuItem)
                    .ThenInclude(menuItem => menuItem!.Category)
            .Where(order => order.Booking != null && order.Booking.UserId == currentUserId)
            .OrderByDescending(order => order.OrderedAt)
            .ToListAsync();

        return foodOrders.Select(foodOrder => new FoodOrderDetailViewModel
        {
            FoodOrderId = foodOrder.FoodOrderId,
            OrderType = foodOrder.OrderType,
            BookingId = foodOrder.BookingId,
            TableReservationId = foodOrder.TableReservationId,
            CustomerName = foodOrder.Booking?.User?.FullName ?? string.Empty,
            RoomNumber = foodOrder.BookingDetail?.Room?.RoomNumber ?? string.Empty,
            TableName = foodOrder.TableReservation?.Table?.TableName ?? string.Empty,
            TableNumber = foodOrder.TableReservation?.Table?.TableNumber ?? string.Empty,
            OrderStatus = foodOrder.OrderStatus,
            KitchenStatus = foodOrder.Kot?.Status,
            TotalAmount = foodOrder.TotalAmount ?? 0,
            OrderedAt = foodOrder.OrderedAt,
            RequestedDeliveryAt = foodOrder.RequestedDeliveryAt,
            CustomerNote = foodOrder.CustomerNote,
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
                    UnitPrice = item.UnitPrice,
                    ItemNote = item.ItemNote
                })
                .ToList()
        }).ToList();
    }

    // [UC66] F&B Staff cap nhat trang thai don hang
    public async Task<FoodOrderActionResult> CancelFoodOrderByCustomerAsync(int foodOrderId, int currentUserId)
    {
        var foodOrder = await _context.FoodOrders
            .Include(order => order.Booking)
            .Include(order => order.Kot)
            .FirstOrDefaultAsync(order => order.FoodOrderId == foodOrderId);

        if (foodOrder == null)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.NotFound, "Food order not found.");
        }

        if (foodOrder.Booking == null || foodOrder.Booking.UserId != currentUserId)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.Forbidden, "You are not authorized to cancel this order.");
        }

        if (foodOrder.IsChargedToRoom)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, "Charged orders cannot be cancelled.");
        }

        if (foodOrder.OrderStatus != FoodOrderStatus.Pending)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, "Only pending orders can be cancelled.");
        }

        if (foodOrder.Kot != null && foodOrder.Kot.Status != KotStatus.Pending)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.InvalidState, "The kitchen has already started preparing this order.");
        }

        foodOrder.OrderStatus = FoodOrderStatus.Cancelled;

        if (foodOrder.Kot != null)
        {
            foodOrder.Kot.Status = KotStatus.Cancelled;
        }

        await _context.SaveChangesAsync();

        return FoodOrderActionResult.Succeeded();
    }

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

        if (newStatus == FoodOrderStatus.Cancelled && foodOrder.Kot != null)
        {
            foodOrder.Kot.Status = KotStatus.Cancelled;
        }

        await _context.SaveChangesAsync();
        await SendFoodOrderEmailAsync(
            foodOrder.FoodOrderId,
            "Food order status updated",
            $"Your room service order status has been updated to {newStatus}.");

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

        if (foodOrder.OrderType == FoodOrderType.TableService)
        {
            return FoodOrderActionResult.Failed(
                FoodOrderActionStatus.InvalidState,
                "Table service orders must be charged through table checkout.");
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

        if (!foodOrder.BookingId.HasValue)
        {
            return FoodOrderActionResult.Failed(FoodOrderActionStatus.NoFolio, "This order is not linked to a room booking.");
        }

        var folio = await _context.GuestFolios
            .FirstOrDefaultAsync(f => f.BookingId == foodOrder.BookingId.Value);

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
        await SendFoodOrderEmailAsync(
            foodOrder.FoodOrderId,
            "Food order charged to room",
            "Your delivered room service order has been charged to your room bill.");

        return FoodOrderActionResult.Succeeded();
    }

    private async Task SendFoodOrderEmailAsync(
        int foodOrderId,
        string subject,
        string message,
        string? extraMessage = null)
    {
        try
        {
            var foodOrder = await _context.FoodOrders
                .AsNoTracking()
                .Include(order => order.Booking).ThenInclude(booking => booking!.User)
                .Include(order => order.BookingDetail).ThenInclude(detail => detail!.Room)
                .Include(order => order.FoodOrderItems).ThenInclude(item => item.MenuItem).ThenInclude(menuItem => menuItem!.Category)
                .FirstOrDefaultAsync(order => order.FoodOrderId == foodOrderId);

            if (foodOrder?.Booking?.User == null ||
                string.IsNullOrWhiteSpace(foodOrder.Booking.User.Email))
            {
                return;
            }

            var htmlBody = BuildFoodOrderEmailBody(foodOrder, message, extraMessage);
            await _emailSender.SendEmailAsync(
                foodOrder.Booking.User.Email,
                $"{subject} - Order #{foodOrder.FoodOrderId}",
                htmlBody);

            // Send In-App Notification
            await _notificationService.CreateAsync(
                foodOrder.Booking.UserId,
                subject,
                $"{message} Đơn hàng: #{foodOrder.FoodOrderId}."
            );
        }
        catch
        {
            // Email delivery must not block F&B staff operations.
        }
    }

    private static string BuildFoodOrderEmailBody(
        FoodOrder foodOrder,
        string message,
        string? extraMessage)
    {
        static string H(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

        var roomNumber = foodOrder.BookingDetail?.Room?.RoomNumber ?? "Not assigned";
        var requestedDeliveryAt = foodOrder.RequestedDeliveryAt.HasValue
            ? foodOrder.RequestedDeliveryAt.Value.ToString("dd/MM/yyyy HH:mm")
            : "As soon as possible";
        var chargedInfo = foodOrder.IsChargedToRoom
            ? $"Charged at {foodOrder.ChargedAt?.ToString("dd/MM/yyyy HH:mm")}"
            : "Not charged";
        var itemRows = string.Join(
            string.Empty,
            foodOrder.FoodOrderItems
                .OrderBy(item => item.OrderItemId)
                .Select(item =>
                {
                    var itemName = item.MenuItem?.ItemName ?? $"Menu item #{item.MenuItemId}";
                    var categoryName = item.MenuItem?.Category?.CategoryName ?? string.Empty;
                    var lineTotal = item.UnitPrice * item.Quantity;
                    var itemNote = string.IsNullOrWhiteSpace(item.ItemNote)
                        ? string.Empty
                        : $"<div style=\"color:#64748b;font-size:13px;margin-top:4px;\">Note: {H(item.ItemNote)}</div>";

                    return $@"
                    <tr>
                        <td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">
                            <strong>{H(itemName)}</strong>
                            {(string.IsNullOrWhiteSpace(categoryName) ? string.Empty : $"<div style=\"color:#64748b;font-size:13px;\">{H(categoryName)}</div>")}
                            {itemNote}
                        </td>
                        <td style=""padding:8px;border-bottom:1px solid #e5e7eb;text-align:center;"">{item.Quantity}</td>
                        <td style=""padding:8px;border-bottom:1px solid #e5e7eb;text-align:right;"">{item.UnitPrice:N0} VND</td>
                        <td style=""padding:8px;border-bottom:1px solid #e5e7eb;text-align:right;"">{lineTotal:N0} VND</td>
                    </tr>";
                }));

        return $@"
<!DOCTYPE html>
<html>
<body style=""font-family:Arial,sans-serif;background:#f6f7fb;margin:0;padding:24px;color:#1f2937;"">
    <div style=""max-width:720px;margin:0 auto;background:#ffffff;border:1px solid #e5e7eb;border-radius:8px;overflow:hidden;"">
        <div style=""background:#dfa974;color:#ffffff;padding:18px 24px;"">
            <h2 style=""margin:0;font-size:22px;"">PhucNguyen Resort</h2>
            <p style=""margin:6px 0 0;"">Room Service Food Order Update</p>
        </div>
        <div style=""padding:24px;"">
            <p>Dear {H(foodOrder.Booking?.User?.FullName)},</p>
            <p>{H(message)}</p>
            {(string.IsNullOrWhiteSpace(extraMessage) ? string.Empty : $"<p><strong>Note:</strong> {H(extraMessage)}</p>")}
            <table style=""width:100%;border-collapse:collapse;margin-top:18px;"">
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Order ID</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">#{foodOrder.FoodOrderId}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Status</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(foodOrder.OrderStatus.ToString())}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Room</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(roomNumber)}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Ordered At</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{foodOrder.OrderedAt:dd/MM/yyyy HH:mm}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Requested Delivery</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(requestedDeliveryAt)}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Room Bill</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(chargedInfo)}</td></tr>
            </table>
            {(string.IsNullOrWhiteSpace(foodOrder.CustomerNote) ? string.Empty : $"<p style=\"margin-top:18px;\"><strong>Customer Note:</strong> {H(foodOrder.CustomerNote)}</p>")}
            <table style=""width:100%;border-collapse:collapse;margin-top:18px;"">
                <thead>
                    <tr>
                        <th style=""padding:8px;border-bottom:2px solid #e5e7eb;text-align:left;"">Item</th>
                        <th style=""padding:8px;border-bottom:2px solid #e5e7eb;text-align:center;"">Qty</th>
                        <th style=""padding:8px;border-bottom:2px solid #e5e7eb;text-align:right;"">Unit Price</th>
                        <th style=""padding:8px;border-bottom:2px solid #e5e7eb;text-align:right;"">Line Total</th>
                    </tr>
                </thead>
                <tbody>
                    {itemRows}
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan=""3"" style=""padding:10px;text-align:right;""><strong>Total Amount</strong></td>
                        <td style=""padding:10px;text-align:right;""><strong>{foodOrder.TotalAmount:N0} VND</strong></td>
                    </tr>
                </tfoot>
            </table>
            <p style=""margin-top:24px;color:#64748b;"">Thank you for choosing PhucNguyen Resort.</p>
        </div>
    </div>
</body>
</html>";
    }

    private static string? NormalizeNote(string? note)
    {
        var trimmedNote = note?.Trim();
        return string.IsNullOrWhiteSpace(trimmedNote) ? null : trimmedNote;
    }
}
