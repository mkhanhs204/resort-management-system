using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Services.Refunds;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Controllers
{
    public class RefundsControllerTests
    {
        private RefundsController CreateControllerWithTempData(IRefundService service)
        {
            var controller = new RefundsController(service);
            
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            return controller;
        }

        [Fact]
        public async Task REF_TC_001_ProcessRefund_RedirectsToIndexWithSuccess_WhenServiceSucceeds()
        {
            // Arrange
            var mockService = new Mock<IRefundService>();
            mockService.Setup(s => s.ProcessRefundAsync(5))
                .ReturnsAsync(new ProcessRefundResult { Success = true });

            var controller = CreateControllerWithTempData(mockService.Object);

            // Act
            var result = await controller.ProcessRefund(5);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Contains("thành công", controller.TempData["SuccessMessage"]?.ToString());
        }

        [Fact]
        public async Task REF_TC_002_ProcessRefund_ReturnsNotFound_WhenServiceSaysNotFound()
        {
            // Arrange
            var mockService = new Mock<IRefundService>();
            mockService.Setup(s => s.ProcessRefundAsync(999))
                .ReturnsAsync(new ProcessRefundResult { Success = false, NotFound = true });

            var controller = CreateControllerWithTempData(mockService.Object);

            // Act
            var result = await controller.ProcessRefund(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task REF_TC_003_ProcessRefund_RedirectsToIndexWithError_WhenServiceFails()
        {
            // Arrange
            var mockService = new Mock<IRefundService>();
            mockService.Setup(s => s.ProcessRefundAsync(6))
                .ReturnsAsync(new ProcessRefundResult { Success = false, ErrorMessage = "Lỗi xử lý" });

            var controller = CreateControllerWithTempData(mockService.Object);

            // Act
            var result = await controller.ProcessRefund(6);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Lỗi xử lý", controller.TempData["Error"]?.ToString());
        }
    }
}
