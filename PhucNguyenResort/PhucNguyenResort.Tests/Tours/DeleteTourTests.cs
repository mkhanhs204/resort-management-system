using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using Xunit;

namespace PhucNguyenResort.Tests.Tours
{
    public class DeleteTourTests
    {
        private readonly Mock<ITourCatalogService> _mockTourService;
        private readonly ToursController _controller;

        public DeleteTourTests()
        {
            _mockTourService = new Mock<ITourCatalogService>();
            _controller = new ToursController(_mockTourService.Object, null!);
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
        public async Task DeleteConfirmed_InvalidId_ReturnsRedirectToActionResult_WithoutDeleting()
        {
            // Arrange
            _mockTourService.Setup(s => s.GetTourByIdAsync(999)).ReturnsAsync((Tour)null);

            // Act
            var result = await _controller.DeleteConfirmed(999);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            
            _mockTourService.Verify(s => s.DeleteTourAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
