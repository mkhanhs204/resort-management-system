using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Bookings
{
    public class BookingServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task BKG_TC_001_CreateBooking_Success_WhenRoomAvailable()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Users.Add(new User { UserId = 1, FullName = "Test User" });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new BookingService(context);
                var request = new CreateBookingViewModel
                {
                    RoomId = 1,
                    CheckInDate = DateTime.Today.AddDays(10),
                    CheckOutDate = DateTime.Today.AddDays(14),
                    NumberOfGuests = 2
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);
                
                var savedBooking = await context.Bookings.FindAsync(result.BookingId);
                Assert.Equal(BookingStatus.Reserved, savedBooking.BookingStatus);
                Assert.Equal(2000000, savedBooking.DepositAmount); // 4 đêm = 4tr, cọc 50% = 2tr
            }
        }

        [Fact]
        public async Task BKG_TC_002_CreateBooking_Error_WhenRoomAlreadyBooked()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Users.Add(new User { UserId = 1, FullName = "Test User 1" });
                context.Users.Add(new User { UserId = 2, FullName = "Test User 2" });
                
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    UserId = 1, 
                    CheckInDate = DateTime.Today.AddDays(10), 
                    CheckOutDate = DateTime.Today.AddDays(14),
                    BookingStatus = BookingStatus.Reserved
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new BookingService(context);
                var request = new CreateBookingViewModel
                {
                    RoomId = 1,
                    CheckInDate = DateTime.Today.AddDays(11),
                    CheckOutDate = DateTime.Today.AddDays(13),
                    NumberOfGuests = 2
                };

                // Act
                var result = await service.CreateBookingAsync(request, 2);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("BKG-002", result.ErrorCode);
            }
        }

        [Fact]
        public async Task BKG_TC_003_CreateBooking_Error_WhenInvalidDates()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingService(context);
                var request = new CreateBookingViewModel
                {
                    RoomId = 1,
                    CheckInDate = DateTime.Today.AddDays(14),
                    CheckOutDate = DateTime.Today.AddDays(10), // CheckOut < CheckIn
                    NumberOfGuests = 2
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("BKG-001", result.ErrorCode);
            }
        }

        [Fact]
        public async Task BKG_TC_004_CancelBooking_Refund100_WhenMoreThan48Hours()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var checkIn = DateTime.Now.AddDays(3); // > 48h
            using (var context = new ResortDbContext(options))
            {
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    UserId = 1, 
                    CheckInDate = checkIn, 
                    CheckOutDate = checkIn.AddDays(2),
                    BookingStatus = BookingStatus.Reserved,
                    DepositAmount = 1000000
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new BookingService(context);

                // Act
                var result = await service.CancelBookingAsync(1, 1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1000000, result.RefundAmount);
                var savedBooking = await context.Bookings.FindAsync(1);
                Assert.Equal(BookingStatus.Cancelled, savedBooking.BookingStatus);
            }
        }

        [Fact]
        public async Task BKG_TC_005_CancelBooking_NoRefund_WhenLessThan48Hours()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var checkIn = DateTime.Now.AddHours(24); // < 48h
            using (var context = new ResortDbContext(options))
            {
                context.Bookings.Add(new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    UserId = 1, 
                    CheckInDate = checkIn, 
                    CheckOutDate = checkIn.AddDays(2),
                    BookingStatus = BookingStatus.Reserved,
                    DepositAmount = 1000000
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var service = new BookingService(context);

                // Act
                var result = await service.CancelBookingAsync(1, 1);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, result.RefundAmount); // Không hoàn cọc
                var savedBooking = await context.Bookings.FindAsync(1);
                Assert.Equal(BookingStatus.Cancelled, savedBooking.BookingStatus);
            }
        }
    }
}
