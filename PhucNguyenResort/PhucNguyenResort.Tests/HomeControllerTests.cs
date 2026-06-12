using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Home;
using PhucNguyenResort.Services.Tours;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IHomeService> _mockHomeService;
        private readonly Mock<ITourService> _mockTourService;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockHomeService = new Mock<IHomeService>();
            _mockTourService = new Mock<ITourService>();
            _controller = new HomeController(_mockLogger.Object, _mockHomeService.Object, _mockTourService.Object);
        }

        [Fact]
        public async Task UC04_06_TC_001_SearchRooms_InvalidDates_UseDefaultDates()
        {
            // Arrange
            var categories = new List<RoomCategory>
            {
                new RoomCategory { CategoryId = 1, CategoryName = "Standard" }
            };

            // It should call SearchAvailableCategoriesAsync with checkInDate = Today and checkOutDate = Today + 1
            _mockHomeService.Setup(s => s.SearchAvailableCategoriesAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchRooms("invalid-date", "invalid-date", 2, 1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories, result.Model);
            Assert.Equal(DateTime.Today.ToString("dd/MM/yyyy"), _controller.ViewData["CheckIn"]);
            Assert.Equal(DateTime.Today.AddDays(1).ToString("dd/MM/yyyy"), _controller.ViewData["CheckOut"]);
            Assert.True((bool)_controller.ViewData["IsFiltered"]!);
        }

        [Fact]
        public async Task UC04_06_TC_002_SearchRooms_ValidDates_CallsServiceAndReturnsCategories()
        {
            // Arrange
            var checkIn = "01/12/2027";
            var checkOut = "05/12/2027";
            var expectedCheckInDate = new DateTime(2027, 12, 1);
            var expectedCheckOutDate = new DateTime(2027, 12, 5);

            var categories = new List<RoomCategory>
            {
                new RoomCategory { CategoryId = 1, CategoryName = "Standard" }
            };

            _mockHomeService.Setup(s => s.SearchAvailableCategoriesAsync(expectedCheckInDate, expectedCheckOutDate, 2, 1))
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.SearchRooms(checkIn, checkOut, 2, 1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories, result.Model);
            Assert.Equal(checkIn, _controller.ViewData["CheckIn"]);
            Assert.Equal(checkOut, _controller.ViewData["CheckOut"]);
            Assert.True((bool)_controller.ViewData["IsFiltered"]!);
            
            _mockHomeService.Verify(s => s.SearchAvailableCategoriesAsync(expectedCheckInDate, expectedCheckOutDate, 2, 1), Times.Once);
        }
    }
}
