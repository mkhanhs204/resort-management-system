using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using Xunit;

namespace PhucNguyenResort.Tests.Tours
{
    public class DeleteTourTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly ToursController _controller;

        public DeleteTourTests()
        {
            _mockTourService = new Mock<ITourService>();
            _controller = new ToursController(_mockTourService.Object);
            _controller.TempData = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>().Object;
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Tour to be deleted" };
            _mockTourService.Setup(s => s.GetTourByIdAsync(1)).ReturnsAsync(tour);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            
            _mockTourService.Verify(s => s.DeleteTourAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteConfirmed_InvalidId_ReturnsNotFoundResult_WithoutDeleting()
        {
            // Arrange
            _mockTourService.Setup(s => s.GetTourByIdAsync(999)).ReturnsAsync((Tour)null);

            // Act
            var result = await _controller.DeleteConfirmed(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockTourService.Verify(s => s.DeleteTourAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTourAsync_WithDependencies_ThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ResortDbContext(options);
            var tour = new Tour 
            { 
                TourId = 2, 
                TourName = "Beach Tour", 
                Price = 100000, 
                MaxCapacity = 20, 
                DurationHours = 4 
            };
            var schedule = new TourSchedule 
            { 
                ScheduleId = 1, 
                TourId = 2, 
                TourDate = DateTime.Now.Date,
                StartTime = new TimeSpan(8, 0, 0),
                AvailableSlots = 20
            };

            context.Tours.Add(tour);
            context.TourSchedules.Add(schedule);
            await context.SaveChangesAsync();

            var service = new TourService(context);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteTourAsync(2));
            Assert.Contains("Tour has active dependencies", ex.Message);
            
            // Verify DB vẫn giữ nguyên record
            var tourInDb = await context.Tours.FindAsync(2);
            Assert.NotNull(tourInDb);
        }
    }
}
