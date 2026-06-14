using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PhucNguyenResort.Controllers;
using PhucNguyenResort.Data;
using PhucNguyenResort.Helpers;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.ViewModels.Payments;

namespace PhucNguyenResort.Tests.Payments
{
    public class VnPayIntegrationTests
    {
        [Fact]
        public void VNPAY_TC_001_CreatePaymentUrl_Should_Format_Amount_And_Return_Valid_Url()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["VNPay:TmnCode"]).Returns("XONYMEI0");
            mockConfig.Setup(c => c["VNPay:HashSecret"]).Returns("2Y3GKP2EU216H98QKZ2R9YCQ8QW3CY9K");
            mockConfig.Setup(c => c["VNPay:BaseUrl"]).Returns("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html");
            mockConfig.Setup(c => c["VNPay:Version"]).Returns("2.1.0");

            var service = new VnPayService(mockConfig.Object);
            var context = new DefaultHttpContext();
            var model = new VNPayRequestModel
            {
                OrderId = 10,
                Amount = 500000,
                CreatedDate = new DateTime(2026, 6, 13, 12, 0, 0),
                Description = "Thanh toan coc"
            };

            // Act
            var url = service.CreatePaymentUrl(context, model);

            // Assert
            Assert.NotNull(url);
            Assert.Contains("vnp_Amount=50000000", url); // Amount * 100
            Assert.Contains("vnp_TmnCode=XONYMEI0", url);
            Assert.Contains("vnp_SecureHash=", url);
        }

        [Fact]
        public void VNPAY_TC_SEC_001_Invalid_Checksum_Should_Return_97()
        {
            // Arrange
            var mockService = new Mock<IVnPayService>();
            mockService.Setup(s => s.ExecutePayment(It.IsAny<IQueryCollection>()))
                       .Returns(new VNPayResponseModel { Success = false, VnPayResponseCode = "97" });

            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_VNPay_" + Guid.NewGuid().ToString())
                .Options;
            var context = new ResortDbContext(options);

            var controller = new PaymentController(mockService.Object, context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.VNPayIPN() as JsonResult;

            // Assert
            Assert.NotNull(result);
            dynamic data = result.Value!;
            var rspCode = data.GetType().GetProperty("RspCode").GetValue(data, null);
            var message = data.GetType().GetProperty("Message").GetValue(data, null);
            Assert.Equal("97", (string)rspCode);
            Assert.Equal("Invalid Checksum", (string)message);
        }
    }
}
