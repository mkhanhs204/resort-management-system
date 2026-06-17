using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    public interface ITourBookingService
    {
        Task<IEnumerable<TourBooking>> GetAllBookingsAsync();
        Task<TourBooking?> GetBookingByIdAsync(int bookingId);
        Task<TourBooking> AddBookingAsync(TourBooking booking);
        Task UpdateBookingAsync(TourBooking booking);
        Task DeleteBookingAsync(int bookingId);
        bool BookingExists(int bookingId);
    }
}
