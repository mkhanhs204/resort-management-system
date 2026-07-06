using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Dashboard;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Integration
{
    public class DashboardReportIntegrationTests
    {
        private ResortDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ResortDbContext(options);
        }

        [Fact]
        public async Task RPT_TC_INT_001_FullDashboardAndExportFlow_WorksCorrectly()
        {
            // Arrange - Seed Data
            var context = GetDbContext();
            
            // FX-001: 10 Rooms
            for (int i = 1; i <= 8; i++)
                context.Rooms.Add(new Room { RoomId = i, Status = RoomStatus.Available });
            context.Rooms.Add(new Room { RoomId = 9, Status = RoomStatus.Occupied });
            context.Rooms.Add(new Room { RoomId = 10, Status = RoomStatus.Maintenance });
            
            var user = new User { UserId = 1, FullName = "Test Guest" };
            context.Users.Add(user);

            // FX-002, FX-003, FX-011: Bookings and Details
            var booking1 = new Booking { BookingId = 1, UserId = 1, BookingStatus = BookingStatus.CheckedIn, CheckInDate = new DateTime(2026, 7, 1), CheckOutDate = new DateTime(2026, 7, 5) };
            var booking2 = new Booking { BookingId = 2, UserId = 1, BookingStatus = BookingStatus.CheckedOut, CheckInDate = new DateTime(2026, 7, 2), CheckOutDate = new DateTime(2026, 7, 4) };
            context.Bookings.Add(booking1);
            context.Bookings.Add(booking2);
            context.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomId = 1 });
            context.BookingDetails.Add(new BookingDetail { BookingDetailId = 2, BookingId = 2, RoomId = 2 });

            // FX-005: Room Revenue
            context.FolioTransactions.Add(new FolioTransaction { TransactionId = 1, TransactionType = FolioTransactionType.RoomCharge, Amount = 2000000, CreatedAt = new DateTime(2026, 7, 1) });
            
            // FX-006: F&B Revenue (Charge to Room)
            context.FolioTransactions.Add(new FolioTransaction { TransactionId = 2, TransactionType = FolioTransactionType.FoodCharge, Amount = 500000, CreatedAt = new DateTime(2026, 7, 2) });
            
            // FX-007: Tour Revenue (Charge to Room)
            context.FolioTransactions.Add(new FolioTransaction { TransactionId = 3, TransactionType = FolioTransactionType.TourCharge, Amount = 300000, CreatedAt = new DateTime(2026, 7, 3) });
            
            // FX-008: F&B Revenue (Paid at Restaurant)
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { FoodOrderId = 1, IsPaidAtRestaurant = true, IsChargedToRoom = false, TotalAmount = 200000, PaidAt = new DateTime(2026, 7, 2) });
            
            // FX-009: F&B Revenue (Charge to Room - should not be double counted)
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { FoodOrderId = 2, IsChargedToRoom = true, IsPaidAtRestaurant = false, TotalAmount = 500000, ChargedAt = new DateTime(2026, 7, 2) });
            
            // FX-010: Tour Revenue (Direct payment)
            context.Payments.Add(new Payment { PaymentId = 1, TourBookingId = 1, Amount = 150000, PaymentStatus = PaymentStatus.Success, PaidAt = new DateTime(2026, 7, 3) });

            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);

            // Act & Assert 1: Occupancy
            var occupancy = await service.GetOccupancyDataAsync(7, 2026);
            Assert.True(occupancy.TotalRooms == 9); // Exclude maintenance
            Assert.True(occupancy.OccupancyRatePercent > 0);
            Assert.True(occupancy.OccupancyRatePercent <= 100);

            // Act & Assert 2: Revenue Breakdown
            var revenue = await service.GetRevenueBreakdownAsync(7, 2026);
            Assert.Equal(2000000, revenue.RoomRevenue);
            Assert.Equal(700000, revenue.FnBRevenue); // 500k + 200k
            Assert.Equal(450000, revenue.TourRevenue); // 300k + 150k
            Assert.Equal(3150000, revenue.TotalRevenue);
            Assert.Equal(revenue.TotalRevenue, revenue.RoomRevenue + revenue.FnBRevenue + revenue.TourRevenue);

            // Act & Assert 3: Financial Report
            var report = await service.GetFinancialReportAsync(7, 2026);
            Assert.Equal(7, report.Month);
            Assert.Equal(2026, report.Year);
            Assert.Equal(revenue.TotalRevenue, report.TotalRevenue);

            // Act & Assert 4: Export to Excel
            var excelBytes = await service.ExportToExcelAsync(7, 2026);
            Assert.NotNull(excelBytes);
            Assert.True(excelBytes.Length > 0);

            // Act & Assert 5: Export to PDF
            var pdfBytes = await service.ExportToPdfAsync(7, 2026);
            Assert.NotNull(pdfBytes);
            Assert.True(pdfBytes.Length > 0);
            
            // Verify PDF Magic Number (%PDF)
            Assert.Equal(0x25, pdfBytes[0]); // '%'
            Assert.Equal(0x50, pdfBytes[1]); // 'P'
            Assert.Equal(0x44, pdfBytes[2]); // 'D'
            Assert.Equal(0x46, pdfBytes[3]); // 'F'
        }
    }
}
