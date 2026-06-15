using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Refunds;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Services
{
    public class RefundServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task REF_TC_001_ProcessRefund_Success_WhenPending()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Refunds.Add(new Refund
                {
                    RefundId = 5,
                    BookingId = 1,
                    RefundStatus = RefundStatus.Pending,
                    RefundAmount = 1000000,
                    BankDetails = "VCB-12345"
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new RefundService(context);

                // Act
                var result = await service.ProcessRefundAsync(5);

                // Assert
                Assert.True(result.Success);
                Assert.False(result.NotFound);
                
                var savedRefund = await context.Refunds.FindAsync(5);
                Assert.Equal(RefundStatus.Completed, savedRefund.RefundStatus);
            }
        }

        [Fact]
        public async Task REF_TC_002_ProcessRefund_NotFound_WhenIdInvalid()
        {
            // Arrange
            var options = CreateNewContextOptions();
            // Không tạo Refund nào trong DB

            using (var context = new ResortDbContext(options))
            {
                var service = new RefundService(context);

                // Act
                var result = await service.ProcessRefundAsync(999);

                // Assert
                Assert.False(result.Success);
                Assert.True(result.NotFound);
            }
        }

        [Fact]
        public async Task REF_TC_003_ProcessRefund_Fail_WhenNotPending()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.Refunds.Add(new Refund
                {
                    RefundId = 6,
                    BookingId = 1,
                    RefundStatus = RefundStatus.Completed, // Đã xử lý
                    RefundAmount = 1000000,
                    BankDetails = "VCB-12345"
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new RefundService(context);

                // Act
                var result = await service.ProcessRefundAsync(6);

                // Assert
                Assert.False(result.Success);
                Assert.False(result.NotFound);
                Assert.Equal("Yêu cầu này đã được xử lý hoặc không hợp lệ.", result.ErrorMessage);
                
                var savedRefund = await context.Refunds.FindAsync(6);
                Assert.Equal(RefundStatus.Completed, savedRefund.RefundStatus); // Không thay đổi
            }
        }
    }
}
