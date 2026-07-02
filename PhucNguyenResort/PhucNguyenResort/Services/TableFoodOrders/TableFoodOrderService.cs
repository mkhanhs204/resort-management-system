using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.ViewModels.TableFoodOrders;

namespace PhucNguyenResort.Services.TableFoodOrders;

public class TableFoodOrderService : ITableFoodOrderService
{
    private const int NoteMaxLength = 500;

    private readonly ResortDbContext _context;

    public TableFoodOrderService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<TableFoodOrderCreateViewModel?> BuildCreateOrderAsync(int reservationId)
    {
        var reservation = await _context.TableReservations
            .AsNoTracking()
            .Include(r => r.Table)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null || reservation.Status != TableReservationStatus.Seated)
        {
            return null;
        }

        var menuItems = await _context.MenuItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.IsAvailable && !item.IsDeleted)
            .OrderBy(item => item.Category!.CategoryName)
            .ThenBy(item => item.ItemName)
            .Select(item => new TableFoodOrderMenuItemViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                CategoryName = item.Category != null ? item.Category.CategoryName : "Uncategorized",
                ImageUrl = item.ImageUrl,
                Price = item.Price
            })
            .ToListAsync();

        return new TableFoodOrderCreateViewModel
        {
            ReservationId = reservation.ReservationId,
            ReservationCode = reservation.ReservationCode,
            TableName = reservation.Table?.TableName ?? string.Empty,
            TableNumber = reservation.Table?.TableNumber ?? string.Empty,
            PartySize = reservation.PartySize,
            IsChargedToRoom = reservation.IsChargedToRoom,
            Items = menuItems
        };
    }

    public async Task<FoodOrderCreateResult> CreateTableFoodOrderAsync(TableFoodOrderCreateViewModel model, int staffUserId)
    {
        if (staffUserId <= 0)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.Forbidden,
                "Staff user is required to create a table food order.");
        }

        var reservation = await _context.TableReservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReservationId == model.ReservationId);

        if (reservation == null)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.ReservationNotFound,
                "Table reservation was not found.");
        }

        if (reservation.Status != TableReservationStatus.Seated)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.ReservationNotSeated,
                "Food orders can only be created after the table is seated.");
        }

        var normalizedCustomerNote = NormalizeNote(model.CustomerNote);
        if (normalizedCustomerNote?.Length > NoteMaxLength)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidNote,
                "Order note must not exceed 500 characters.");
        }

        var requestedItems = model.Items
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

        if (requestedItems.Any(item => NormalizeNote(item.ItemNote)?.Length > NoteMaxLength))
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.InvalidNote,
                "Food order item notes must not exceed 500 characters.");
        }

        var duplicateItemId = requestedItems
            .GroupBy(item => item.MenuItemId)
            .Any(group => group.Count() > 1);

        if (duplicateItemId)
        {
            return FoodOrderCreateResult.Failed(
                FoodOrderCreateStatus.DuplicateMenuItem,
                "Duplicate menu items are not allowed in the same order.");
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
            OrderType = FoodOrderType.TableService,
            BookingId = reservation.BookingId,
            BookingDetailId = null,
            TableReservationId = reservation.ReservationId,
            OrderStatus = FoodOrderStatus.Pending,
            OrderedAt = DateTime.Now,
            CustomerNote = normalizedCustomerNote,
            IsChargedToRoom = false,
            IsPaidAtRestaurant = false,
            Kot = new Kot
            {
                Status = KotStatus.Pending,
                CreatedAt = DateTime.Now
            }
        };

        foreach (var requestedItem in requestedItems)
        {
            var menuItem = menuItems[requestedItem.MenuItemId];
            foodOrder.FoodOrderItems.Add(new FoodOrderItem
            {
                MenuItemId = menuItem.MenuItemId,
                Quantity = requestedItem.Quantity,
                UnitPrice = menuItem.Price,
                ItemNote = NormalizeNote(requestedItem.ItemNote)
            });
        }

        foodOrder.TotalAmount = foodOrder.FoodOrderItems
            .Sum(item => item.Quantity * item.UnitPrice);

        _context.FoodOrders.Add(foodOrder);
        await _context.SaveChangesAsync();

        return FoodOrderCreateResult.Created(foodOrder.FoodOrderId, foodOrder.TotalAmount.Value);
    }

    public async Task<IReadOnlyList<TableFoodOrderListViewModel>> GetOrdersByReservationAsync(int reservationId)
    {
        return await _context.FoodOrders
            .AsNoTracking()
            .Include(order => order.TableReservation)
                .ThenInclude(reservation => reservation!.Table)
            .Include(order => order.Kot)
            .Where(order => order.OrderType == FoodOrderType.TableService && order.TableReservationId == reservationId)
            .OrderByDescending(order => order.OrderedAt)
            .Select(order => new TableFoodOrderListViewModel
            {
                FoodOrderId = order.FoodOrderId,
                ReservationId = order.TableReservationId!.Value,
                ReservationCode = order.TableReservation != null ? order.TableReservation.ReservationCode : string.Empty,
                TableName = order.TableReservation != null && order.TableReservation.Table != null ? order.TableReservation.Table.TableName : string.Empty,
                TableNumber = order.TableReservation != null && order.TableReservation.Table != null ? order.TableReservation.Table.TableNumber ?? string.Empty : string.Empty,
                PartySize = order.TableReservation != null ? order.TableReservation.PartySize : 0,
                OrderStatus = order.OrderStatus,
                KitchenStatus = order.Kot != null ? order.Kot.Status : null,
                OrderedAt = order.OrderedAt,
                TotalAmount = order.TotalAmount ?? 0,
                IsChargedToRoom = order.IsChargedToRoom,
                IsPaidAtRestaurant = order.IsPaidAtRestaurant
            })
            .ToListAsync();
    }

    public async Task<TableFoodOrderCheckoutViewModel?> BuildCheckoutAsync(int reservationId)
    {
        var reservation = await _context.TableReservations
            .Include(r => r.Table)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return null;
        }

        var orders = await GetOrdersByReservationAsync(reservationId);
        var billableOrders = orders
            .Where(order => order.OrderStatus != FoodOrderStatus.Cancelled)
            .ToList();
        var bookingOptions = await GetEligibleBookingOptionsAsync(
            reservation.UserId,
            reservation.ReservationDate);
        var selectedBookingId = reservation.BookingId.HasValue
            ? reservation.BookingId
            : bookingOptions.Count == 1 ? bookingOptions[0].BookingId : null;

        if (!reservation.BookingId.HasValue && selectedBookingId.HasValue && bookingOptions.Count == 1)
        {
            reservation.BookingId = selectedBookingId.Value;
            await _context.SaveChangesAsync();
        }

        var linkedBookingDisplay = selectedBookingId.HasValue
            ? bookingOptions.FirstOrDefault(option => option.BookingId == selectedBookingId.Value)?.DisplayText
            : null;

        return new TableFoodOrderCheckoutViewModel
        {
            ReservationId = reservation.ReservationId,
            ReservationCode = reservation.ReservationCode,
            TableName = reservation.Table?.TableName ?? string.Empty,
            TableNumber = reservation.Table?.TableNumber ?? string.Empty,
            PartySize = reservation.PartySize,
            BookingId = reservation.BookingId,
            SelectedBookingId = selectedBookingId,
            LinkedBookingDisplay = linkedBookingDisplay,
            BookingOptions = bookingOptions,
            CanChargeToRoom = bookingOptions.Count > 0 &&
                billableOrders.Count > 0 &&
                billableOrders.All(order => !order.IsChargedToRoom && !order.IsPaidAtRestaurant),
            IsChargedToRoom = billableOrders.Count > 0 && billableOrders.All(order => order.IsChargedToRoom),
            IsPaidAtRestaurant = billableOrders.Count > 0 && billableOrders.All(order => order.IsPaidAtRestaurant),
            TotalAmount = billableOrders.Sum(order => order.TotalAmount),
            Orders = orders.ToList()
        };
    }

    public async Task<TableFoodOrderActionResult> ChargeToRoomAsync(int reservationId, int bookingId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var reservation = await _context.TableReservations
            .Include(r => r.Booking)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.ReservationNotFound,
                "Table reservation was not found.");
        }

        var orders = await GetBillableTrackedOrdersAsync(reservationId);
        var validation = ValidateOrdersForCheckout(reservation, orders);
        if (validation != null)
        {
            return validation;
        }

        var selectedBooking = await GetEligibleBookingForReservationAsync(
            reservation.UserId,
            reservation.ReservationDate,
            bookingId);

        if (selectedBooking == null)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.NoLinkedBooking,
                "Selected booking is not eligible for room charge.");
        }

        if (reservation.BookingId.HasValue && reservation.BookingId.Value != bookingId)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.InvalidState,
                "This reservation is already linked to a different booking.");
        }

        var folio = await _context.GuestFolios
            .FirstOrDefaultAsync(folio => folio.BookingId == selectedBooking.BookingId);

        if (folio == null)
        {
            folio = new GuestFolio
            {
                BookingId = selectedBooking.BookingId,
                TotalRoomCharges = 0,
                TotalFoodCharges = 0,
                TotalTourCharges = 0,
                TotalAmount = 0,
                Status = FolioStatus.Unpaid
            };

            _context.GuestFolios.Add(folio);
            await _context.SaveChangesAsync();
        }

        var totalAmount = orders.Sum(order => order.TotalAmount ?? 0);
        var chargedAt = DateTime.Now;

        foreach (var order in orders)
        {
            order.BookingId = selectedBooking.BookingId;
            order.IsChargedToRoom = true;
            order.ChargedAt = chargedAt;

            _context.FolioTransactions.Add(new FolioTransaction
            {
                FolioId = folio.FolioId,
                TransactionType = FolioTransactionType.FoodCharge,
                ReferenceId = order.FoodOrderId,
                Amount = order.TotalAmount ?? 0,
                CreatedAt = chargedAt
            });
        }

        folio.TotalFoodCharges += totalAmount;
        folio.TotalAmount += totalAmount;
        reservation.BookingId = selectedBooking.BookingId;
        reservation.IsChargedToRoom = true;
        reservation.ChargedAmount = totalAmount;
        reservation.ChargedAt = chargedAt;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return TableFoodOrderActionResult.Succeeded();
    }

    public async Task<TableFoodOrderActionResult> PayAtRestaurantAsync(int reservationId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var reservation = await _context.TableReservations
            .Include(r => r.Booking)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.ReservationNotFound,
                "Table reservation was not found.");
        }

        var orders = await GetBillableTrackedOrdersAsync(reservationId);
        var validation = ValidateOrdersForCheckout(reservation, orders);
        if (validation != null)
        {
            return validation;
        }

        var eligibleBookingOptions = await GetEligibleBookingOptionsAsync(
            reservation.UserId,
            reservation.ReservationDate);

        if (IsEligibleRoomBooking(reservation.Booking) || eligibleBookingOptions.Count > 0)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.InvalidState,
                "This reservation has an arrived or checked-in booking and should be charged to room.");
        }

        var paidAt = DateTime.Now;
        foreach (var order in orders)
        {
            order.IsPaidAtRestaurant = true;
            order.PaidAt = paidAt;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return TableFoodOrderActionResult.Succeeded();
    }

    private async Task<List<FoodOrder>> GetBillableTrackedOrdersAsync(int reservationId)
    {
        return await _context.FoodOrders
            .Where(order =>
                order.OrderType == FoodOrderType.TableService &&
                order.TableReservationId == reservationId &&
                order.OrderStatus != FoodOrderStatus.Cancelled)
            .ToListAsync();
    }

    private static TableFoodOrderActionResult? ValidateOrdersForCheckout(
        TableReservation reservation,
        IReadOnlyCollection<FoodOrder> orders)
    {
        if (reservation.Status != TableReservationStatus.Seated)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.ReservationNotSeated,
                "Table reservation must be seated before checkout.");
        }

        if (orders.Count == 0)
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.NoOrders,
                "There are no billable table food orders for this reservation.");
        }

        if (orders.Any(order => order.OrderStatus != FoodOrderStatus.Delivered))
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.InvalidState,
                "All billable food orders must be delivered before checkout.");
        }

        if (orders.Any(order => order.IsChargedToRoom))
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.AlreadyCharged,
                "One or more table food orders have already been charged to room.");
        }

        if (orders.Any(order => order.IsPaidAtRestaurant))
        {
            return TableFoodOrderActionResult.Failed(
                TableFoodOrderActionStatus.AlreadyPaid,
                "One or more table food orders have already been paid at restaurant.");
        }

        return null;
    }

    private async Task<List<TableFoodOrderBookingOptionViewModel>> GetEligibleBookingOptionsAsync(
        int userId,
        DateTime reservationDate)
    {
        var bookings = await _context.Bookings
            .AsNoTracking()
            .Include(booking => booking.BookingDetails)
                .ThenInclude(detail => detail.Room)
            .Where(booking =>
                booking.UserId == userId &&
                (booking.BookingStatus == BookingStatus.CheckedIn ||
                 (booking.BookingStatus == BookingStatus.Arrived &&
                  booking.CheckInDate.Date <= reservationDate.Date &&
                  booking.CheckOutDate.Date >= reservationDate.Date)))
            .OrderByDescending(booking => booking.BookingStatus == BookingStatus.CheckedIn)
            .ThenByDescending(booking => booking.CheckInDate)
            .ToListAsync();

        return bookings
            .Select(booking =>
            {
                var rooms = booking.BookingDetails
                    .Where(detail => detail.Room != null && !string.IsNullOrWhiteSpace(detail.Room.RoomNumber))
                    .Select(detail => detail.Room!.RoomNumber)
                    .ToList();
                var roomDisplay = rooms.Count == 0 ? "Not assigned" : string.Join(", ", rooms);

                return new TableFoodOrderBookingOptionViewModel
                {
                    BookingId = booking.BookingId,
                    DisplayText = $"Booking #{booking.BookingId} - {booking.BookingStatus} - {booking.CheckInDate:dd/MM/yyyy} to {booking.CheckOutDate:dd/MM/yyyy} - Rooms: {roomDisplay}"
                };
            })
            .ToList();
    }

    private async Task<Booking?> GetEligibleBookingForReservationAsync(
        int userId,
        DateTime reservationDate,
        int bookingId)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(booking =>
                booking.BookingId == bookingId &&
                booking.UserId == userId &&
                (booking.BookingStatus == BookingStatus.CheckedIn ||
                 (booking.BookingStatus == BookingStatus.Arrived &&
                  booking.CheckInDate.Date <= reservationDate.Date &&
                  booking.CheckOutDate.Date >= reservationDate.Date)));
    }

    private static bool IsEligibleRoomBooking(Booking? booking)
    {
        return booking?.BookingStatus is BookingStatus.Arrived or BookingStatus.CheckedIn;
    }

    private static string? NormalizeNote(string? note)
    {
        var normalized = note?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
