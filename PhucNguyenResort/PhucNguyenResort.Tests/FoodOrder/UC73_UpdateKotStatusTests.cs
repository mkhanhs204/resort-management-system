using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.Services.Kots;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.FoodOrder
{
    public class UC73_UpdateKotStatusTests : IDisposable
    {
        private readonly ResortDbContext _dbContext;
        private readonly KotService _kotService;

        public UC73_UpdateKotStatusTests()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ResortDbContext(options);
            _kotService = new KotService(_dbContext);

            SeedDataAsync().Wait();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private async Task SeedDataAsync()
        {
            var kot1 = new Kot { KotId = 1, FoodOrderId = 1, Status = KotStatus.Pending };
            var kot2 = new Kot { KotId = 2, FoodOrderId = 2, Status = KotStatus.Cooking };
            var kot3 = new Kot { KotId = 3, FoodOrderId = 3, Status = KotStatus.Ready };
            var kot4 = new Kot { KotId = 4, FoodOrderId = 4, Status = KotStatus.Pending };
            var kot5 = new Kot { KotId = 5, FoodOrderId = 5, Status = KotStatus.Completed };
            var kot6 = new Kot { KotId = 6, FoodOrderId = 6, Status = KotStatus.Pending };

            _dbContext.KOT.AddRange(kot1, kot2, kot3, kot4, kot5, kot6);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task UC73_TC_001_HappyPath_PendingToCooking()
        {
            // Act
            var result = await _kotService.UpdateKotStatusAsync(1, KotStatus.Cooking);

            // Assert
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);
            var kot = await _dbContext.KOT.FindAsync(1);
            Assert.Equal(KotStatus.Cooking, kot.Status);
        }

        [Fact]
        public async Task UC73_TC_002_HappyPath_CookingToReady()
        {
            // Act
            var result = await _kotService.UpdateKotStatusAsync(2, KotStatus.Ready);

            // Assert
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);
            var kot = await _dbContext.KOT.FindAsync(2);
            Assert.Equal(KotStatus.Ready, kot.Status);
        }

        [Fact]
        public async Task UC73_TC_003_HappyPath_ReadyToCompleted()
        {
            // Act
            var result = await _kotService.UpdateKotStatusAsync(3, KotStatus.Completed);

            // Assert
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);
            var kot = await _dbContext.KOT.FindAsync(3);
            Assert.Equal(KotStatus.Completed, kot.Status);
        }

        [Fact]
        public async Task UC73_TC_004_InvalidTransition_JumpState()
        {
            // Act (Pending -> Ready)
            var result = await _kotService.UpdateKotStatusAsync(4, KotStatus.Ready);

            // Assert
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);
            Assert.Contains("KOT-003", result.ErrorMessage);
            var kot = await _dbContext.KOT.FindAsync(4);
            Assert.Equal(KotStatus.Pending, kot.Status);
        }

        [Fact]
        public async Task UC73_TC_005_InvalidTransition_BackwardState()
        {
            // Act (Completed -> Cooking)
            var result = await _kotService.UpdateKotStatusAsync(5, KotStatus.Cooking);

            // Assert
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);
            Assert.Contains("KOT-003", result.ErrorMessage);
            var kot = await _dbContext.KOT.FindAsync(5);
            Assert.Equal(KotStatus.Completed, kot.Status);
        }

        [Fact]
        public async Task UC73_TC_006_InvalidTransition_ManualCancelled()
        {
            // Act (Pending -> Cancelled)
            var result = await _kotService.UpdateKotStatusAsync(6, KotStatus.Cancelled);

            // Assert
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);
            Assert.Contains("KOT-003", result.ErrorMessage);
            var kot = await _dbContext.KOT.FindAsync(6);
            Assert.Equal(KotStatus.Pending, kot.Status);
        }
    }
}
