using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.FoodOrders;
using PhucNguyenResort.Services.FoodOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.FoodOrder
{
    public class UC11_FoodOrderServiceTests : IDisposable
    {
        private readonly ResortDbContext _dbContext;
        private readonly FoodOrderService _foodOrderService;

        public UC11_FoodOrderServiceTests()
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
            var cat = new MenuCategory { CategoryId = 1, CategoryName = "Main" };
            _dbContext.MenuCategories.Add(cat);

            // Users
            _dbContext.Users.Add(new User { UserId = 2, FullName = "Customer 2" });
            _dbContext.Users.Add(new User { UserId = 7, FullName = "Customer 7" });

            // Room
            _dbContext.Rooms.Add(new Room { RoomId = 1, RoomNumber = "101" });

            // Bookings
            _dbContext.Bookings.Add(new Booking { BookingId = 2, UserId = 2, RoomId = 1, BookingStatus = BookingStatus.CheckedIn }); // FX-BK-001
            _dbContext.Bookings.Add(new Booking { BookingId = 7, UserId = 7, RoomId = 1, BookingStatus = BookingStatus.CheckedIn }); // FX-BK-002
            _dbContext.Bookings.Add(new Booking { BookingId = 1, UserId = 2, RoomId = 1, BookingStatus = BookingStatus.Reserved }); // FX-BK-003
            
            // Menu Items
            _dbContext.MenuItems.Add(new MenuItem { MenuItemId = 1, Price = 85000, IsAvailable = true, IsDeleted = false, CategoryId = 1 }); // FX-MN-001
            _dbContext.MenuItems.Add(new MenuItem { MenuItemId = 8, Price = 260000, IsAvailable = false, IsDeleted = false, CategoryId = 1 }); // FX-MN-002
            _dbContext.MenuItems.Add(new MenuItem { MenuItemId = 7, Price = 235000, IsAvailable = true, IsDeleted = false, CategoryId = 1 }); // FX-MN-003
            _dbContext.MenuItems.Add(new MenuItem { MenuItemId = 10, Price = 310000, IsAvailable = true, IsDeleted = false, CategoryId = 1 }); // FX-MN-004
            _dbContext.MenuItems.Add(new MenuItem { MenuItemId = 9, Price = 285000, IsAvailable = true, IsDeleted = true, CategoryId = 1 }); // FX-MN-005

            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task UC11_TC_001_CreateOrder_HappyPath_FiltersZeroQuantities()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 2 },
                    new FoodOrderItemRequest { MenuItemId = 7, Quantity = 1 },
                    new FoodOrderItemRequest { MenuItemId = 10, Quantity = 0 } // Should be filtered out
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.Created, result.Status);
            Assert.True(result.FoodOrderId > 0);

            var dbOrder = await _dbContext.FoodOrders
                .Include(o => o.FoodOrderItems)
                .Include(o => o.Kot)
                .FirstOrDefaultAsync(o => o.FoodOrderId == result.FoodOrderId);

            Assert.NotNull(dbOrder);
            Assert.Equal(2, dbOrder.BookingId);
            Assert.Equal(FoodOrderStatus.Pending, dbOrder.OrderStatus);
            
            
            // Expected Amount: 85000*2 + 235000*1 = 405000
            Assert.Equal(405000m, dbOrder.TotalAmount);
            
            // Check items: only 2 items (10 should be filtered out)
            Assert.Equal(2, dbOrder.FoodOrderItems.Count);
            Assert.Contains(dbOrder.FoodOrderItems, i => i.MenuItemId == 1 && i.Quantity == 2 && i.UnitPrice == 85000);
            Assert.Contains(dbOrder.FoodOrderItems, i => i.MenuItemId == 7 && i.Quantity == 1 && i.UnitPrice == 235000);

            // Check KOT
            Assert.NotNull(dbOrder.Kot);
            Assert.Equal(KotStatus.Pending, dbOrder.Kot.Status);
        }

        [Fact]
        public async Task UC11_TC_002_CreateOrder_AccessDenied_NotOwner()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 7, // Belongs to User 7
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 1 }
                }
            };
            var currentUserId = 2; // User 2 trying to order for Booking 7

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.Forbidden, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_003_CreateOrder_InvalidState_NotCheckedIn()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 1, // Reserved, not CheckedIn
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 1 }
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.BookingNotCheckedIn, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_004_CreateOrder_ValidationError_AllQuantitiesZero()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 0 },
                    new FoodOrderItemRequest { MenuItemId = 7, Quantity = 0 }
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.NoItems, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_005_CreateOrder_UnavailableMenuItem()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 8, Quantity = 1 } // IsAvailable = false
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.MenuItemUnavailable, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_006_CreateOrder_InvalidQuantity_Negative()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = -1 }
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.InvalidQuantity, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_007_CreateOrder_InvalidQuantity_Exceeds1000()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 1001 }
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.InvalidQuantity, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_008_CreateOrder_BookingNotFound()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 999, // Does not exist
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 1 }
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.BookingNotFound, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_009_CreateOrder_DeletedMenuItem()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 9, Quantity = 1 } // IsDeleted = true
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.MenuItemUnavailable, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_010_CreateOrder_MenuItemNotFound()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 999, Quantity = 1 } // Does not exist
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.MenuItemNotFound, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_011_CreateOrder_DuplicateMenuItem()
        {
            // Arrange
            var request = new FoodOrderCreateRequest
            {
                BookingId = 2,
                Items = new List<FoodOrderItemRequest>
                {
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 1 },
                    new FoodOrderItemRequest { MenuItemId = 1, Quantity = 2 } // Duplicate MenuItemId
                }
            };
            var currentUserId = 2;

            // Act
            var result = await _foodOrderService.CreateFoodOrderAsync(request, currentUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(FoodOrderCreateStatus.DuplicateMenuItem, result.Status);

            var dbOrdersCount = await _dbContext.FoodOrders.CountAsync();
            Assert.Equal(0, dbOrdersCount);
        }

        [Fact]
        public async Task UC11_TC_012_BuildMenu_HappyPath()
        {
            // Act
            var result = await _foodOrderService.BuildRoomServiceOrderAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.BookingId);
            Assert.True(result.CanPlaceOrder);
            
            // Should contain 1, 7, 10
            Assert.NotNull(result.MenuItems);
            Assert.Equal(3, result.MenuItems.Count);
            Assert.Contains(result.MenuItems, m => m.MenuItemId == 1);
            Assert.Contains(result.MenuItems, m => m.MenuItemId == 7);
            Assert.Contains(result.MenuItems, m => m.MenuItemId == 10);
            
            // Should NOT contain 8 (unavailable) and 9 (deleted)
            Assert.DoesNotContain(result.MenuItems, m => m.MenuItemId == 8);
            Assert.DoesNotContain(result.MenuItems, m => m.MenuItemId == 9);
        }

        [Fact]
        public async Task UC11_TC_013_BuildMenu_InvalidBooking()
        {
            // Act 1: Not CheckedIn
            var result1 = await _foodOrderService.BuildRoomServiceOrderAsync(1);

            // Act 2: Not Found
            var result2 = await _foodOrderService.BuildRoomServiceOrderAsync(999);

            // Assert
            // Since BuildRoomServiceOrderAsync might return a viewmodel with CanPlaceOrder = false
            // according to the current code, or null if booking not found.
            // The TDD specifies "Cả 2 lần gọi đều trả về null".
            Assert.Null(result1);
            Assert.Null(result2);
        }
    }
}
