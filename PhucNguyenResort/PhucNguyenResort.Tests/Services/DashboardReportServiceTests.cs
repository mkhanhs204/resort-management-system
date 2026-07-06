using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Dashboard;
using Xunit;

namespace PhucNguyenResort.Tests.Services
{
    public class DashboardReportServiceTests
    {
        private ResortDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ResortDbContext(options);
        }

        private async Task SeedRoomsAsync(ResortDbContext context)
        {
            // Seed 10 rooms: 8 available, 1 occupied, 1 maintenance
            var rooms = new List<Room>();
            for (int i = 1; i <= 8; i++)
            {
                rooms.Add(new Room { RoomId = i, Status = RoomStatus.Available });
            }
            rooms.Add(new Room { RoomId = 9, Status = RoomStatus.Occupied });
            rooms.Add(new Room { RoomId = 10, Status = RoomStatus.Maintenance });
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetOccupancyDataAsync_WhenBookingsExist_CalculatesCorrectPercentage()
        {
            // Arrange
            var context = GetDbContext();
            await SeedRoomsAsync(context);

            var b1 = new Booking { BookingId = 1, BookingStatus = BookingStatus.CheckedIn, CheckInDate = new DateTime(2026, 7, 1), CheckOutDate = new DateTime(2026, 7, 5) };
            var b2 = new Booking { BookingId = 2, BookingStatus = BookingStatus.CheckedOut, CheckInDate = new DateTime(2026, 7, 2), CheckOutDate = new DateTime(2026, 7, 4) };
            
            b1.BookingDetails = new List<BookingDetail> { new BookingDetail { BookingId = 1, RoomId = 1 } };
            b2.BookingDetails = new List<BookingDetail> { new BookingDetail { BookingId = 2, RoomId = 2 } };

            context.Bookings.AddRange(b1, b2);
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);

            // Act
            var result = await service.GetOccupancyDataAsync(7, 2026);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9, result.TotalRooms);
            
