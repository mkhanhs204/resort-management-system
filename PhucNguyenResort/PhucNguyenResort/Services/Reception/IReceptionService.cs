using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Bookings;

namespace PhucNguyenResort.Services.Reception
{
    public interface IReceptionService
    {
        Task<List<Booking>> GetBookingsAsync(string? searchString, BookingStatus? statusFilter, DateTime? dateFilter = null);
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<PreCheckinViewModel?> PrepareCheckInViewModelAsync(int id);
        Task<(bool Success, string ErrorMessage)> ProcessCheckInAsync(PreCheckinViewModel model);
        Task<(byte[]? FileContent, string FileName, string ErrorMessage)> GetManifestDataAsync(int id);
        Task<List<Room>> GetAvailableRoomsForCategoryAsync(int categoryId, DateTime checkIn, DateTime checkOut, int? currentBookingId = null);
    }
}
