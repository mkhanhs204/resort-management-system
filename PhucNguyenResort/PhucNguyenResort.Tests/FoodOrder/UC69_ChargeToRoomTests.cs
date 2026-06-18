using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.FoodOrders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.FoodOrder
{
    public class UC69_ChargeToRoomTests : IDisposable
    {
        private readonly ResortDbContext _dbContext;
        private readonly FoodOrderService _foodOrderService;

        public UC69_ChargeToRoomTests()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ResortDbContext(options);
            _foodOrderService = new FoodOrderService(_dbContext);

            SeedDataAsync().Wait();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private async Task SeedDataAsync()
        {
            // Seed User
            var user = new User
            {
                UserId = 1,
                Email = "test@user.com",
                FullName = "Test User"
            };
            var user99 = new User
            {
                UserId = 99,
                Email = "user99@user.com",
                FullName = "User 99"
            };
            _dbContext.Users.AddRange(user, user99);

            // Seed Booking
            var booking1 = new Booking
            {
                BookingId = 1,
                UserId = 1,
                RoomId = 101,
                BookingStatus = BookingStatus.CheckedIn
            };
            var booking99 = new Booking
            {
                BookingId = 99,
                UserId = 99,
                RoomId = 102,
                BookingStatus = BookingStatus.CheckedIn
            };
            _dbContext.Bookings.AddRange(booking1, booking99);

            // Seed GuestFolio (Only for booking 1)
            var folio1 = new GuestFolio
            {
                FolioId = 1,
                BookingId = 1,
                TotalFoodCharges = 0,
                Status = FolioStatus.Unpaid
            };
            _dbContext.GuestFolios.Add(folio1);

            // Seed FoodOrders
            var order1 = new PhucNguyenResort.Models.FoodOrder
            {
                FoodOrderId = 1,
                BookingId = 1,
                OrderStatus = FoodOrderStatus.Delivered,
                IsChargedToRoom = false,
                TotalAmount = 150000,
                OrderedAt = DateTime.Now
            };

            var order2 = new PhucNguyenResort.Models.FoodOrder
            {
                FoodOrderId = 2,
                BookingId = 1,
                OrderStatus = FoodOrderStatus.Cooking,
                IsChargedToRoom = false,
                TotalAmount = 50000,
                OrderedAt = DateTime.Now
            };

            var order3 = new PhucNguyenResort.Models.FoodOrder
            {
                FoodOrderId = 3,
                BookingId = 1,
                OrderStatus = FoodOrderStatus.Delivered,
                IsChargedToRoom = true,
                ChargedAt = DateTime.Now.AddMinutes(-10),
                TotalAmount = 100000,
                OrderedAt = DateTime.Now
            };

            var order4 = new PhucNguyenResort.Models.FoodOrder
            {
                FoodOrderId = 4,
                BookingId = 99, // Booking without GuestFolio
                OrderStatus = FoodOrderStatus.Delivered,
                IsChargedToRoom = false,
                TotalAmount = 200000,
                OrderedAt = DateTime.Now
            };

            var order6 = new PhucNguyenResort.Models.FoodOrder
            {
                FoodOrderId = 6,
                BookingId = 1,
                OrderStatus = FoodOrderStatus.Cancelled,
                IsChargedToRoom = false,
                TotalAmount = 50000,
                OrderedAt = DateTime.Now
            };

            _dbContext.FoodOrders.AddRange(order1, order2, order3, order4, order6);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task UC69_TC_001_HappyPath_ChargeSuccess()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(1);

            // Assert
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);

            var order = await _dbContext.FoodOrders.FindAsync(1);
            Assert.True(order.IsChargedToRoom);
            Assert.NotNull(order.ChargedAt);

            var folio = await _dbContext.GuestFolios.FindAsync(1);
            Assert.Equal(150000m, folio.TotalFoodCharges);

            var transaction = await _dbContext.FolioTransactions.FirstOrDefaultAsync(t => t.ReferenceId == 1 && t.TransactionType == FolioTransactionType.FoodCharge);
            Assert.NotNull(transaction);
            Assert.Equal(150000m, transaction.Amount);
            Assert.Equal(1, transaction.FolioId);
        }

        [Fact]
        public async Task UC69_TC_002_Invalid_OrderNotDelivered()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(2);

            // Assert
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var order = await _dbContext.FoodOrders.FindAsync(2);
            Assert.False(order.IsChargedToRoom);

            var folio = await _dbContext.GuestFolios.FindAsync(1);
            Assert.Equal(0m, folio.TotalFoodCharges);

            var transactionCount = await _dbContext.FolioTransactions.CountAsync(t => t.ReferenceId == 2);
            Assert.Equal(0, transactionCount);
        }

        [Fact]
        public async Task UC69_TC_003_Invalid_AlreadyCharged()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(3);

            // Assert
            Assert.Equal(FoodOrderActionStatus.AlreadyCharged, result.Status);

            // Ensure folio is not added again
            var folio = await _dbContext.GuestFolios.FindAsync(1);
            Assert.Equal(0m, folio.TotalFoodCharges); // Since initial is 0 and no double charge
        }

        [Fact]
        public async Task UC69_TC_004_Invalid_NoFolio()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(4);

            // Assert
            Assert.Equal(FoodOrderActionStatus.NoFolio, result.Status);

            var order = await _dbContext.FoodOrders.FindAsync(4);
            Assert.False(order.IsChargedToRoom);
        }

        [Fact]
        public async Task UC69_TC_005_Invalid_OrderNotFound()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(999);

            // Assert
            Assert.Equal(FoodOrderActionStatus.NotFound, result.Status);
        }

        [Fact]
        public async Task UC69_TC_006_Invalid_CancelledOrder()
        {
            // Act
            var result = await _foodOrderService.ChargeToRoomAsync(6);

            // Assert
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);
        }
    }
}
