using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PhucNguyenResort.Services.Dashboard;
using PhucNguyenResort.ViewModels.Dashboard;
using PhucNguyenResort.Web.Controllers;
using System.Reflection;
using Xunit;

namespace PhucNguyenResort.Tests.Controllers
{
    public class DashboardControllerReportTests
    {
        private readonly Mock<IDashboardReportService> _mockReportService;
        private readonly DashboardController _controller;

        public DashboardControllerReportTests()
        {
            _mockReportService = new Mock<IDashboardReportService>();

            // Setup the controller with mocked dependencies
            // We need to pass nulls for other services if we don't mock them, but it's better to pass mocks
            var mockFoodOrderService = new Mock<PhucNguyenResort.Services.FoodOrders.IFoodOrderService>();
            var mockKotService = new Mock<PhucNguyenResort.Services.Kots.IKotService>();
            var mockTableReservationService = new Mock<PhucNguyenResort.Services.TableReservations.ITableReservationService>();
            
            _controller = new DashboardController(
                mockFoodOrderService.Object,
                mockKotService.Object,
                null!, // ResortDbContext
                mockTableReservationService.Object,
                _mockReportService.Object);
        }

        [Fact]
        public void ControllerActions_HaveAuthorizeAttribute_WithRoleBusinessManager()
        {
            // Assert RPT-TC-SEC-001
            var type = typeof(DashboardController);

            var methods = new[] { "Manager", "FinancialReport", "ExportExcel", "ExportPdf" };

            foreach (var methodName in methods)
            {
                var methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                Assert.NotNull(methodInfo);

                var attributes = methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
                Assert.NotEmpty(attributes);
                
                var authAttr = (AuthorizeAttribute)attributes[0];
                Assert.Contains("Business Manager", authAttr.Roles);
            }
        }

        [Fact]
        public async Task FinancialReport_InvalidMonthOrYear_ReturnsBadRequest()
        {
            // Assert RPT-TC-SEC-002
            
            // Invalid month
            var result1 = await _controller.FinancialReport(13, 2026);
            Assert.IsType<BadRequestObjectResult>(result1);

            var result2 = await _controller.FinancialReport(0, 2026);
            Assert.IsType<BadRequestObjectResult>(result2);

            // Invalid year
            var result3 = await _controller.FinancialReport(7, 1900); // Or before 2000
            Assert.IsType<BadRequestObjectResult>(result3);
        }

        [Fact]
        public async Task ExportExcel_InvalidMonthOrYear_ReturnsBadRequest()
        {
            var result = await _controller.ExportExcel(13, 2026);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ExportPdf_InvalidMonthOrYear_ReturnsBadRequest()
        {
            var result = await _controller.ExportPdf(13, 2026);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
