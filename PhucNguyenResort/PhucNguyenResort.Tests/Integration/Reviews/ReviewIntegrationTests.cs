using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Reviews;
using PhucNguyenResort.ViewModels.Reviews;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Integration.Reviews;

public class ReviewIntegrationTests : IDisposable
{
    private readonly ResortDbContext _context;
    private readonly ReviewService _reviewService;

    public ReviewIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ResortDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ResortDbContext(options);
        _reviewService = new ReviewService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task REVIEW_TC_INT_001_FullFlow_Submit_View_Moderate()
    {
        // Preconditions
        var user = new User { UserId = 1, FullName = "Customer 1", RoleId = 2 };
        var booking = new Booking { BookingId = 100, UserId = 1, BookingStatus = BookingStatus.CheckedOut, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now };
        await _context.Users.AddAsync(user);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        // Step 1: Customer submits review
        var model = new SubmitReviewVM { Rating = 5, Comment = "Tuyệt!", BookingId = 100 };
        var submitResult = await _reviewService.CreateReviewAsync(1, model);
        Assert.True(submitResult);

        // Step 2: GetPublicReviews -> verify review is present
        var publicReviews = await _reviewService.GetPublicReviewsAsync();
        Assert.Single(publicReviews);
        var createdReview = publicReviews.First();
        Assert.Equal(5, createdReview.Rating);

        // Step 3: Manager toggles visibility (hide)
        await _reviewService.ToggleVisibilityAsync(createdReview.ReviewId);

        // Step 4: GetPublicReviews -> verify review is hidden
        var publicReviewsHidden = await _reviewService.GetPublicReviewsAsync();
        Assert.Empty(publicReviewsHidden);

        // Step 5: Manager toggles visibility (show)
        await _reviewService.ToggleVisibilityAsync(createdReview.ReviewId);

        // Step 6: GetPublicReviews -> verify review is shown again
        var publicReviewsShown = await _reviewService.GetPublicReviewsAsync();
        Assert.Single(publicReviewsShown);
    }

    [Fact]
    public async Task REVIEW_TC_INT_002_DuplicatePreventionAcrossRequests()
    {
        // Preconditions
        var user = new User { UserId = 1, FullName = "Customer 1", RoleId = 2 };
        var booking = new Booking { BookingId = 100, UserId = 1, BookingStatus = BookingStatus.CheckedOut, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now };
        await _context.Users.AddAsync(user);
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();

        // Step 1: Submit first review
        var model1 = new SubmitReviewVM { Rating = 5, Comment = "Nice", BookingId = 100 };
        var result1 = await _reviewService.CreateReviewAsync(1, model1);
        Assert.True(result1);

        // Step 2: Submit second review for same booking
        var model2 = new SubmitReviewVM { Rating = 4, Comment = "Duplicate", BookingId = 100 };
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.CreateReviewAsync(1, model2));
        Assert.Contains("REV-004", ex.Message);

        // Verify only 1 review in DB
        var count = await _context.Reviews.CountAsync(r => r.BookingId == 100 && r.UserId == 1);
        Assert.Equal(1, count);
    }
}