            var day2 = result.DailyData.FirstOrDefault(d => d.Date.Day == 2);
            Assert.NotNull(day2);
            Assert.Equal(2, day2.OccupiedRooms);
            Assert.Equal(22.22, Math.Round(day2.OccupancyRatePercent, 2));
        }

        [Fact]
        public async Task GetOccupancyDataAsync_NoBookings_ReturnsZeroPercent()
        {
            var context = GetDbContext();
            await SeedRoomsAsync(context);

            var service = new DashboardReportService(context);

            var result = await service.GetOccupancyDataAsync(8, 2026);

            Assert.NotNull(result);
            Assert.Equal(0, result.OccupancyRatePercent);
        }

        [Fact]
        public async Task GetOccupancyDataAsync_MaintenanceRoomsExcludedFromTotal()
        {
            var context = GetDbContext();
            await SeedRoomsAsync(context);

            var service = new DashboardReportService(context);

            var result = await service.GetOccupancyDataAsync(7, 2026);

            Assert.Equal(9, result.TotalRooms);
        }

        [Fact]
        public async Task GetOccupancyDataAsync_OccupancyRateCannotExceed100Percent()
        {
            var context = GetDbContext();
            // Seed 5 rooms
            for (int i = 1; i <= 5; i++)
            {
                context.Rooms.Add(new Room { RoomId = i, Status = RoomStatus.Occupied });
                context.Bookings.Add(new Booking 
                { 
                    BookingId = i, 
                    BookingStatus = BookingStatus.CheckedIn, 
                    CheckInDate = new DateTime(2026, 7, 1), 
                    CheckOutDate = new DateTime(2026, 7, 5),
                    BookingDetails = new List<BookingDetail> { new BookingDetail { BookingId = i, RoomId = i } }
                });
            }
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);

            var result = await service.GetOccupancyDataAsync(7, 2026);

            var day3 = result.DailyData.FirstOrDefault(d => d.Date.Day == 3);
            Assert.NotNull(day3);
            Assert.Equal(100, day3.OccupancyRatePercent);
            Assert.Equal(100, result.OccupancyRatePercent);
        }

        [Fact]
        public async Task GetOccupancyDataAsync_ReturnsDataForEachDayOfMonth()
        {
            var context = GetDbContext();
            await SeedRoomsAsync(context);
            var service = new DashboardReportService(context);

            var result = await service.GetOccupancyDataAsync(7, 2026);

            Assert.Equal(31, result.DailyData.Count);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_CalculatesRoomRevenue()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.RoomCharge, Amount = 2000000, CreatedAt = new DateTime(2026, 7, 1) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(2000000, result.RoomRevenue);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_CalculatesFnBRevenueIncludingRestaurant()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 500000, CreatedAt = new DateTime(2026, 7, 2) });
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { IsPaidAtRestaurant = true, IsChargedToRoom = false, TotalAmount = 200000, PaidAt = new DateTime(2026, 7, 2) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(700000, result.FnBRevenue);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_CalculatesTourRevenueIncludingPayments()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.TourCharge, Amount = 300000, CreatedAt = new DateTime(2026, 7, 3) });
            context.Payments.Add(new Payment { TourBookingId = 1, Amount = 150000, PaymentStatus = PaymentStatus.Success, PaidAt = new DateTime(2026, 7, 3) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(450000, result.TourRevenue);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_TotalRevenueMatchesSumOfCategories()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.RoomCharge, Amount = 2000000, CreatedAt = new DateTime(2026, 7, 1) });
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 500000, CreatedAt = new DateTime(2026, 7, 2) });
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { IsPaidAtRestaurant = true, IsChargedToRoom = false, TotalAmount = 200000, PaidAt = new DateTime(2026, 7, 2) });
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.TourCharge, Amount = 300000, CreatedAt = new DateTime(2026, 7, 3) });
            context.Payments.Add(new Payment { TourBookingId = 1, Amount = 150000, PaymentStatus = PaymentStatus.Success, PaidAt = new DateTime(2026, 7, 3) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(3150000, result.TotalRevenue);
            Assert.Equal(result.RoomRevenue + result.FnBRevenue + result.TourRevenue, result.TotalRevenue);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_ZeroWhenNoTransactions()
        {
            var context = GetDbContext();
            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(8, 2026);

            Assert.Equal(0, result.TotalRevenue);
            Assert.Equal(0, result.RoomRevenue);
            Assert.Equal(0, result.FnBRevenue);
            Assert.Equal(0, result.TourRevenue);
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_DoesNotDoubleCountFoodOrdersChargedToRoom()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 500000, CreatedAt = new DateTime(2026, 7, 2) });
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { FoodOrderId = 1, IsPaidAtRestaurant = true, IsChargedToRoom = false, TotalAmount = 200000, PaidAt = new DateTime(2026, 7, 2) });
            context.FoodOrders.Add(new PhucNguyenResort.Models.FoodOrder { FoodOrderId = 2, IsChargedToRoom = true, IsPaidAtRestaurant = false, TotalAmount = 500000, ChargedAt = new DateTime(2026, 7, 2) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(700000, result.FnBRevenue); // Not 1200000
        }

        [Fact]
        public async Task GetRevenueBreakdownAsync_PercentagesAreCorrect()
        {
            var context = GetDbContext();
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.RoomCharge, Amount = 2000000, CreatedAt = new DateTime(2026, 7, 1) });
            context.FolioTransactions.Add(new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 500000, CreatedAt = new DateTime(2026, 7, 2) });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetRevenueBreakdownAsync(7, 2026);

            Assert.Equal(80, result.RoomPercentage);
            Assert.Equal(20, result.FnBPercentage);
            Assert.Equal(0, result.TourPercentage);
        }

        [Fact]
        public async Task ExportToExcelAsync_ReturnsValidWorkbookBytes()
        {
            var context = GetDbContext();
            var service = new DashboardReportService(context);
            var result = await service.ExportToExcelAsync(7, 2026);

            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task ExportToPdfAsync_ReturnsValidPdfBytes()
        {
            var context = GetDbContext();
            var service = new DashboardReportService(context);
            var result = await service.ExportToPdfAsync(7, 2026);

            Assert.NotNull(result);
            Assert.True(result.Length > 4);
            Assert.Equal(0x25, result[0]); // '%'
            Assert.Equal(0x50, result[1]); // 'P'
            Assert.Equal(0x44, result[2]); // 'D'
            Assert.Equal(0x46, result[3]); // 'F'
        }

        [Fact]
        public async Task GetFinancialReportAsync_ReturnsCompleteData()
        {
            var context = GetDbContext();
            context.Bookings.Add(new Booking { BookingStatus = BookingStatus.CheckedOut });
            await context.SaveChangesAsync();

            var service = new DashboardReportService(context);
            var result = await service.GetFinancialReportAsync(7, 2026);

            Assert.NotNull(result);
            Assert.Equal(7, result.Month);
            Assert.Equal(2026, result.Year);
            Assert.True(result.CompletedBookings > 0);
        }
    }
}
