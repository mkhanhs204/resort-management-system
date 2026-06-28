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

namespace PhucNguyenResort.Tests.Integration
{
    public class ReviewIntegrationTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task REVIEW_TC_INT_001_FullFlow_Submit_ViewPublic_Moderate()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, RoleId = 2 }); // Customer
                context.Users.Add(new User { UserId = 10, RoleId = 3 }); // BusinessManager
                context.Bookings.Add(new Booking { BookingId = 102, UserId = 1, BookingStatus = BookingStatus.CheckedOut });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);

                // Step 1: Customer submit review
                var model = new SubmitReviewVM { Rating = 5, Comment = "Tuyệt!", BookingId = 102 };
                await service.CreateReviewAsync(1, model);

                // Step 2: GetPublicReviews -> verify review exists
                var publicReviews = await service.GetPublicReviewsAsync();
                Assert.Single(publicReviews);
                var reviewId = publicReviews.First().ReviewId;

                // Step 3: Manager toggle visibility (Hide)
                await service.ToggleVisibilityAsync(reviewId);

                // Step 4: GetPublicReviews -> verify review hidden
                var publicReviewsAfterHide = await service.GetPublicReviewsAsync();
                Assert.Empty(publicReviewsAfterHide);

                // Step 5: Manager toggle visibility again (Show)
                await service.ToggleVisibilityAsync(reviewId);

                // Step 6: GetPublicReviews -> verify review visible again
                var publicReviewsAfterShow = await service.GetPublicReviewsAsync();
                Assert.Single(publicReviewsAfterShow);
            }
        }

        [Fact]
        public async Task REVIEW_TC_INT_002_DuplicatePrevention_AcrossRequests()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, RoleId = 2 });
                context.Bookings.Add(new Booking { BookingId = 102, UserId = 1, BookingStatus = BookingStatus.CheckedOut });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new ReviewService(context);
                var model = new SubmitReviewVM { Rating = 4, Comment = "Lần 1", BookingId = 102 };

                // Step 1: Submit first time -> success
                await service.CreateReviewAsync(1, model);
                var count1 = await context.Reviews.CountAsync(r => r.BookingId == 102 && r.UserId == 1);
                Assert.Equal(1, count1);

                // Step 2: Submit second time -> exception
                var model2 = new SubmitReviewVM { Rating = 5, Comment = "Lần 2", BookingId = 102 };
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReviewAsync(1, model2));
                Assert.Contains("REV-004", ex.Message);

                var count2 = await context.Reviews.CountAsync(r => r.BookingId == 102 && r.UserId == 1);
                Assert.Equal(1, count2); // Still only 1 record
            }
        }
    }
}
