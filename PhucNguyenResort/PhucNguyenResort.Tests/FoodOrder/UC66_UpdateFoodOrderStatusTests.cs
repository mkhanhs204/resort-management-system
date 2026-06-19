using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.FoodOrders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.FoodOrder
{
    /// <summary>
    /// TDD Spec: UC66 — Update Food Order Status
    /// Document: PHUCNGUYEN-FOODORDER-TDD-002 v1.0
    /// 
    /// LUU Y VE ENUM:
    /// EDS chuan moi dinh nghia: Pending -> Cooking -> Delivered -> Cancelled
    /// Code hien tai dang dung:   Pending -> Preparing -> Ready -> Served -> Cancelled
    /// Test nay dung enum HIEN TAI de compile duoc. Khi refactor enum, chi can rename.
    /// Mapping: Preparing = Cooking (EDS), Served = Delivered (EDS)
    /// </summary>
    public class UC66_UpdateFoodOrderStatusTests : IDisposable
    {
        private readonly ResortDbContext _dbContext;
        private readonly FoodOrderService _foodOrderService;

        public UC66_UpdateFoodOrderStatusTests()
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
            // Booking chuan (CheckedIn)
            _dbContext.Users.Add(new User { UserId = 2, FullName = "Customer 2" });
            _dbContext.Rooms.Add(new Room { RoomId = 1, RoomNumber = "101" });
            var booking = new Booking // FX-BK-001
            {
                BookingId = 2,
                UserId = 2,
                BookingStatus = BookingStatus.CheckedIn
            };
            booking.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, RoomId = 1, BookingId = 2 });
            _dbContext.Bookings.Add(booking);

            // FX-ORD-001: Don hang Pending (cho chuyen sang Preparing/Cancelled)
            _dbContext.FoodOrders.Add(new Models.FoodOrder
            {
                FoodOrderId = 1,
                BookingId = 2,
                OrderStatus = FoodOrderStatus.Pending,
                TotalAmount = 100000,
                IsChargedToRoom = false,
                Kot = new Kot { KotId = 1, Status = KotStatus.Pending, CreatedAt = DateTime.Now }
            });

            // FX-ORD-002: Don dang nau, bep xong (KOT = Ready) -> cho chuyen sang Served
            _dbContext.FoodOrders.Add(new Models.FoodOrder
            {
                FoodOrderId = 2,
                BookingId = 2,
                OrderStatus = FoodOrderStatus.Cooking,
                TotalAmount = 200000,
                IsChargedToRoom = false,
                Kot = new Kot { KotId = 2, Status = KotStatus.Ready, CreatedAt = DateTime.Now }
            });

            // FX-ORD-003: Don da giao (terminal state)
            _dbContext.FoodOrders.Add(new Models.FoodOrder
            {
                FoodOrderId = 3,
                BookingId = 2,
                OrderStatus = FoodOrderStatus.Delivered,
                TotalAmount = 300000,
                IsChargedToRoom = false,
                Kot = new Kot { KotId = 3, Status = KotStatus.Completed, CreatedAt = DateTime.Now }
            });

            // FX-ORD-004: Don da huy (terminal state)
            _dbContext.FoodOrders.Add(new Models.FoodOrder
            {
                FoodOrderId = 4,
                BookingId = 2,
                OrderStatus = FoodOrderStatus.Cancelled,
                TotalAmount = 150000,
                IsChargedToRoom = false,
                Kot = new Kot { KotId = 4, Status = KotStatus.Cancelled, CreatedAt = DateTime.Now }
            });

            // FX-ORD-005: Don dang nau nhung bep CHUA xong (KOT = Pending)
            _dbContext.FoodOrders.Add(new Models.FoodOrder
            {
                FoodOrderId = 5,
                BookingId = 2,
                OrderStatus = FoodOrderStatus.Cooking,
                TotalAmount = 250000,
                IsChargedToRoom = false,
                Kot = new Kot { KotId = 5, Status = KotStatus.Pending, CreatedAt = DateTime.Now }
            });

            await _dbContext.SaveChangesAsync();
        }

        // ============================================================
        // HAPPY PATH — Cac transition hop le
        // ============================================================

        [Fact] // UC66-TC-001: Pending -> Preparing (EDS: Pending -> Cooking)
        public async Task UC66_TC_001_UpdateStatus_Pending_To_Preparing_Success()
        {
            // Act
            var result = await _foodOrderService.UpdateOrderStatusAsync(1, FoodOrderStatus.Cooking);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(1);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Cooking, dbOrder.OrderStatus); // DB phai cap nhat
        }

        [Fact] // UC66-TC-002: Preparing -> Served khi KOT = Ready (EDS: Cooking -> Delivered)
        public async Task UC66_TC_002_UpdateStatus_Preparing_To_Served_WhenKotReady_Success()
        {
            // Act — FX-ORD-002 co KOT status = Ready
            var result = await _foodOrderService.UpdateOrderStatusAsync(2, FoodOrderStatus.Delivered);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(2);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Delivered, dbOrder.OrderStatus);
        }

        [Fact] // UC66-TC-003: Pending -> Cancelled
        public async Task UC66_TC_003_UpdateStatus_Pending_To_Cancelled_Success()
        {
            // Act
            var result = await _foodOrderService.UpdateOrderStatusAsync(1, FoodOrderStatus.Cancelled);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.Success, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(1);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Cancelled, dbOrder.OrderStatus);
        }

        // ============================================================
        // INVALID TRANSITIONS — Cac transition bat hop le
        // ============================================================

        [Fact] // UC66-TC-004: Pending -> Served (nhay coc, khong duoc phep)
        public async Task UC66_TC_004_UpdateStatus_Pending_To_Served_InvalidState()
        {
            // Act
            var result = await _foodOrderService.UpdateOrderStatusAsync(1, FoodOrderStatus.Delivered);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(1);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Pending, dbOrder.OrderStatus); // KHONG bi thay doi
        }

        [Fact] // UC66-TC-005: Preparing -> Cancelled (khong cho huy khi dang nau)
        public async Task UC66_TC_005_UpdateStatus_Preparing_To_Cancelled_InvalidState()
        {
            // Act — FX-ORD-002 dang Preparing
            var result = await _foodOrderService.UpdateOrderStatusAsync(2, FoodOrderStatus.Cancelled);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(2);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Cooking, dbOrder.OrderStatus); // KHONG bi thay doi
        }

        [Fact] // UC66-TC-006: Served -> Pending (lui trang thai)
        public async Task UC66_TC_006_UpdateStatus_Served_To_Pending_InvalidState()
        {
            // Act — FX-ORD-003 dang Served
            var result = await _foodOrderService.UpdateOrderStatusAsync(3, FoodOrderStatus.Pending);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(3);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Delivered, dbOrder.OrderStatus); // KHONG bi thay doi
        }

        [Fact] // UC66-TC-007: Served -> Preparing (lui trang thai)
        public async Task UC66_TC_007_UpdateStatus_Served_To_Preparing_InvalidState()
        {
            // Act — FX-ORD-003 dang Served
            var result = await _foodOrderService.UpdateOrderStatusAsync(3, FoodOrderStatus.Cooking);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(3);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Delivered, dbOrder.OrderStatus); // KHONG bi thay doi
        }

        [Fact] // UC66-TC-008: Cancelled -> Pending (hoi sinh don da huy)
        public async Task UC66_TC_008_UpdateStatus_Cancelled_To_Pending_InvalidState()
        {
            // Act — FX-ORD-004 da Cancelled
            var result = await _foodOrderService.UpdateOrderStatusAsync(4, FoodOrderStatus.Pending);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(4);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Cancelled, dbOrder.OrderStatus); // KHONG bi thay doi
        }

        // ============================================================
        // NOT FOUND
        // ============================================================

        [Fact] // UC66-TC-009: FoodOrderId khong ton tai
        public async Task UC66_TC_009_UpdateStatus_OrderNotFound()
        {
            // Act
            var result = await _foodOrderService.UpdateOrderStatusAsync(999, FoodOrderStatus.Cooking);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.NotFound, result.Status);
        }

        // ============================================================
        // CROSS-STATUS CONSTRAINT (Dual Status Model — ADR-002)
        // ============================================================

        [Fact] // UC66-TC-010: Preparing -> Served BI CHAN khi KOT chua Ready
        public async Task UC66_TC_010_UpdateStatus_Preparing_To_Served_BlockedWhenKotNotReady()
        {
            // Act — FX-ORD-005 co KOT status = Pending (bep chua nau xong)
            var result = await _foodOrderService.UpdateOrderStatusAsync(5, FoodOrderStatus.Delivered);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderActionStatus.InvalidState, result.Status);

            var dbOrder = await _dbContext.FoodOrders.FindAsync(5);
            Assert.NotNull(dbOrder);
            Assert.Equal(FoodOrderStatus.Cooking, dbOrder.OrderStatus); // KHONG bi thay doi
        }
    }
}
