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
                TotalAmount = totalPrice,
                DepositAmount = depositAmount,
                BookingStatus = BookingStatus.Reserved
            };

            _context.Bookings.Add(booking);
            
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
