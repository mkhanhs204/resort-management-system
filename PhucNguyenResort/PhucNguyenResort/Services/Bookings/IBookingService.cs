using System.Threading.Tasks;
using PhucNguyenResort.ViewModels.Bookings;

namespace PhucNguyenResort.Services.Bookings
{
    public interface IBookingService
    {
        Task<BookingResult> CreateBookingAsync(CreateBookingViewModel request, int userId);
        Task<CancelBookingResult> CancelBookingAsync(int bookingId, int userId);
    }

    public class BookingResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public int? BookingId { get; set; }
    }

    public class CancelBookingResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public decimal RefundAmount { get; set; }
    }
}
