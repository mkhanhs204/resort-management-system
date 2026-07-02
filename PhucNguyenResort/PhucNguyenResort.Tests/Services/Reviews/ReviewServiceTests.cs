using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Reviews;
using PhucNguyenResort.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Services.Reviews
{
    public class ReviewServiceTests : IDisposable
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        public void Dispose()
        {
            // Optional: clean up if using shared context, but we use new context for each test.
        }

        [Fact]
        public async Task REVIEW_TC_001_SubmitReview_CheckedOutBooking_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, RoleId = 2 }); // Customer
                context.Bookings.Add(new Booking { BookingId = 100, UserId = 1, BookingStatus = BookingStatus.CheckedOut });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 5, Comment = "Dịch vụ tuyệt vời!", BookingId = 100 };

                // Act
                var result = await service.CreateReviewAsync(1, model);

                // Assert
                Assert.True(result);
                var review = await context.Reviews.FirstOrDefaultAsync(r => r.BookingId == 100);
                Assert.NotNull(review);
                Assert.Equal(5, review.Rating);
                Assert.Equal("Dịch vụ tuyệt vời!", review.Comment);
                Assert.True(review.IsVisible);
            }
        }

        [Fact]
        public async Task REVIEW_TC_002_SubmitReview_CompletedTour_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, RoleId = 2 });
                context.Tours.Add(new Tour { TourId = 50, TourName = "Ha Long" });
                var schedule = new TourSchedule { ScheduleId = 1, TourId = 50 };
                context.TourSchedules.Add(schedule);
                context.TourBookings.Add(new TourBooking { BookingId = 200, UserId = 1, ScheduleId = 1, BookingStatus = TourBookingStatus.Completed, Schedule = schedule });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var tourBooking = await context.TourBookings.FirstAsync();
                var model = new SubmitReviewVM { Rating = 4, Comment = "Rất vui", TourBookingId = tourBooking.TourBookingId };

                // Act
                var result = await service.CreateReviewAsync(1, model);

                // Assert
                Assert.True(result);
                var review = await context.Reviews.FirstOrDefaultAsync(r => r.TourBookingId == tourBooking.TourBookingId);
                Assert.NotNull(review);
                Assert.Equal(4, review.Rating);
            }
        }

        [Fact]
        public async Task REVIEW_TC_003_SubmitReview_BookingNotCheckedOut_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Bookings.Add(new Booking { BookingId = 101, UserId = 1, BookingStatus = BookingStatus.CheckedIn });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 3, BookingId = 101 };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-003", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_004_SubmitReview_Duplicate_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Bookings.Add(new Booking { BookingId = 100, UserId = 1, BookingStatus = BookingStatus.CheckedOut });
                context.Reviews.Add(new Review { ReviewId = 10, UserId = 1, BookingId = 100, Rating = 5, IsVisible = true });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 4, BookingId = 100 };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-004", ex.Message);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(6)]
        public async Task REVIEW_TC_005_SubmitReview_InvalidRating_ThrowsException(int invalidRating)
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = invalidRating, BookingId = 102 };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-001", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_006_SubmitReview_NoBookingOrTour_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 4, BookingId = null, TourBookingId = null };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-002", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_007_ToggleVisibility_HideVisibleReview_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Reviews.Add(new Review { ReviewId = 10, IsVisible = true });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.ToggleVisibilityAsync(10);

                // Assert
                Assert.True(result);
                var review = await context.Reviews.FindAsync(10);
                Assert.False(review!.IsVisible); // Should be hidden
            }
        }

        [Fact]
        public async Task REVIEW_TC_008_ToggleVisibility_ShowHiddenReview_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Reviews.Add(new Review { ReviewId = 11, IsVisible = false });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                var result = await service.ToggleVisibilityAsync(11);

                // Assert
                Assert.True(result);
                var review = await context.Reviews.FindAsync(11);
                Assert.True(review!.IsVisible); // Should be visible
            }
        }

        [Fact]
        public async Task REVIEW_TC_009_ToggleVisibility_NotFound_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.ToggleVisibilityAsync(9999));
                Assert.Contains("REV-005", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_010_GetPublicReviews_ReturnsOnlyVisible()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Reviews.Add(new Review { ReviewId = 10, IsVisible = true });
                context.Reviews.Add(new Review { ReviewId = 11, IsVisible = false });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                var results = await service.GetPublicReviewsAsync();

                // Assert
                Assert.Single(results);
                Assert.Equal(10, results.First().ReviewId);
            }
        }

        [Fact]
        public async Task REVIEW_TC_011_GetAllReviews_ReturnsAll()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Reviews.Add(new Review { ReviewId = 10, IsVisible = true });
                context.Reviews.Add(new Review { ReviewId = 11, IsVisible = false });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Act
                var results = await service.GetAllReviewsAsync();

                // Assert
                Assert.Equal(2, results.Count());
            }
        }

        [Fact]
        public async Task REVIEW_TC_012_SubmitReview_BothBookingAndTour_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 4, BookingId = 100, TourBookingId = 200 };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-002", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_013_SubmitReview_TourNotCompleted_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Tours.Add(new Tour { TourId = 50 });
                var schedule = new TourSchedule { ScheduleId = 1, TourId = 50 };
                context.TourSchedules.Add(schedule);
                context.TourBookings.Add(new TourBooking { BookingId = 201, UserId = 1, ScheduleId = 1, BookingStatus = TourBookingStatus.Confirmed, Schedule = schedule });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var tourBooking = await context.TourBookings.FirstAsync();
                var model = new SubmitReviewVM { Rating = 3, TourBookingId = tourBooking.TourBookingId };

                // Act & Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model));
                Assert.Contains("REV-003", ex.Message);
            }
        }

        [Fact]
        public async Task REVIEW_TC_SEC_003_XSS_Prevention_IsHandledByViewEngine_ServiceStoresAsIs()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Bookings.Add(new Booking { BookingId = 100, UserId = 1, BookingStatus = BookingStatus.CheckedOut });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var xssPayload = "<script>alert('XSS')</script>";
                var model = new SubmitReviewVM { Rating = 5, Comment = xssPayload, BookingId = 100 };

                // Act
                await service.CreateReviewAsync(1, model);

                // Assert
                var review = await context.Reviews.FirstOrDefaultAsync(r => r.BookingId == 100);
                Assert.Equal(xssPayload, review!.Comment);
            }
        }
    }
}
