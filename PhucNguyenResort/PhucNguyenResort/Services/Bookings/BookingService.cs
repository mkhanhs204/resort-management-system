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

            var overlap = await _context.Bookings
                .AnyAsync(b => b.RoomId == request.RoomId 
                            && b.BookingStatus != BookingStatus.Cancelled
                            && b.BookingStatus != BookingStatus.CheckedOut
                            && b.CheckInDate < request.CheckOutDate 
                            && b.CheckOutDate > request.CheckInDate);

            if (overlap)
            {
                return new BookingResult { Success = false, ErrorCode = "BKG-002", ErrorMessage = "Room not available" };
            }

            var room = await _context.Rooms
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.RoomId == request.RoomId);

            if (room == null || room.Category == null)
            {
                return new BookingResult { Success = false, ErrorMessage = "Room not found" };
            }

            int nights = (request.CheckOutDate - request.CheckInDate).Days;
            decimal pricePerNight = room.Category.PricePerNight ?? 0;
            decimal totalPrice = nights * pricePerNight;
            decimal depositAmount = totalPrice * 0.5m;

            var booking = new Booking
            {
                RoomId = request.RoomId,
                UserId = userId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfAdults = request.NumberOfAdults,
                NumberOfChildren = request.NumberOfChildren,
                TotalAmount = totalPrice,
                DepositAmount = depositAmount,
                BookingStatus = BookingStatus.PendingPayment
            };

            _context.Bookings.Add(booking);

            // Xử lý tạo TourBookings nếu khách có chọn Add-on
            if (request.SelectedTourScheduleIds != null && request.SelectedTourScheduleIds.Any())
            {
                // Lấy thông tin các Schedule để tính tiền
                var schedules = await _context.TourSchedules
                    .Include(s => s.Tour)
                    .Where(s => request.SelectedTourScheduleIds.Contains(s.ScheduleId))
                    .ToListAsync();

                foreach (var schedule in schedules)
                {
                    if (schedule.Tour != null)
                    {
                        decimal tourTotal = (schedule.Tour.Price ?? 0) * (request.NumberOfAdults + request.NumberOfChildren);
                        var tourBooking = new TourBooking
                        {
                            Booking = booking, // EF Core sẽ tự link BookingId khi booking được save
                            UserId = userId,
                            ScheduleId = schedule.ScheduleId,
                            Participants = request.NumberOfAdults + request.NumberOfChildren,
                            TotalAmount = tourTotal,
                            BookingStatus = TourBookingStatus.Pending
                        };
                        _context.TourBookings.Add(tourBooking);
                        
                        // Cập nhật lại tổng tiền phòng và tiền cọc bao gồm cả Tour
                        booking.TotalAmount += tourTotal;
                        booking.DepositAmount += tourTotal * 0.5m;
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

        public async Task<CancelBookingResult> CancelBookingAsync(int bookingId, int userId)
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
            await _context.SaveChangesAsync();

            return new CancelBookingResult { Success = true, RefundAmount = refundAmount };
        }
    }
}
