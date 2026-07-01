using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Services.Gemini;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Controllers
{
    public class ChatControllerTests
    {
        private readonly Mock<IGeminiService> _mockGeminiService;
        private readonly ChatController _controller;

        public ChatControllerTests()
        {
            _mockGeminiService = new Mock<IGeminiService>();
            _controller = new ChatController(_mockGeminiService.Object);
        }

        [Fact]
        public async Task SendMessage_ValidMessage_ReturnsOk()
        {
            // Arrange
            var request = new ChatRequest
            {
                Message = "Giá phòng Standard?",
                History = new List<ChatMessageDto>()
            };
            var expectedResponse = "Phòng Standard giá 1.500.000 VNĐ/đêm ạ.";

            _mockGeminiService.Setup(s => s.ChatAsync(request.Message, It.IsAny<List<ChatMessageDto>>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.SendMessage(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            
            // DTO returned has property response
            var data = result.Value;
            Assert.NotNull(data);
            
            var responseProperty = data.GetType().GetProperty("response");
            Assert.NotNull(responseProperty);
            
            var responseValue = responseProperty.GetValue(data) as string;
            Assert.Equal(expectedResponse, responseValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SendMessage_EmptyMessage_ReturnsBadRequest(string invalidMessage)
        {
            // Arrange
            var request = new ChatRequest
            {
                Message = invalidMessage,
                History = new List<ChatMessageDto>()
            };

            // Act
            var result = await _controller.SendMessage(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Nội dung tin nhắn không được để trống.", result.Value);
            
            // GeminiService should not be called
            _mockGeminiService.Verify(s => s.ChatAsync(It.IsAny<string>(), It.IsAny<List<ChatMessageDto>>()), Times.Never);
        }
    }
}
