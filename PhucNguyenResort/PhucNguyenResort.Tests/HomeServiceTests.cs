using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Home;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests
{
    public class HomeServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            // Use a unique database name for each test to avoid test isolation issues
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task ROOM_TC_001_CheckAvailabilityAsync_EmptyBookings_ReturnsTotalRooms()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard" });
                for (int i = 1; i <= 5; i++)
                {
                    context.Rooms.Add(new Room { RoomId = i, CategoryId = 1, Status = RoomStatus.Available });
                }
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);
                
                // Act
                var result = await service.CheckAvailabilityAsync(1, new DateTime(2027, 1, 1), new DateTime(2027, 1, 5));

                // Assert
                Assert.Equal(5, result);
            }
        }

        [Fact]
        public async Task ROOM_TC_002_CheckAvailabilityAsync_NoOverlapBookings_ReturnsTotalRooms()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard" });
                for (int i = 1; i <= 5; i++)
                {
                    context.Rooms.Add(new Room { RoomId = i, CategoryId = 1, Status = RoomStatus.Available });
                }
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 3),
                    BookingStatus = BookingStatus.Reserved
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);
                
                // Act: Check 05/01/2027 to 10/01/2027 (Khoảng thời gian hoàn toàn không giao nhau)
                var result = await service.CheckAvailabilityAsync(1, new DateTime(2027, 1, 5), new DateTime(2027, 1, 10));

                // Assert
                Assert.Equal(5, result);
            }
        }

        [Fact]
        public async Task ROOM_TC_003_CheckAvailabilityAsync_OverlapBookings_ReturnsCorrectAvailableRooms()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard" });
                for (int i = 1; i <= 5; i++)
                {
                    context.Rooms.Add(new Room { RoomId = i, CategoryId = 1, Status = RoomStatus.Available });
                }
                
                // 2 bookings overlap
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    CheckInDate = new DateTime(2027, 1, 2), 
                    CheckOutDate = new DateTime(2027, 1, 6),
                    BookingStatus = BookingStatus.Reserved
                });
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 2, 
                    RoomId = 2, 
                    CheckInDate = new DateTime(2027, 1, 2), 
                    CheckOutDate = new DateTime(2027, 1, 6),
                    BookingStatus = BookingStatus.Reserved
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);
                
                // Act: Check 01/01/2027 to 05/01/2027
                var result = await service.CheckAvailabilityAsync(1, new DateTime(2027, 1, 1), new DateTime(2027, 1, 5));

                // Assert
                Assert.Equal(3, result);
            }
        }

        [Fact]
        public async Task ROOM_TC_004_CheckAvailabilityAsync_MaintenanceRooms_ExcludedFromTotal()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard" });
                
                // 2 Available, 3 Maintenance
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 2, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 3, CategoryId = 1, Status = RoomStatus.Maintenance });
                context.Rooms.Add(new Room { RoomId = 4, CategoryId = 1, Status = RoomStatus.Maintenance });
                context.Rooms.Add(new Room { RoomId = 5, CategoryId = 1, Status = RoomStatus.Maintenance });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);
                
                // Act
                var result = await service.CheckAvailabilityAsync(1, new DateTime(2027, 1, 1), new DateTime(2027, 1, 5));

                // Assert
                Assert.Equal(2, result);
            }
        }

        [Fact]
        public async Task ROOM_TC_005_CheckAvailabilityAsync_CancelledOrNoShowBookings_Ignored()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard" });
                for (int i = 1; i <= 5; i++)
                {
                    context.Rooms.Add(new Room { RoomId = i, CategoryId = 1, Status = RoomStatus.Available });
                }
                
                // 1 Cancelled, 1 NoShow
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 5),
                    BookingStatus = BookingStatus.Cancelled
                });
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 2, 
                    RoomId = 2, 
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 5),
                    BookingStatus = BookingStatus.NoShow
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);
                
                // Act
                var result = await service.CheckAvailabilityAsync(1, new DateTime(2027, 1, 1), new DateTime(2027, 1, 5));

                // Assert
                Assert.Equal(5, result);
            }
        }
    }
}
