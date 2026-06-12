using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.ViewModels.Tours;
using Xunit;

namespace ManagerTourTesting.Tours
{
    public class CreateTourTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly ToursController _controller;

        public CreateTourTests()
        {
            _mockTourService = new Mock<ITourService>();
            _controller = new ToursController(_mockTourService.Object);
        }

        [Fact]
        public async Task Create_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockModel = new TourCreateVM
            {
                TourName = "New Tour",
                Price = 100000,
                MaxCapacity = 30,
                DurationHours = 8
            };

            // Act
            var result = await _controller.Create(mockModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            
            _mockTourService.Verify(s => s.AddTourAsync(It.Is<Tour>(t => 
                t.TourName == mockModel.TourName && 
                t.Price == mockModel.Price &&
                t.MaxCapacity == mockModel.MaxCapacity &&
                t.DurationHours == mockModel.DurationHours)), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var mockModel = new TourCreateVM
            {
                Price = -1 // Invalid
            };
            _controller.ModelState.AddModelError("TourName", "Tour Name is required");

            // Act
            var result = await _controller.Create(mockModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(mockModel, viewResult.Model);
            Assert.False(_controller.ModelState.IsValid);
            
            _mockTourService.Verify(s => s.AddTourAsync(It.IsAny<Tour>()), Times.Never);
        }
    }
}
