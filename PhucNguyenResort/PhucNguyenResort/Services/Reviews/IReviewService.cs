using PhucNguyenResort.ViewModels.Reviews;

namespace PhucNguyenResort.Services.Reviews;

public interface IReviewService
{
    Task<bool> CreateReviewAsync(int userId, SubmitReviewVM model);
    Task<IEnumerable<ReviewListVM>> GetPublicReviewsAsync(int? tourId = null);
    Task<IEnumerable<ReviewListVM>> GetAllReviewsAsync();
    Task<bool> ToggleVisibilityAsync(int reviewId);
    Task<ReviewListVM?> GetReviewByIdAsync(int reviewId);
    Task<bool> CanSubmitReviewAsync(int userId, int? bookingId, int? tourBookingId);
}
