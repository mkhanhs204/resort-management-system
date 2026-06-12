using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using Xunit;

namespace ManagerTourTesting.Tours
{
    public class DetailsTourTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly ToursController _controller;

        public DetailsTourTests()
        {
            _mockTourService = new Mock<ITourService>();
            _controller = new ToursController(_mockTourService.Object);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsViewResultWithTour()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "City Tour" };
            _mockTourService.Setup(s => s.GetTourByIdAsync(1)).ReturnsAsync(tour);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Tour>(viewResult.ViewData.Model);
            Assert.Equal(1, model.TourId);
            Assert.Equal("City Tour", model.TourName);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            _mockTourService.Setup(s => s.GetTourByIdAsync(999)).ReturnsAsync((Tour)null);

            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_NullId_ReturnsNotFoundResult()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
