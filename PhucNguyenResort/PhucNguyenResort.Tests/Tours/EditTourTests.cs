using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.ViewModels.Tours;
using Xunit;

namespace PhucNguyenResort.Tests.Tours
{
    public class EditTourTests
    {
        private readonly Mock<ITourService> _mockTourService;
        private readonly ToursController _controller;

        public EditTourTests()
        {
            _mockTourService = new Mock<ITourService>();
            _controller = new ToursController(_mockTourService.Object);
            _controller.TempData = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>().Object;
        }

        [Fact]
        public async Task Edit_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var tour = new Tour { TourId = 1, TourName = "Old Tour Name", Price = 500000 };
            _mockTourService.Setup(s => s.GetTourByIdAsync(1)).ReturnsAsync(tour);

            var editModel = new TourEditVM
            {
                TourId = 1,
                TourName = "Updated City Tour",
                Price = 600000
            };

            // Act
            var result = await _controller.Edit(1, editModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            _mockTourService.Verify(s => s.UpdateTourAsync(It.Is<Tour>(t => 
                t.TourId == 1 &&
                t.TourName == "Updated City Tour" &&
                t.Price == 600000)), Times.Once);
        }

        [Fact]
        public async Task Edit_IdMismatch_ReturnsNotFoundResult()
        {
            // Arrange
            var editModel = new TourEditVM { TourId = 1 };

            // Act
            var result = await _controller.Edit(2, editModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockTourService.Verify(s => s.UpdateTourAsync(It.IsAny<Tour>()), Times.Never);
        }
        
        [Fact]
        public async Task Edit_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var editModel = new TourEditVM { TourId = 1, Price = -100 };
            _controller.ModelState.AddModelError("Price", "Invalid price");
            
            // Act
            var result = await _controller.Edit(1, editModel);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(editModel, viewResult.Model);
        }
    }
}
