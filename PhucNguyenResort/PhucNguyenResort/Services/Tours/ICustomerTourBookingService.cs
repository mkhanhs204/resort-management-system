using PhucNguyenResort.ViewModels.Tours;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Tours
{
    /// <summary>
    /// Service interface for customer-facing tour booking operations.
    /// Handles browsing, booking with pessimistic locking, payment integration, and cancellation.
    /// </summary>
    public interface ICustomerTourBookingService
    {
        /// <summary>
        /// Get all active tours for customer browsing with optional search/sort.
        /// </summary>
        Task<TourListVM> GetTourListAsync(string? searchKeyword, string? sortBy);

        /// <summary>
        /// Get tour detail with upcoming schedules.
        /// </summary>
        Task<TourDetailVM?> GetTourDetailAsync(int tourId);

        /// <summary>
        /// Prepare the booking form ViewModel for a specific schedule.
        /// </summary>
        Task<BookTourVM?> PrepareBookingFormAsync(int scheduleId, int userId);

        /// <summary>
        /// Book a tour with Pessimistic Locking to prevent overbooking.
        /// </summary>
        Task<TourBookingResultVM> BookTourAsync(BookTourVM model, int userId);

        /// <summary>
        /// Get all tour bookings for a specific user.
        /// </summary>
        Task<List<MyTourBookingItemVM>> GetUserTourBookingsAsync(int userId);

        /// <summary>
        /// Cancel a tour booking and restore available slots.
        /// </summary>
        Task<TourBookingResultVM> CancelTourBookingAsync(int tourBookingId, int userId);

        /// <summary>
        /// Confirm a pending tour booking after VNPay payment success.
        /// </summary>
        Task<bool> ConfirmPaymentAsync(int tourBookingId, string transactionCode);

        /// <summary>
        /// Get booking success info for display.
        /// </summary>
        Task<TourBookingSuccessVM?> GetBookingSuccessInfoAsync(int tourBookingId, int userId);
    }
}
