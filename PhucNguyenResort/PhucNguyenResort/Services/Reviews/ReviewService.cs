using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Reviews;

namespace PhucNguyenResort.Services.Reviews;

public class ReviewService : IReviewService
{
    private readonly ResortDbContext _context;

    public ReviewService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanSubmitReviewAsync(int userId, int? bookingId, int? tourBookingId)
    {
        if (bookingId == null && tourBookingId == null)
            return false;

        if (bookingId.HasValue)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId.Value && b.UserId == userId);
            
            if (booking == null || booking.BookingStatus != BookingStatus.CheckedOut)
                return false;
        }

        if (tourBookingId.HasValue)
        {
            var hasCompletedTour = await _context.TourBookings
                .AnyAsync(tb => tb.UserId == userId 
                             && tb.TourBookingId == tourBookingId.Value 
                             && tb.BookingStatus == TourBookingStatus.Completed);
            
            if (!hasCompletedTour)
                return false;
        }

        return true;
    }

    public async Task<bool> CreateReviewAsync(int userId, SubmitReviewVM model)
    {
        if (model.Rating == null || model.Rating < 1 || model.Rating > 5)
            throw new InvalidOperationException("REV-001: Rating must be between 1 and 5");

        if (model.BookingId == null && model.TourBookingId == null)
            throw new InvalidOperationException("REV-002: Must provide BookingId or TourBookingId");

        if (model.BookingId != null && model.TourBookingId != null)
            throw new InvalidOperationException("REV-002: Cannot provide both BookingId and TourBookingId simultaneously");

        var canSubmit = await CanSubmitReviewAsync(userId, model.BookingId, model.TourBookingId);
        if (!canSubmit)
            throw new InvalidOperationException("REV-003: Service not completed yet");

        // Check duplicate
        var isDuplicate = await _context.Reviews.AnyAsync(r => 
            r.UserId == userId && 
            ((model.BookingId.HasValue && r.BookingId == model.BookingId) || 
             (model.TourBookingId.HasValue && r.TourBookingId == model.TourBookingId)));

        if (isDuplicate)
            throw new InvalidOperationException("REV-004: Review already submitted for this booking");

        int? tourId = null;
        if (model.TourBookingId.HasValue)
        {
            var tourBooking = await _context.TourBookings
                .Include(tb => tb.Schedule)
                .FirstOrDefaultAsync(tb => tb.TourBookingId == model.TourBookingId.Value);
            tourId = tourBooking?.Schedule?.TourId;
        }

        var review = new Review
        {
            UserId = userId,
            BookingId = model.BookingId,
            TourBookingId = model.TourBookingId,
            TourId = tourId,
            Rating = model.Rating,
            Comment = model.Comment,
            IsVisible = true,
            CreatedAt = DateTime.Now
        };

        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ReviewListVM>> GetAllReviewsAsync()
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Tour)
            .Include(r => r.Booking)
                .ThenInclude(b => b!.BookingDetails)
                    .ThenInclude(bd => bd.RoomCategory)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(r => new ReviewListVM
        {
            ReviewId = r.ReviewId,
            UserName = r.User != null ? r.User.FullName : "Khách",
            Rating = r.Rating,
            Comment = r.Comment,
            IsVisible = r.IsVisible,
            CreatedAt = r.CreatedAt,
            ServiceType = r.BookingId != null ? "Room" : "Tour",
            ServiceName = r.BookingId != null 
                ? (r.Booking?.BookingDetails?.FirstOrDefault()?.RoomCategory?.CategoryName ?? "Phòng Resort") 
                : r.Tour != null ? r.Tour.TourName : ""
        });
    }

    public async Task<IEnumerable<ReviewListVM>> GetPublicReviewsAsync(int? tourId = null)
    {
        var query = _context.Reviews
            .Include(r => r.User)
            .Include(r => r.Tour)
            .Include(r => r.Booking)
                .ThenInclude(b => b!.BookingDetails)
                    .ThenInclude(bd => bd.RoomCategory)
            .Where(r => r.IsVisible);

        if (tourId.HasValue)
        {
            query = query.Where(r => r.TourId == tourId.Value);
        }

        var reviews = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();

        return reviews.Select(r => new ReviewListVM
        {
            ReviewId = r.ReviewId,
            UserName = r.User != null ? r.User.FullName : "Khách",
            Rating = r.Rating,
            Comment = r.Comment,
            IsVisible = r.IsVisible,
            CreatedAt = r.CreatedAt,
            ServiceType = r.BookingId != null ? "Room" : "Tour",
            ServiceName = r.BookingId != null 
                ? (r.Booking?.BookingDetails?.FirstOrDefault()?.RoomCategory?.CategoryName ?? "Phòng Resort") 
                : r.Tour != null ? r.Tour.TourName : ""
        });
    }

    public async Task<ReviewListVM?> GetReviewByIdAsync(int reviewId)
    {
        var r = await _context.Reviews
            .Include(rev => rev.User)
            .Include(rev => rev.Tour)
            .Include(rev => rev.Booking)
                .ThenInclude(b => b!.BookingDetails)
                    .ThenInclude(bd => bd.RoomCategory)
            .FirstOrDefaultAsync(rev => rev.ReviewId == reviewId);

        if (r == null) return null;

        return new ReviewListVM
        {
            ReviewId = r.ReviewId,
            UserName = r.User != null ? r.User.FullName : "Khách",
            Rating = r.Rating,
            Comment = r.Comment,
            IsVisible = r.IsVisible,
            CreatedAt = r.CreatedAt,
            ServiceType = r.BookingId != null ? "Room" : "Tour",
            ServiceName = r.BookingId != null 
                ? (r.Booking?.BookingDetails?.FirstOrDefault()?.RoomCategory?.CategoryName ?? "Phòng Resort") 
                : r.Tour != null ? r.Tour.TourName : ""
        };
    }

    public async Task<bool> ToggleVisibilityAsync(int reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);
        if (review == null)
            throw new KeyNotFoundException("REV-005: Review not found");

        review.IsVisible = !review.IsVisible;
        await _context.SaveChangesAsync();
        
        return true;
    }
}
