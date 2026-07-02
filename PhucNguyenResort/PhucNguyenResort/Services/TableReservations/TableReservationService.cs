using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Email;
using PhucNguyenResort.ViewModels.TableReservations;
using System.Data;
using System.Net;

namespace PhucNguyenResort.Services.TableReservations;

public class TableReservationService : ITableReservationService
{
    private static readonly TimeSpan OpeningTime = new(6, 0, 0);
    private static readonly TimeSpan ClosingTime = new(22, 0, 0);
    private const int DefaultDurationMinutes = 120;
    private const int NoShowGraceMinutes = 30;
    private const string TableHoldPolicyMessage =
        "Please arrive within 30 minutes of your reservation time. After this hold period, the table may be released or marked as no-show.";

    private readonly ResortDbContext _context;
    private readonly IEmailSender _emailSender;

    public TableReservationService(ResortDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task<IReadOnlyList<AvailableTableViewModel>> SearchAvailableTablesAsync(
        TableReservationSearchViewModel search)
    {
        if (!search.ReservationDate.HasValue ||
            !search.ReservationTime.HasValue ||
            search.PartySize <= 0)
        {
            return Array.Empty<AvailableTableViewModel>();
        }

        var reservationDate = search.ReservationDate.Value.Date;
        var reservationTime = search.ReservationTime.Value;
        var durationMinutes = DefaultDurationMinutes;
        var requestedStart = BuildReservationStart(reservationDate, reservationTime);

        if (requestedStart <= DateTime.Now ||
            IsOutsideOperatingHours(reservationTime, durationMinutes))
        {
            return Array.Empty<AvailableTableViewModel>();
        }

        var activeReservations = await _context.TableReservations
            .Where(r => r.ReservationDate.Date == reservationDate &&
                        (r.Status == TableReservationStatus.Pending ||
                         r.Status == TableReservationStatus.Confirmed))
            .ToListAsync();

        var requestedEnd = BuildReservationEnd(reservationDate, reservationTime, durationMinutes);
        var conflictedTableIds = activeReservations
            .Where(r => HasTimeOverlap(r, requestedStart, requestedEnd))
            .Select(r => r.TableId)
            .ToHashSet();

        return await _context.DiningTables
            .Where(t => t.Status != DiningTableStatus.Maintenance &&
                        t.IsReservable &&
                        t.Capacity.HasValue &&
                        t.Capacity.Value >= search.PartySize &&
                        !conflictedTableIds.Contains(t.TableId))
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.Capacity)
            .ThenBy(t => t.TableNumber)
            .Select(t => new AvailableTableViewModel
            {
                TableId = t.TableId,
                TableNumber = t.TableNumber ?? string.Empty,
                TableName = t.TableName,
                ImageUrl = t.ImageUrl,
                Description = t.Description,
                Area = t.Area,
                TableType = t.TableType,
                LocationNote = t.LocationNote,
                Capacity = t.Capacity ?? 0,
                ReservationDate = reservationDate,
                ReservationTime = reservationTime,
                DurationMinutes = durationMinutes
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyList<PublicDiningTableViewModel>> GetPublicDiningTablesAsync()
    {
        return await _context.DiningTables
            .Where(t => t.Status != DiningTableStatus.Maintenance && t.IsReservable)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.Capacity)
            .ThenBy(t => t.TableNumber)
            .Select(t => new PublicDiningTableViewModel
            {
                TableId = t.TableId,
                TableNumber = t.TableNumber ?? string.Empty,
                TableName = t.TableName,
                ImageUrl = t.ImageUrl,
                Description = t.Description,
                Area = t.Area,
                TableType = t.TableType,
                LocationNote = t.LocationNote,
                Capacity = t.Capacity ?? 0,
                Status = t.Status,
                IsReservable = t.IsReservable
            })
            .ToListAsync();
    }

    public async Task<TableReservationCreateResult> CreateReservationAsync(
        TableReservationCreateViewModel model,
        int currentUserId)
    {
        if (currentUserId <= 0)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.Unauthorized,
                "You must sign in before reserving a table.");
        }

        if (model.PartySize <= 0)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.InvalidPartySize,
                "Party size must be greater than zero.");
        }

        var reservationDate = model.ReservationDate.Date;
        var reservationTime = model.ReservationTime;
        var durationMinutes = DefaultDurationMinutes;

        var requestedStart = BuildReservationStart(reservationDate, reservationTime);
        if (requestedStart <= DateTime.Now)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.InvalidReservationTime,
                "Reservation time must be in the future.");
        }

        if (IsOutsideOperatingHours(reservationTime, durationMinutes))
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.OutsideOperatingHours,
                "Reservation time is outside restaurant operating hours.");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var table = await _context.DiningTables.FindAsync(model.TableId);
        if (table == null)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.TableNotFound,
                "Selected table was not found.");
        }

        if (table.Status == DiningTableStatus.Maintenance || !table.IsReservable)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.TableUnavailable,
                "Selected table is not available for reservation.");
        }

        if (!table.Capacity.HasValue || model.PartySize > table.Capacity.Value)
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.CapacityExceeded,
                "Party size exceeds selected table capacity.");
        }

        if (await HasTableConflictAsync(table.TableId, reservationDate, reservationTime, durationMinutes))
        {
            return TableReservationCreateResult.Failed(
                TableReservationCreateStatus.Conflict,
                "Selected table is already reserved for this time slot.");
        }

        var eligibleBooking = await ResolveChargeableBookingForReservationAsync(
            currentUserId,
            reservationDate);

        var reservation = new TableReservation
        {
            UserId = currentUserId,
            TableId = table.TableId,
            BookingId = eligibleBooking?.BookingId,
            ReservationDate = reservationDate,
            ReservationTime = reservationTime,
            DurationMinutes = durationMinutes,
            PartySize = model.PartySize,
            SpecialRequests = NormalizeText(model.SpecialRequests),
            Status = TableReservationStatus.Pending,
            CreatedAt = DateTime.Now,
            ReservationCode = await GenerateReservationCodeAsync(reservationDate)
        };

        _context.TableReservations.Add(reservation);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation request received",
            "We have received your table reservation request. Our F&B staff will review and confirm it shortly.");

        return TableReservationCreateResult.Created(reservation.ReservationId);
    }

    public async Task<TableReservationDetailViewModel?> GetReservationDetailAsync(
        int reservationId,
        int currentUserId)
    {
        return await _context.TableReservations
            .Include(r => r.Table)
            .Where(r => r.ReservationId == reservationId && r.UserId == currentUserId)
            .Select(r => new TableReservationDetailViewModel
            {
                ReservationId = r.ReservationId,
                ReservationCode = r.ReservationCode,
                BookingId = r.BookingId,
                TableNumber = r.Table != null ? r.Table.TableNumber ?? string.Empty : string.Empty,
                TableName = r.Table != null ? r.Table.TableName : string.Empty,
                ImageUrl = r.Table != null ? r.Table.ImageUrl : null,
                Description = r.Table != null ? r.Table.Description : null,
                Area = r.Table != null ? r.Table.Area : null,
                TableType = r.Table != null ? r.Table.TableType : null,
                LocationNote = r.Table != null ? r.Table.LocationNote : null,
                Capacity = r.Table != null ? r.Table.Capacity ?? 0 : 0,
                ReservationDate = r.ReservationDate,
                ReservationTime = r.ReservationTime,
                DurationMinutes = r.DurationMinutes,
                PartySize = r.PartySize,
                SpecialRequests = r.SpecialRequests,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                ConfirmedAt = r.ConfirmedAt,
                CancelledAt = r.CancelledAt,
                CancellationReason = r.CancellationReason,
                IsChargedToRoom = r.IsChargedToRoom,
                ChargedAmount = r.ChargedAmount,
                ChargedAt = r.ChargedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TableReservationListViewModel>> GetReservationsForCustomerAsync(
        int currentUserId)
    {
        var reservations = await _context.TableReservations
            .Include(r => r.Table)
            .Where(r => r.UserId == currentUserId)
            .OrderByDescending(r => r.ReservationDate)
            .ThenByDescending(r => r.ReservationTime)
            .ToListAsync();

        return reservations.Select(r => new TableReservationListViewModel
        {
            ReservationId = r.ReservationId,
            ReservationCode = r.ReservationCode,
            TableNumber = r.Table?.TableNumber ?? string.Empty,
            TableName = r.Table?.TableName ?? string.Empty,
            Area = r.Table?.Area,
            TableType = r.Table?.TableType,
            ReservationDate = r.ReservationDate,
            ReservationTime = r.ReservationTime,
            PartySize = r.PartySize,
            Status = r.Status,
            CanCancel = CanCancel(r)
        }).ToList();
    }

    public async Task<bool> HasTableConflictAsync(
        int tableId,
        DateTime reservationDate,
        TimeSpan reservationTime,
        int durationMinutes,
        int? excludeReservationId = null)
    {
        var normalizedDate = reservationDate.Date;
        var normalizedDuration = durationMinutes > 0
            ? durationMinutes
            : DefaultDurationMinutes;
        var requestedStart = BuildReservationStart(normalizedDate, reservationTime);
        var requestedEnd = BuildReservationEnd(normalizedDate, reservationTime, normalizedDuration);

        var query = _context.TableReservations
            .Where(r => r.TableId == tableId &&
                        r.ReservationDate.Date == normalizedDate &&
                        (r.Status == TableReservationStatus.Pending ||
                         r.Status == TableReservationStatus.Confirmed));

        if (excludeReservationId.HasValue)
        {
            query = query.Where(r => r.ReservationId != excludeReservationId.Value);
        }

        var activeReservations = await query.ToListAsync();
        return activeReservations.Any(r => HasTimeOverlap(r, requestedStart, requestedEnd));
    }

    public async Task<bool> CancelReservationAsync(
        int reservationId,
        int currentUserId,
        string? reason)
    {
        var reservation = await _context.TableReservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == currentUserId);

        if (reservation == null || !CanCancel(reservation))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.Cancelled;
        reservation.CancelledAt = DateTime.Now;
        reservation.CancellationReason = NormalizeText(reason);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<FnBTableReservationDashboardViewModel> GetFnBDashboardAsync(DateTime date)
    {
        var normalizedDate = date.Date;
        var todayReservations = await GetStaffReservationsAsync(normalizedDate, null);

        return new FnBTableReservationDashboardViewModel
        {
            TotalTables = await _context.DiningTables.CountAsync(),
            AvailableTables = await _context.DiningTables.CountAsync(t => t.Status == DiningTableStatus.Available),
            OccupiedTables = await _context.DiningTables.CountAsync(t => t.Status == DiningTableStatus.Occupied),
            PendingReservationsToday = todayReservations.Count(r => r.Status == TableReservationStatus.Pending),
            ConfirmedReservationsToday = todayReservations.Count(r => r.Status == TableReservationStatus.Confirmed),
            TodayReservations = todayReservations
        };
    }

    public async Task<IReadOnlyList<TableReservationStaffListViewModel>> GetStaffReservationsAsync(
        DateTime? date = null,
        TableReservationStatus? status = null)
    {
        var query = _context.TableReservations
            .Include(r => r.Table)
            .Include(r => r.User)
            .Include(r => r.Booking)
            .AsQueryable();

        if (date.HasValue)
        {
            var normalizedDate = date.Value.Date;
            query = query.Where(r => r.ReservationDate.Date == normalizedDate);
        }

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        var reservations = await query
            .OrderBy(r => r.ReservationDate)
            .ThenBy(r => r.ReservationTime)
            .ThenBy(r => r.Table != null ? r.Table.TableNumber : string.Empty)
            .ToListAsync();

        var incompleteCheckoutReservationIds = await GetIncompleteCheckoutReservationIdsAsync(reservations);

        return reservations
            .Select(reservation => MapStaffReservation(
                reservation,
                !incompleteCheckoutReservationIds.Contains(reservation.ReservationId)))
            .ToList();
    }

    public async Task<bool> ConfirmReservationAsync(int reservationId, int staffUserId)
    {
        if (staffUserId <= 0)
        {
            return false;
        }

        var reservation = await _context.TableReservations.FindAsync(reservationId);
        if (reservation == null || !CanConfirm(reservation))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.Confirmed;
        reservation.ConfirmedAt = DateTime.Now;
        reservation.ConfirmedByUserId = staffUserId;

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation confirmed",
            "Your table reservation has been confirmed by our F&B staff.");

        return true;
    }

    public async Task<bool> CompleteReservationAsync(int reservationId)
    {
        var reservation = await _context.TableReservations.FindAsync(reservationId);
        if (reservation == null || !CanComplete(reservation))
        {
            return false;
        }

        if (!await IsTableBillSettledAsync(reservation.ReservationId))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.Completed;

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation completed",
            "Your table reservation has been marked as completed. Thank you for dining with us.");

        return true;
    }

    public async Task<bool> SeatReservationAsync(int reservationId)
    {
        var reservation = await _context.TableReservations.FindAsync(reservationId);
        if (reservation == null || !CanSeat(reservation))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.Seated;

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation seated",
            "Your table reservation has been marked as seated. We hope you enjoy your dining experience.");

        return true;
    }

    public async Task<bool> MarkReservationNoShowAsync(int reservationId)
    {
        var reservation = await _context.TableReservations.FindAsync(reservationId);
        if (reservation == null || !CanMarkNoShow(reservation))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.NoShow;

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation marked as no-show",
            "Your table reservation has been marked as no-show because the reserved time has passed.");

        return true;
    }

    public async Task<bool> CancelReservationByStaffAsync(
        int reservationId,
        int staffUserId,
        string? reason)
    {
        if (staffUserId <= 0)
        {
            return false;
        }

        var reservation = await _context.TableReservations.FindAsync(reservationId);
        if (reservation == null || !CanCancel(reservation))
        {
            return false;
        }

        reservation.Status = TableReservationStatus.Cancelled;
        reservation.CancelledAt = DateTime.Now;
        reservation.CancellationReason = NormalizeText(reason);

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Table reservation cancelled",
            "Your table reservation has been cancelled by our F&B staff.",
            reservation.CancellationReason);

        return true;
    }

    public async Task<TableReservationActionResult> ChargeReservationToRoomAsync(
        int reservationId,
        decimal amount)
    {
        if (amount <= 0)
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.InvalidAmount,
                "Charge amount must be greater than zero.");
        }

        var reservation = await _context.TableReservations
            .Include(r => r.Booking)
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.NotFound,
                "Table reservation was not found.");
        }

        if (reservation.Status != TableReservationStatus.Completed)
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.InvalidState,
                "Only completed table reservations can be charged to a room.");
        }

        if (reservation.IsChargedToRoom)
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.AlreadyCharged,
                "This table reservation has already been charged to room.");
        }

        if (!reservation.BookingId.HasValue ||
            reservation.Booking == null ||
            !IsChargeableBookingStatus(reservation.Booking.BookingStatus))
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.NoCheckedInBooking,
                "This reservation is not linked to an arrived or checked-in hotel booking.");
        }

        var folio = await _context.GuestFolios
            .FirstOrDefaultAsync(f => f.BookingId == reservation.BookingId.Value);

        if (folio == null)
        {
            return TableReservationActionResult.Failed(
                TableReservationActionStatus.NoFolio,
                "No guest folio found for this booking.");
        }

        reservation.IsChargedToRoom = true;
        reservation.ChargedAmount = amount;
        reservation.ChargedAt = DateTime.Now;

        folio.TotalFoodCharges += amount;
        folio.TotalAmount += amount;

        _context.FolioTransactions.Add(new FolioTransaction
        {
            FolioId = folio.FolioId,
            TransactionType = FolioTransactionType.FoodCharge,
            ReferenceId = reservation.ReservationId,
            Amount = amount,
            CreatedAt = DateTime.Now
        });

        await _context.SaveChangesAsync();
        await SendReservationEmailAsync(
            reservation.ReservationId,
            "Restaurant charge added to room bill",
            "Your restaurant charge has been added to your room bill.",
            $"Charge amount: {amount:N0} VND");

        return TableReservationActionResult.Succeeded();
    }

    private static DateTime BuildReservationStart(DateTime date, TimeSpan time)
    {
        return date.Date.Add(time);
    }

    private static DateTime BuildReservationEnd(DateTime date, TimeSpan time, int durationMinutes)
    {
        return BuildReservationStart(date, time).AddMinutes(durationMinutes);
    }

    private static bool IsOutsideOperatingHours(TimeSpan time, int durationMinutes)
    {
        var normalizedDuration = durationMinutes > 0
            ? durationMinutes
            : DefaultDurationMinutes;
        var endTime = time.Add(TimeSpan.FromMinutes(normalizedDuration));

        return time < OpeningTime || endTime > ClosingTime;
    }

    private static bool HasTimeOverlap(
        TableReservation existingReservation,
        DateTime requestedStart,
        DateTime requestedEnd)
    {
        var existingStart = BuildReservationStart(
            existingReservation.ReservationDate,
            existingReservation.ReservationTime);
        var existingEnd = BuildReservationEnd(
            existingReservation.ReservationDate,
            existingReservation.ReservationTime,
            existingReservation.DurationMinutes);

        return existingStart < requestedEnd && requestedStart < existingEnd;
    }

    private static bool CanCancel(TableReservation reservation)
    {
        if (reservation.Status != TableReservationStatus.Pending &&
            reservation.Status != TableReservationStatus.Confirmed)
        {
            return false;
        }

        var reservationStart = BuildReservationStart(
            reservation.ReservationDate,
            reservation.ReservationTime);

        return reservationStart > DateTime.Now;
    }

    private static bool CanConfirm(TableReservation reservation)
    {
        if (reservation.Status != TableReservationStatus.Pending)
        {
            return false;
        }

        var reservationStart = BuildReservationStart(
            reservation.ReservationDate,
            reservation.ReservationTime);

        return reservationStart > DateTime.Now;
    }

    private static bool CanSeat(TableReservation reservation)
    {
        return reservation.Status == TableReservationStatus.Confirmed;
    }

    private static bool CanComplete(TableReservation reservation)
    {
        return reservation.Status == TableReservationStatus.Seated;
    }

    private static bool CanMarkNoShow(TableReservation reservation)
    {
        if (reservation.Status != TableReservationStatus.Confirmed)
        {
            return false;
        }

        var noShowTime = BuildReservationStart(
            reservation.ReservationDate,
            reservation.ReservationTime).AddMinutes(NoShowGraceMinutes);

        return noShowTime <= DateTime.Now;
    }

    private async Task<HashSet<int>> GetIncompleteCheckoutReservationIdsAsync(IReadOnlyCollection<TableReservation> reservations)
    {
        var reservationIds = reservations.Select(r => r.ReservationId).ToList();
        if (reservationIds.Count == 0)
        {
            return new HashSet<int>();
        }

        var billableOrders = await _context.FoodOrders
            .AsNoTracking()
            .Where(order =>
                order.OrderType == FoodOrderType.TableService &&
                order.TableReservationId.HasValue &&
                reservationIds.Contains(order.TableReservationId.Value) &&
                order.OrderStatus != FoodOrderStatus.Cancelled)
            .Select(order => new
            {
                ReservationId = order.TableReservationId!.Value,
                order.IsChargedToRoom,
                order.IsPaidAtRestaurant
            })
            .ToListAsync();

        var ordersByReservation = billableOrders
            .GroupBy(order => order.ReservationId)
            .ToDictionary(group => group.Key, group => group.ToList());

        return reservations
            .Where(reservation =>
            {
                if (!ordersByReservation.TryGetValue(reservation.ReservationId, out var orders) ||
                    orders.Count == 0)
                {
                    return true;
                }

                return reservation.BookingId.HasValue
                    ? orders.Any(order => !order.IsChargedToRoom)
                    : orders.Any(order => !order.IsPaidAtRestaurant);
            })
            .Select(reservation => reservation.ReservationId)
            .ToHashSet();
    }

    private async Task<bool> IsTableBillSettledAsync(int reservationId)
    {
        var reservation = await _context.TableReservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return false;
        }

        var billableOrders = await _context.FoodOrders
            .AsNoTracking()
            .Where(order =>
                order.OrderType == FoodOrderType.TableService &&
                order.TableReservationId == reservationId &&
                order.OrderStatus != FoodOrderStatus.Cancelled)
            .Select(order => new
            {
                order.IsChargedToRoom,
                order.IsPaidAtRestaurant
            })
            .ToListAsync();

        if (billableOrders.Count == 0)
        {
            return false;
        }

        return reservation.BookingId.HasValue
            ? billableOrders.All(order => order.IsChargedToRoom)
            : billableOrders.All(order => order.IsPaidAtRestaurant);
    }

    private static TableReservationStaffListViewModel MapStaffReservation(
        TableReservation reservation,
        bool isTableBillSettled)
    {
        return new TableReservationStaffListViewModel
        {
            ReservationId = reservation.ReservationId,
            ReservationCode = reservation.ReservationCode,
            BookingId = reservation.BookingId,
            CustomerName = reservation.User?.FullName ?? string.Empty,
            CustomerPhone = reservation.User?.PhoneNumber ?? string.Empty,
            TableNumber = reservation.Table?.TableNumber ?? string.Empty,
            TableName = reservation.Table?.TableName ?? string.Empty,
            Area = reservation.Table?.Area,
            TableType = reservation.Table?.TableType,
            Capacity = reservation.Table?.Capacity ?? 0,
            ReservationDate = reservation.ReservationDate,
            ReservationTime = reservation.ReservationTime,
            DurationMinutes = reservation.DurationMinutes,
            PartySize = reservation.PartySize,
            SpecialRequests = reservation.SpecialRequests,
            Status = reservation.Status,
            CreatedAt = reservation.CreatedAt,
            IsChargedToRoom = reservation.IsChargedToRoom,
            ChargedAmount = reservation.ChargedAmount,
            ChargedAt = reservation.ChargedAt,
            CanChargeToRoom = reservation.BookingId.HasValue &&
                              reservation.Booking != null &&
                              IsChargeableBookingStatus(reservation.Booking.BookingStatus) &&
                              reservation.Status == TableReservationStatus.Completed &&
                              !reservation.IsChargedToRoom,
            CanConfirm = CanConfirm(reservation),
            CanSeat = CanSeat(reservation),
            CanComplete = CanComplete(reservation) && isTableBillSettled,
            CanMarkNoShow = CanMarkNoShow(reservation),
            NoShowAvailableAt = reservation.Status == TableReservationStatus.Confirmed
                ? BuildReservationStart(reservation.ReservationDate, reservation.ReservationTime).AddMinutes(NoShowGraceMinutes)
                : null,
            CanCancel = CanCancel(reservation)
        };
    }

    private async Task<Booking?> ResolveChargeableBookingForReservationAsync(
        int currentUserId,
        DateTime reservationDate)
    {
        return await _context.Bookings
            .Where(b => b.UserId == currentUserId &&
                        (b.BookingStatus == BookingStatus.CheckedIn ||
                         (b.BookingStatus == BookingStatus.Arrived &&
                          b.CheckInDate.Date <= reservationDate.Date &&
                          b.CheckOutDate.Date >= reservationDate.Date)))
            .OrderByDescending(b => b.BookingStatus == BookingStatus.CheckedIn)
            .ThenByDescending(b => b.CheckInDate)
            .FirstOrDefaultAsync();
    }

    private static bool IsChargeableBookingStatus(BookingStatus status)
    {
        return status == BookingStatus.Arrived ||
               status == BookingStatus.CheckedIn;
    }

    private async Task SendReservationEmailAsync(
        int reservationId,
        string subject,
        string message,
        string? extraMessage = null)
    {
        try
        {
            var reservation = await _context.TableReservations
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.Table)
                .Include(r => r.Booking)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation?.User == null ||
                string.IsNullOrWhiteSpace(reservation.User.Email))
            {
                return;
            }

            var htmlBody = BuildReservationEmailBody(reservation, message, extraMessage);
            await _emailSender.SendEmailAsync(
                reservation.User.Email,
                $"{subject} - {reservation.ReservationCode}",
                htmlBody);
        }
        catch
        {
            // Email delivery must not block staff operations.
        }
    }

    private static string BuildReservationEmailBody(
        TableReservation reservation,
        string message,
        string? extraMessage)
    {
        static string H(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

        var tableNumber = reservation.Table?.TableNumber ?? string.Empty;
        var bookingInfo = reservation.BookingId.HasValue
            ? $"Booking #{reservation.BookingId.Value} ({reservation.Booking?.BookingStatus.ToString() ?? "Linked"})"
            : "Not linked to a room booking";
        var chargedInfo = reservation.IsChargedToRoom
            ? $"{reservation.ChargedAmount:N0} VND at {reservation.ChargedAt?.ToString("dd/MM/yyyy HH:mm")}"
            : "Not charged";

        return $@"
<!DOCTYPE html>
<html>
<body style=""font-family:Arial,sans-serif;background:#f6f7fb;margin:0;padding:24px;color:#1f2937;"">
    <div style=""max-width:640px;margin:0 auto;background:#ffffff;border:1px solid #e5e7eb;border-radius:8px;overflow:hidden;"">
        <div style=""background:#dfa974;color:#ffffff;padding:18px 24px;"">
            <h2 style=""margin:0;font-size:22px;"">PhucNguyen Resort</h2>
            <p style=""margin:6px 0 0;"">Restaurant Table Reservation Update</p>
        </div>
        <div style=""padding:24px;"">
            <p>Dear {H(reservation.User?.FullName)},</p>
            <p>{H(message)}</p>
            {(string.IsNullOrWhiteSpace(extraMessage) ? string.Empty : $"<p><strong>Note:</strong> {H(extraMessage)}</p>")}
            <p style=""background:#fff7e6;border:1px solid #f7d58a;border-radius:8px;padding:12px 14px;margin:16px 0;color:#7a4b00;""><strong>Table hold policy:</strong> {H(TableHoldPolicyMessage)}</p>
            <table style=""width:100%;border-collapse:collapse;margin-top:18px;"">
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Reservation Code</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(reservation.ReservationCode)}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Status</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(reservation.Status.ToString())}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Table</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">Table {H(tableNumber)}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Date & Time</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{reservation.ReservationDate:dd/MM/yyyy} {reservation.ReservationTime:hh\:mm}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Party Size</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{reservation.PartySize}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Room Booking</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(bookingInfo)}</td></tr>
                <tr><td style=""padding:8px;border-bottom:1px solid #e5e7eb;""><strong>Room Bill</strong></td><td style=""padding:8px;border-bottom:1px solid #e5e7eb;"">{H(chargedInfo)}</td></tr>
            </table>
            {(string.IsNullOrWhiteSpace(reservation.SpecialRequests) ? string.Empty : $"<p style=\"margin-top:18px;\"><strong>Special Requests:</strong> {H(reservation.SpecialRequests)}</p>")}
            <p style=""margin-top:24px;color:#64748b;"">Thank you for choosing PhucNguyen Resort.</p>
        </div>
    </div>
</body>
</html>";
    }

    private async Task<string> GenerateReservationCodeAsync(DateTime reservationDate)
    {
        string code;

        do
        {
            code = $"TR-{reservationDate:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
        }
        while (await _context.TableReservations.AnyAsync(r => r.ReservationCode == code));

        return code;
    }

    private static string? NormalizeText(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
