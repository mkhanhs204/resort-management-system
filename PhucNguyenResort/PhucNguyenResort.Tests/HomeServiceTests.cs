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
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 3),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail
                {
                    BookingDetailId = 1,
                    BookingId = 1,
                    RoomCategoryId = 1
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
                
                // 2 bookings overlap, each with a BookingDetail linked to CategoryId=1
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    CheckInDate = new DateTime(2027, 1, 2), 
                    CheckOutDate = new DateTime(2027, 1, 6),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomCategoryId = 1 });
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 2, 
                    CheckInDate = new DateTime(2027, 1, 2), 
                    CheckOutDate = new DateTime(2027, 1, 6),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 2, BookingId = 2, RoomCategoryId = 1 });

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
        public async Task ROOM_TC_005_CheckAvailabilityAsync_CancelledOrCheckedOutBookings_Ignored()
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
                
                // 1 Cancelled, 1 CheckedOut — both should be excluded from availability count
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 5),
                    BookingStatus = BookingStatus.Cancelled
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomCategoryId = 1 });
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 2, 
                    CheckInDate = new DateTime(2027, 1, 1), 
                    CheckOutDate = new DateTime(2027, 1, 5),
                    BookingStatus = BookingStatus.CheckedOut
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 2, BookingId = 2, RoomCategoryId = 1 });

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
        public async Task ROOM_TC_007_SearchAvailableCategoriesAsync_CapacityExceeded_FiltersOutCategory()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                // Category 1: Capacity 2 adults, 1 child
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", MaxAdults = 2, MaxChildren = 1 });
                // Category 2: Capacity 4 adults, 2 children
                context.RoomCategories.Add(new RoomCategory { CategoryId = 2, CategoryName = "Deluxe", MaxAdults = 4, MaxChildren = 2 });

                // Add 1 available room to each to make sure they aren't filtered out by availability
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 2, CategoryId = 2, Status = RoomStatus.Available });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);

                // Act: Request 3 adults, 1 child, 1 room
                var result = await service.SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), 3, 1, 1);

                // Assert: Only Category 2 should be returned
                var list = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<RoomCategory>>(result);
                var category = Assert.Single(list);
                Assert.Equal(2, category.CategoryId);
            }
        }

        [Fact]
        public async Task ROOM_TC_008_SearchAvailableCategoriesAsync_InsufficientAvailableRooms_FiltersOutCategory()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                // Category 1: Capacity matches (4 adults) but only has 1 available room (total 2 rooms, but 1 booked)
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", MaxAdults = 4, MaxChildren = 2 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 2, CategoryId = 1, Status = RoomStatus.Available });

                // Booking overlaps and occupies 1 room of Category 1
                context.Bookings.Add(new Booking
                {
                    BookingId = 1,
                    CheckInDate = new DateTime(2027, 1, 2),
                    CheckOutDate = new DateTime(2027, 1, 4),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomCategoryId = 1 });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);

                // Act: Request 2 rooms of Category 1
                var result = await service.SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), 2, 0, 2);

                // Assert: Empty list because only 1 room is available
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task ROOM_TC_009_SearchAvailableCategoriesAsync_ValidParameters_ReturnsCorrectCategoriesWithCount()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                // Category 1: 5 rooms total, 2 available (3 maintenance)
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", MaxAdults = 2, MaxChildren = 1 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 2, CategoryId = 1, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 3, CategoryId = 1, Status = RoomStatus.Maintenance });
                context.Rooms.Add(new Room { RoomId = 4, CategoryId = 1, Status = RoomStatus.Maintenance });
                context.Rooms.Add(new Room { RoomId = 5, CategoryId = 1, Status = RoomStatus.Maintenance });

                // Category 2: 3 rooms total, 1 available (2 booked)
                context.RoomCategories.Add(new RoomCategory { CategoryId = 2, CategoryName = "Deluxe", MaxAdults = 4, MaxChildren = 2 });
                context.Rooms.Add(new Room { RoomId = 6, CategoryId = 2, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 7, CategoryId = 2, Status = RoomStatus.Available });
                context.Rooms.Add(new Room { RoomId = 8, CategoryId = 2, Status = RoomStatus.Available });

                context.Bookings.Add(new Booking
                {
                    BookingId = 1,
                    CheckInDate = new DateTime(2027, 1, 2),
                    CheckOutDate = new DateTime(2027, 1, 4),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, BookingId = 1, RoomCategoryId = 2 });

                context.Bookings.Add(new Booking
                {
                    BookingId = 2,
                    CheckInDate = new DateTime(2027, 1, 3),
                    CheckOutDate = new DateTime(2027, 1, 5),
                    BookingStatus = BookingStatus.Reserved
                });
                context.BookingDetails.Add(new BookingDetail { BookingDetailId = 2, BookingId = 2, RoomCategoryId = 2 });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new HomeService(context);

                // Act: Request 1 room, 2 adults, 1 child
                var result = await service.SearchAvailableCategoriesAsync(new DateTime(2027, 1, 1), new DateTime(2027, 1, 5), 2, 1, 1);

                // Assert: Both should be returned but with correct AvailableRoomsCount set
                var list = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<RoomCategory>>(result);
                var categoryList = new System.Collections.Generic.List<RoomCategory>(list);

                Assert.Equal(2, categoryList.Count);

                var cat1 = categoryList.Find(c => c.CategoryId == 1);
                Assert.NotNull(cat1);
                Assert.Equal(2, cat1.AvailableRoomsCount); // 5 total - 3 maintenance = 2 available

                var cat2 = categoryList.Find(c => c.CategoryId == 2);
                Assert.NotNull(cat2);
                Assert.Equal(1, cat2.AvailableRoomsCount); // 3 total - 2 booked = 1 available
            }
        }
    }
}
