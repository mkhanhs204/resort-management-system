using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using Xunit;

namespace PhucNguyenResort.Tests.Tours
{
    public class ListTourTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTours()
        {
            // Arrange
            var mockTourService = new Mock<ITourService>();
            var controller = new ToursController(mockTourService.Object, null!);
            var tours = new List<Tour>
            {
                new Tour { TourId = 1, TourName = "Tour 1" },
                new Tour { TourId = 2, TourName = "Tour 2" }
            };
            mockTourService.Setup(s => s.GetAllToursAsync()).ReturnsAsync(tours);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Tour>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}
