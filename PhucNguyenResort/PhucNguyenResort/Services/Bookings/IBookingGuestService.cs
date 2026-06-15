using PhucNguyenResort.ViewModels.Bookings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Bookings;

public interface IBookingGuestService
{
    Task<bool> SaveBookingGuestsAsync(int bookingId, IEnumerable<BookingGuestDto> guests, bool consentChecked);
    Task<IEnumerable<BookingGuestDto>> GetBookingGuestsForCheckInAsync(int bookingId);
    Task<int> PurgeExpiredPiiDataAsync();
}
