using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public class TourBookingService : ITourBookingService
    {
        private readonly ResortDbContext _context;

        public TourBookingService(ResortDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourBooking>> GetAllBookingsAsync()
        {
            return await _context.TourBookings
                .Include(tb => tb.User)
                .Include(tb => tb.Schedule)
                    .ThenInclude(s => s.Tour)
                .OrderByDescending(tb => tb.TourBookingId)
                .ToListAsync();
        }

        public async Task<TourBooking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.TourBookings
                .Include(tb => tb.User)
                .Include(tb => tb.Schedule)
                    .ThenInclude(s => s.Tour)
                .FirstOrDefaultAsync(tb => tb.TourBookingId == bookingId);
        }

        public async Task<TourBooking> AddBookingAsync(TourBooking booking)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Verify capacity (Pessimistic lock simulated or basic check)
                var schedule = await _context.TourSchedules
                    .FirstOrDefaultAsync(s => s.ScheduleId == booking.ScheduleId);

                if (schedule == null) throw new Exception("Schedule not found");

                if (schedule.AvailableSlots < booking.Participants)
                {
                    throw new Exception("TOUR-002: Not enough available slots.");
                }

                schedule.AvailableSlots -= booking.Participants;
                _context.TourSchedules.Update(schedule);

                _context.TourBookings.Add(booking);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateBookingAsync(TourBooking booking)
        {
            _context.TourBookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.TourBookings.FindAsync(bookingId);
            if (booking != null)
            {
                // Restore slots if confirmed
                if (booking.BookingStatus == PhucNguyenResort.Enums.TourBookingStatus.Confirmed)
                {
                    var schedule = await _context.TourSchedules.FindAsync(booking.ScheduleId);
                    if (schedule != null)
                    {
                        schedule.AvailableSlots += booking.Participants;
                        _context.TourSchedules.Update(schedule);
                    }
                }

                _context.TourBookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public bool BookingExists(int bookingId)
        {
            return _context.TourBookings.Any(e => e.TourBookingId == bookingId);
        }
    }
}
