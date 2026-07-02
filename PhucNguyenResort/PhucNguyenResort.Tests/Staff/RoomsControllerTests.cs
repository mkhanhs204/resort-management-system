using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.RoomCategories;
using PhucNguyenResort.Services.Rooms;
using PhucNguyenResort.ViewModels.Rooms;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Staff
{
    public class RoomsControllerTests
    {
        private readonly Mock<IRoomService> _mockRoomService;
        private readonly Mock<IRoomCategoryService> _mockCategoryService;
        private readonly RoomsController _controller;

        public RoomsControllerTests()
        {
            _mockRoomService = new Mock<IRoomService>();
            _mockCategoryService = new Mock<IRoomCategoryService>();
            _controller = new RoomsController(_mockRoomService.Object, _mockCategoryService.Object);
        }

        [Fact]
        public async Task UC59_TC_001_Edit_ValidModel_UpdatesStatusAndRedirects()
        {
            // Arrange
            var roomId = 1;
            var room = new Room { RoomId = roomId, Status = RoomStatus.Available };
            
            var model = new RoomEditVM 
            { 
                RoomId = roomId, 
                RoomNumber = "101", 
                CategoryId = 1, 
                FloorNumber = 1, 
                Status = RoomStatus.Maintenance // Changed status
            };

            _mockRoomService.Setup(s => s.GetRoomByIdAsync(roomId)).ReturnsAsync(room);
            _mockRoomService.Setup(s => s.UpdateRoomAsync(It.IsAny<Room>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(roomId, model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            
            // Verify that UpdateRoomAsync was called and Status was updated
            _mockRoomService.Verify(s => s.UpdateRoomAsync(It.Is<Room>(r => r.Status == RoomStatus.Maintenance)), Times.Once);
        }

        [Fact]
        public async Task UC59_TC_002_Edit_IdMismatch_ReturnsNotFound()
        {
            // Arrange
            var model = new RoomEditVM { RoomId = 2 };

            // Act
            var result = await _controller.Edit(1, model);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
