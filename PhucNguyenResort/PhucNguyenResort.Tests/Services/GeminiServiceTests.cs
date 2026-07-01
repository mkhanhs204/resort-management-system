using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Gemini;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Services
{
    public class GeminiServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private IConfiguration CreateMockConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"Gemini:ApiKey", "fake-api-key"},
                {"Gemini:Model", "gemini-1.5-flash"},
                {"Gemini:ApiUrl", "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task ChatAsync_ValidRequest_SendsCorrectPayloadAndReturnsResponse()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory
                {
                    CategoryId = 1,
                    CategoryName = "Standard Room",
                    PricePerNight = 1200000,
                    MaxAdults = 2,
                    MaxChildren = 1,
                    Description = "Phòng tiêu chuẩn ấm cúng"
                });
                await context.SaveChangesAsync();
            }

            var configuration = CreateMockConfiguration();
            var mockLogger = new Mock<ILogger<GeminiService>>();

            // Mock HttpClient to verify the payload sent to Gemini
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string capturedRequestContent = "";

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    capturedRequestContent = req.Content.ReadAsStringAsync().Result;
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        candidates = new[]
                        {
                            new
                            {
                                content = new
                                {
                                    parts = new[]
                                    {
                                        new { text = "Chào anh/chị, phòng Standard hiện có giá là 1.200.000 VNĐ." }
                                    }
                                }
                            }
                        }
                    }))
                });

            var httpClient = new HttpClient(handlerMock.Object);

            using (var context = new ResortDbContext(options))
            {
                var service = new GeminiService(httpClient, context, configuration, mockLogger.Object);
                var history = new List<ChatMessageDto>
                {
                    new ChatMessageDto { Role = "user", Text = "Xin chào" },
                    new ChatMessageDto { Role = "model", Text = "Chào quý khách!" }
                };

                // Act
                var result = await service.ChatAsync("Tôi muốn hỏi giá phòng Standard", history);

                // Assert
                Assert.Equal("Chào anh/chị, phòng Standard hiện có giá là 1.200.000 VNĐ.", result);

                // Verify the captured request JSON structure (History, System Instruction, Context)
                Assert.NotEmpty(capturedRequestContent);
                using var doc = JsonDocument.Parse(capturedRequestContent);
                var root = doc.RootElement;

                // 1. Verify history is included
                var contents = root.GetProperty("contents");
                Assert.Equal(3, contents.GetArrayLength()); // 2 from history + 1 current message

                Assert.Equal("user", contents[0].GetProperty("role").GetString());
                Assert.Equal("Xin chào", contents[0].GetProperty("parts")[0].GetProperty("text").GetString());

                Assert.Equal("model", contents[1].GetProperty("role").GetString());
                Assert.Equal("Chào quý khách!", contents[1].GetProperty("parts")[0].GetProperty("text").GetString());

                Assert.Equal("user", contents[2].GetProperty("role").GetString());
                Assert.Equal("Tôi muốn hỏi giá phòng Standard", contents[2].GetProperty("parts")[0].GetProperty("text").GetString());

                // 2. Verify System Instruction exists and has Guardrails
                var systemInstruction = root.GetProperty("systemInstruction").GetProperty("parts")[0].GetProperty("text").GetString();
                Assert.Contains("ĐA NGÔN NGỮ", systemInstruction);
                Assert.Contains("BẢO VỆ THƯƠNG HIỆU", systemInstruction);
                Assert.Contains("PHẠM VI TRẢ LỜI", systemInstruction);
                Assert.Contains("TĂNG CHUYỂN ĐỔI", systemInstruction);

                // 3. Verify intent classification: Question is about "room", so it should load Room context
                Assert.Contains("[DANH SÁCH LOẠI PHÒNG NGỦ]", systemInstruction);
                Assert.Contains("Standard Room", systemInstruction);
                Assert.Contains((1200000M).ToString("N0"), systemInstruction);
            }
        }

        [Fact]
        public async Task ChatAsync_GeminiApiReturnsError_ReturnsFallbackGracefully()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var configuration = CreateMockConfiguration();
            var mockLogger = new Mock<ILogger<GeminiService>>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests, // 429 Error
                    Content = new StringContent("Quota exceeded")
                });

            var httpClient = new HttpClient(handlerMock.Object);

            using (var context = new ResortDbContext(options))
            {
                var service = new GeminiService(httpClient, context, configuration, mockLogger.Object);

                // Act
                var result = await service.ChatAsync("Xin chào", new List<ChatMessageDto>());

                // Assert
                Assert.Contains("hệ thống AI đang bận xử lý", result);
            }
        }

        [Fact]
        public async Task ChatAsync_NetworkException_ReturnsFallbackGracefully()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var configuration = CreateMockConfiguration();
            var mockLogger = new Mock<ILogger<GeminiService>>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network failure"));

            var httpClient = new HttpClient(handlerMock.Object);

            using (var context = new ResortDbContext(options))
            {
                var service = new GeminiService(httpClient, context, configuration, mockLogger.Object);

                // Act
                var result = await service.ChatAsync("Xin chào", new List<ChatMessageDto>());

                // Assert
                Assert.Contains("Đã xảy ra sự cố kết nối", result);
            }
        }

        [Theory]
        [InlineData("Tôi muốn biết giá phòng ngủ Standard", true, false, false)]
        [InlineData("Resort có những tour tham quan nào thế?", false, true, false)]
        [InlineData("Thực đơn nhà hàng hôm nay có gì ngon?", false, false, true)]
        [InlineData("Xin chào bạn nhé", true, false, false)] // default fallback to room
        public async Task ContextLoading_BasedOnIntent_InjectsCorrectDatabaseInfo(
            string message, bool expectRooms, bool expectTours, bool expectFood)
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Premium Deluxe", PricePerNight = 3500000 });
                context.Tours.Add(new Tour { TourId = 1, TourName = "Ký Ức Hội An", Price = 600000 });
                context.MenuItems.Add(new MenuItem { MenuItemId = 1, ItemName = "Bánh Mì Hội An", Price = 45000, IsAvailable = true });
                await context.SaveChangesAsync();
            }

            var configuration = CreateMockConfiguration();
            var mockLogger = new Mock<ILogger<GeminiService>>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            string systemInstruction = "";

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    var reqJson = req.Content.ReadAsStringAsync().Result;
                    using var doc = JsonDocument.Parse(reqJson);
                    systemInstruction = doc.RootElement.GetProperty("systemInstruction").GetProperty("parts")[0].GetProperty("text").GetString() ?? "";
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new
                    {
                        candidates = new[] { new { content = new { parts = new[] { new { text = "Mocked Response" } } } } }
                    }))
                });

            var httpClient = new HttpClient(handlerMock.Object);

            using (var context = new ResortDbContext(options))
            {
                var service = new GeminiService(httpClient, context, configuration, mockLogger.Object);

                // Act
                await service.ChatAsync(message, new List<ChatMessageDto>());

                // Assert
                Assert.NotEmpty(systemInstruction);

                if (expectRooms)
                {
                    Assert.Contains("[DANH SÁCH LOẠI PHÒNG NGỦ]", systemInstruction);
                    Assert.Contains("Premium Deluxe", systemInstruction);
                }
                else
                {
                    Assert.DoesNotContain("[DANH SÁCH LOẠI PHÒNG NGỦ]", systemInstruction);
                }

                if (expectTours)
                {
                    Assert.Contains("[DANH SÁCH TOUR DU LỊCH]", systemInstruction);
                    Assert.Contains("Ký Ức Hội An", systemInstruction);
                }
                else
                {
                    Assert.DoesNotContain("[DANH SÁCH TOUR DU LỊCH]", systemInstruction);
                }

                if (expectFood)
                {
                    Assert.Contains("[THỰC ĐƠN NHÀ HÀNG - CÁC MÓN TIÊU BIỂU]", systemInstruction);
                    Assert.Contains("Bánh Mì Hội An", systemInstruction);
                }
                else
                {
                    Assert.DoesNotContain("[THỰC ĐƠN NHÀ HÀNG - CÁC MÓN TIÊU BIỂU]", systemInstruction);
                }
            }
        }
    }
}
