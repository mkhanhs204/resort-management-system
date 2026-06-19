using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Bookings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly ResortDbContext _context;

        public BookingService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<BookingResult> CreateBookingAsync(CreateBookingViewModel request, int userId)
        {
            if (request.CheckOutDate <= request.CheckInDate || request.CheckInDate < DateTime.Today)
            {
                return new BookingResult { Success = false, ErrorCode = "BKG-001", ErrorMessage = "Invalid dates" };
            }

            if (request.RoomSelections == null || !request.RoomSelections.Any())
            {
                return new BookingResult { Success = false, ErrorMessage = "No rooms selected" };
            }

            decimal totalRoomPrice = 0;
            int nights = (request.CheckOutDate - request.CheckInDate).Days;

            var booking = new Booking
            {
                UserId = userId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfAdults = request.NumberOfAdults,
                NumberOfChildren = request.NumberOfChildren,
                BookingStatus = BookingStatus.PendingPayment,
                BookingDetails = new List<BookingDetail>()
            };

            foreach (var selection in request.RoomSelections)
            {
                if (selection.Quantity <= 0) continue;

                var category = await _context.RoomCategories.FindAsync(selection.CategoryId);
                if (category == null)
                {
                    return new BookingResult { Success = false, ErrorMessage = $"Category {selection.CategoryId} not found" };
                }

                int totalRooms = await _context.Rooms.CountAsync(r => r.CategoryId == selection.CategoryId && r.Status != RoomStatus.Maintenance);
                int bookedRooms = await _context.BookingDetails
                    .Where(bd => bd.RoomCategoryId == selection.CategoryId
                              && bd.Booking != null
                              && bd.Booking.BookingStatus != BookingStatus.Cancelled
                              && bd.Booking.BookingStatus != BookingStatus.CheckedOut
                              && bd.Booking.CheckInDate < request.CheckOutDate
                              && bd.Booking.CheckOutDate > request.CheckInDate)
                    .CountAsync();

                if (totalRooms - bookedRooms < selection.Quantity)
                {
                    return new BookingResult { Success = false, ErrorCode = "BKG-002", ErrorMessage = $"Not enough rooms available for category {category.CategoryName}" };
                }

                decimal pricePerNight = category.PricePerNight ?? 0;
                totalRoomPrice += pricePerNight * nights * selection.Quantity;

                for (int i = 0; i < selection.Quantity; i++)
                {
                    booking.BookingDetails.Add(new BookingDetail
                    {
                        RoomCategoryId = selection.CategoryId,
                        Price = pricePerNight
                    });
                }
            }

            if (!booking.BookingDetails.Any())
            {
                return new BookingResult { Success = false, ErrorMessage = "No valid rooms selected" };
            }

            booking.TotalAmount = totalRoomPrice;
            booking.DepositAmount = totalRoomPrice * 0.4m;

            _context.Bookings.Add(booking);

            // Xử lý tạo TourBookings nếu khách có chọn Add-on
            if (request.SelectedTours != null && request.SelectedTours.Any(t => t.Participants > 0))
            {
                var tourParticipants = request.SelectedTours
                    .Where(t => t.Participants > 0)
                    .ToDictionary(t => t.ScheduleId, t => t.Participants);

                var scheduleIds = tourParticipants.Keys.ToList();

                // Lấy thông tin các Schedule để tính tiền
                var schedules = await _context.TourSchedules
                    .Include(s => s.Tour)
                    .Where(s => scheduleIds.Contains(s.ScheduleId))
                    .ToListAsync();

                foreach (var schedule in schedules)
                {
                    if (schedule.Tour != null)
                    {
                        int requestedSlots = tourParticipants[schedule.ScheduleId];

                        if (schedule.AvailableSlots.HasValue && schedule.AvailableSlots.Value < requestedSlots)
                        {
                            return new BookingResult
                            {
                                Success = false,
                                ErrorCode = "BKG-003",
                                ErrorMessage = $"Not enough slots available for tour: {schedule.Tour.TourName}"
                            };
                        }

                        decimal tourTotal = (schedule.Tour.Price ?? 0) * requestedSlots;
                        var tourBooking = new TourBooking
                        {
                            Booking = booking, // EF Core sẽ tự link BookingId khi booking được save
                            UserId = userId,
                            ScheduleId = schedule.ScheduleId,
                            Participants = requestedSlots,
                            TotalAmount = tourTotal,
                            BookingStatus = TourBookingStatus.Pending
                        };
                        _context.TourBookings.Add(tourBooking);
                        
                        if (schedule.AvailableSlots.HasValue)
                        {
                            schedule.AvailableSlots -= requestedSlots;
                        }

                        // Cập nhật lại tổng tiền phòng và tiền cọc bao gồm cả Tour
                        booking.TotalAmount += tourTotal;
                        booking.DepositAmount += tourTotal * 0.4m;
                    }
                }
            }
            
            try
            {
                await _context.SaveChangesAsync();
                return new BookingResult { Success = true, BookingId = booking.BookingId };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BookingResult { Success = false, ErrorCode = "BKG-002", ErrorMessage = "Room not available" };
            }
        }

        public async Task<CancelBookingResult> CancelBookingAsync(int bookingId, int userId, string? reason = null, string? bankDetails = null)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId && b.UserId == userId);
            if (booking == null)
            {
                return new CancelBookingResult { Success = false, ErrorMessage = "Booking not found" };
            }

            if (booking.BookingStatus == BookingStatus.Cancelled)
            {
                return new CancelBookingResult { Success = false, ErrorMessage = "Already cancelled" };
            }

            decimal refundAmount = 0;
            if ((booking.CheckInDate - DateTime.Now).TotalHours > 48)
            {
                refundAmount = booking.DepositAmount ?? 0;
            }

            booking.BookingStatus = BookingStatus.Cancelled;
            
            if (refundAmount > 0)
            {
                var refund = new Refund
                {
                    BookingId = booking.BookingId,
                    RefundAmount = refundAmount,
                    RefundReason = reason,
                    BankDetails = bankDetails,
                    RefundStatus = RefundStatus.Pending,
                    CreatedAt = DateTime.Now
                };
                _context.Refunds.Add(refund);
            }

            // Cancel all associated tour bookings and restore slots
            var tourBookings = await _context.TourBookings
                .Include(tb => tb.Schedule)
                .Where(tb => tb.BookingId == bookingId)
                .ToListAsync();
            foreach (var tb in tourBookings)
            {
                if (tb.BookingStatus != TourBookingStatus.Cancelled)
                {
                    tb.BookingStatus = TourBookingStatus.Cancelled;
                    if (tb.Schedule != null && tb.Schedule.AvailableSlots.HasValue)
                    {
                        tb.Schedule.AvailableSlots += tb.Participants ?? 0;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new CancelBookingResult { Success = true, RefundAmount = refundAmount };
        }
    }
}
