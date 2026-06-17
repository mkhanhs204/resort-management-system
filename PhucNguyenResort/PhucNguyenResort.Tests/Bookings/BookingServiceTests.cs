using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    NumberOfAdults = 2,
                    NumberOfChildren = 0
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);
                
                var savedBooking = await context.Bookings.FindAsync(result.BookingId);
                Assert.Equal(BookingStatus.PendingPayment, savedBooking.BookingStatus);
                Assert.Equal(1600000, savedBooking.DepositAmount); // 4 đêm = 4tr, cọc 40% = 1.6tr
                Assert.Equal(4000000, savedBooking.TotalAmount); // Tổng 4tr
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
                    NumberOfAdults = 2,
                    NumberOfChildren = 0
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
                    NumberOfAdults = 2,
                    NumberOfChildren = 0
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("BKG-001", result.ErrorCode);
            }
        }

        [Fact]
        public async Task BKG_TC_004_CreateBooking_WithTourAddOn_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Users.Add(new User { UserId = 1, FullName = "Test User" });
                
                context.Tours.Add(new Tour { TourId = 1, TourName = "Island Hopping", Price = 200000 });
                context.TourSchedules.Add(new TourSchedule { ScheduleId = 5, TourId = 1, StartTime = new TimeSpan(8, 0, 0) });
                
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
                    NumberOfAdults = 2,
                NumberOfChildren = 0,
                    SelectedTourScheduleIds = new List<int> { 5 }
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);
                
                var savedBooking = await context.Bookings
                    .Include(b => b.TourBookings)
                    .FirstOrDefaultAsync(b => b.BookingId == result.BookingId);
                    
                Assert.NotNull(savedBooking);
                Assert.Single(savedBooking.TourBookings); // Phải có đúng 1 tour booking đi kèm
                
                var tourBooking = savedBooking.TourBookings.First();
                Assert.Equal(5, tourBooking.ScheduleId);
                Assert.Equal(400000, tourBooking.TotalAmount); // 200,000 * 2 guests
                Assert.Equal(TourBookingStatus.Pending, tourBooking.BookingStatus);
            }
        }

        [Fact]
        public async Task BKG_TC_005_CancelBooking_Refund100_WhenMoreThan48Hours()
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
                var result = await service.CancelBookingAsync(1, 1, "Bận việc", "VCB-123");

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1000000, result.RefundAmount);
                var savedBooking = await context.Bookings.FindAsync(1);
                Assert.Equal(BookingStatus.Cancelled, savedBooking.BookingStatus);
                
                var refund = await context.Refunds.FirstOrDefaultAsync(r => r.BookingId == 1);
                Assert.NotNull(refund);
                Assert.Equal(RefundStatus.Pending, refund.RefundStatus);
                Assert.Equal("Bận việc", refund.RefundReason);
                Assert.Equal("VCB-123", refund.BankDetails);
            }
        }

        [Fact]
        public async Task BKG_TC_006_CancelBooking_NoRefund_WhenLessThan48Hours()
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
                
                var refund = await context.Refunds.FirstOrDefaultAsync(r => r.BookingId == 1);
                Assert.Null(refund);
            }
        }

        [Fact]
        public async Task BKG_TC_007_CancelBooking_ShouldCancelAssociatedTourBookings()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var checkIn = DateTime.Now.AddDays(3);
            using (var context = new ResortDbContext(options))
            {
                var booking = new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    UserId = 1, 
                    CheckInDate = checkIn, 
                    CheckOutDate = checkIn.AddDays(2),
                    BookingStatus = BookingStatus.Reserved,
                    DepositAmount = 1000000
                };
                context.Bookings.Add(booking);

                context.Tours.Add(new Tour { TourId = 1, TourName = "Island Hopping", Price = 250000 });
                context.TourSchedules.Add(new TourSchedule { ScheduleId = 1, TourId = 1, AvailableSlots = 10 });
                
                context.TourBookings.Add(new TourBooking
                {
                    TourBookingId = 1,
                    BookingId = 1,
                    ScheduleId = 1,
                    Participants = 2,
                    TotalAmount = 500000,
                    BookingStatus = TourBookingStatus.Pending,
                    UserId = 1
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
                var savedBooking = await context.Bookings.FindAsync(1);
                                Assert.Equal(BookingStatus.Cancelled, savedBooking.BookingStatus);
                
                var savedTourBooking = await context.TourBookings.FindAsync(1);
                Assert.Equal(TourBookingStatus.Cancelled, savedTourBooking.BookingStatus);
            }
        }

        [Fact]
        public async Task BKG_TC_008_CreateBooking_ShouldDecrementTourAvailableSlots()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Users.Add(new User { UserId = 1, FullName = "Test User" });
                
                context.Tours.Add(new Tour { TourId = 1, TourName = "Island Hopping", Price = 200000 });
                context.TourSchedules.Add(new TourSchedule { ScheduleId = 5, TourId = 1, AvailableSlots = 10 });
                
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
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    SelectedTourScheduleIds = new List<int> { 5 }
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.True(result.Success);
                var schedule = await context.TourSchedules.FindAsync(5);
                Assert.Equal(8, schedule.AvailableSlots); // 10 - 2 = 8
            }
        }

        [Fact]
        public async Task BKG_TC_009_CreateBooking_ShouldFail_WhenTourSlotsInsufficient()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1, Status = RoomStatus.Available });
                context.Users.Add(new User { UserId = 1, FullName = "Test User" });
                
                context.Tours.Add(new Tour { TourId = 1, TourName = "Island Hopping", Price = 200000 });
                context.TourSchedules.Add(new TourSchedule { ScheduleId = 5, TourId = 1, AvailableSlots = 1 });
                
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
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    SelectedTourScheduleIds = new List<int> { 5 }
                };

                // Act
                var result = await service.CreateBookingAsync(request, 1);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("BKG-003", result.ErrorCode);
                
                var bookingsCount = await context.Bookings.CountAsync();
                Assert.Equal(0, bookingsCount); // Booking không được lưu
            }
        }

        [Fact]
        public async Task BKG_TC_010_CancelBooking_ShouldRestoreTourAvailableSlots()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var checkIn = DateTime.Now.AddDays(3);
            using (var context = new ResortDbContext(options))
            {
                var booking = new Booking 
                { 
                    BookingId = 1, 
                    RoomId = 1, 
                    UserId = 1, 
                    CheckInDate = checkIn, 
                    CheckOutDate = checkIn.AddDays(2),
                    BookingStatus = BookingStatus.Reserved,
                    DepositAmount = 1000000
                };
                context.Bookings.Add(booking);
                
                context.Tours.Add(new Tour { TourId = 1, TourName = "Island Hopping", Price = 200000 });
                context.TourSchedules.Add(new TourSchedule { ScheduleId = 5, TourId = 1, AvailableSlots = 8 });
                
                context.TourBookings.Add(new TourBooking
                {
                    TourBookingId = 1,
                    BookingId = 1,
                    ScheduleId = 5,
                    Participants = 2,
                    TotalAmount = 400000,
                    BookingStatus = TourBookingStatus.Pending,
                    UserId = 1
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
                var schedule = await context.TourSchedules.FindAsync(5);
                Assert.Equal(10, schedule.AvailableSlots); // 8 + 2 = 10
            }
        }
    }
}
