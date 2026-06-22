using Microsoft.EntityFrameworkCore;
using Moq;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.ViewModels.Bookings;
using PhucNguyenResort.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Services
{
    public class WalkInBookingTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task WKB_TC_001_ProcessWalkInBookingAsync_WithExistingCustomer_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int existingUserId = 10;
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.Users.Add(new User { UserId = existingUserId, FullName = "Nguyen Van A", PhoneNumber = "0987654321", Email = "nguyenvala@gmail.com" });
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Nguyen Van A",
                    PhoneNumber = "0987654321",
                    Email = "nguyenvala@gmail.com",
                    CheckInDate = DateTime.Today.AddDays(2),
                    CheckOutDate = DateTime.Today.AddDays(4),
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = false,
                    PaidAmount = 2000000,
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);

                // Verify User count is still 1 (no new user created)
                var usersCount = await context.Users.CountAsync();
                Assert.Equal(1, usersCount);

                var booking = await context.Bookings
                    .Include(b => b.BookingDetails)
                    .FirstOrDefaultAsync(b => b.BookingId == result.BookingId);

                Assert.NotNull(booking);
                Assert.Equal(existingUserId, booking.UserId);
                Assert.Equal(BookingStatus.Reserved, booking.BookingStatus);
                Assert.Equal(2000000, booking.TotalAmount);
                Assert.Equal(2000000, booking.DepositAmount);
                Assert.Equal(roomId, booking.BookingDetails.First().RoomId);

                // Verify room is still Available because check-in is in the future
                var room = await context.Rooms.FindAsync(roomId);
                Assert.Equal(RoomStatus.Available, room.Status);
            }
        }

        [Fact]
        public async Task WKB_TC_002_ProcessWalkInBookingAsync_WithNewCustomer_CreatesUserAndSuccess()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Tran Thi B",
                    PhoneNumber = "0911222333",
                    Email = "tranthib@gmail.com",
                    CheckInDate = DateTime.Today.AddDays(1),
                    CheckOutDate = DateTime.Today.AddDays(2),
                    NumberOfAdults = 2,
                    NumberOfChildren = 0,
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = false,
                    PaidAmount = 1000000,
                    PaymentMethod = "POS",
                    PdpdConsentChecked = true
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);

                // Verify new user is created in DB
                var newUser = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == "0911222333");
                Assert.NotNull(newUser);
                Assert.Equal("Tran Thi B", newUser.FullName);
                Assert.Equal("tranthib@gmail.com", newUser.Email);
                Assert.True(PasswordHelper.VerifyPassword(newUser, newUser.PasswordHash, "123456"));

                var booking = await context.Bookings.FindAsync(result.BookingId);
                Assert.NotNull(booking);
                Assert.Equal(newUser.UserId, booking.UserId);
            }
        }

        [Fact]
        public async Task WKB_TC_003_ProcessWalkInBookingAsync_InvalidPhoneNumber_ReturnsFailure()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Nguyen Van A",
                    PhoneNumber = "abc", // Invalid phone
                    CheckInDate = DateTime.Today.AddDays(1),
                    CheckOutDate = DateTime.Today.AddDays(2),
                    SelectedRoomIds = new List<int> { 101 }
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("WKB-001", result.ErrorCode);
                Assert.Contains("Số điện thoại", result.ErrorMessage);
            }
        }

        [Fact]
        public async Task WKB_TC_004_ProcessWalkInBookingAsync_InstantCheckIn_SavesGuestsAndUpdatesStatuses()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;
            int testBookingId = 1;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                mockGuestService
                    .Setup(s => s.SaveBookingGuestsAsync(It.IsAny<int>(), It.IsAny<List<BookingGuestDto>>(), It.IsAny<bool>()))
                    .ReturnsAsync(true);

                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Le Van C",
                    PhoneNumber = "0955555555",
                    CheckInDate = DateTime.Today,
                    CheckOutDate = DateTime.Today.AddDays(1),
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = true,
                    PaidAmount = 1000000,
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true,
                    Guests = new List<BookingGuestDto>
                    {
                        new BookingGuestDto
                        {
                            FullName = "Le Van C",
                            DateOfBirth = new DateTime(1990, 1, 1),
                            Gender = GenderType.Male,
                            PassportNumber = "001090123456",
                            Nationality = "Vietnam",
                            IsPrimaryBooker = true
                        }
                    }
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.BookingId);

                var booking = await context.Bookings.FindAsync(result.BookingId);
                Assert.Equal(BookingStatus.CheckedIn, booking.BookingStatus);

                var room = await context.Rooms.FindAsync(roomId);
                Assert.Equal(RoomStatus.Occupied, room.Status);

                mockGuestService.Verify(s => s.SaveBookingGuestsAsync(
                    result.BookingId.Value,
                    It.Is<List<BookingGuestDto>>(g => g.First().PassportNumber == "001090123456"),
                    true
                ), Times.Once);
            }
        }

        [Fact]
        public async Task WKB_TC_005_ProcessWalkInBookingAsync_WithCashPayment_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Nguyen Van Cash",
                    PhoneNumber = "0977777777",
                    CheckInDate = DateTime.Today.AddDays(2),
                    CheckOutDate = DateTime.Today.AddDays(3),
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = false,
                    PaidAmount = 500000, // Partial deposit
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.True(result.Success);
                
                var savedBooking = await context.Bookings.FindAsync(result.BookingId);
                Assert.Equal(500000, savedBooking.DepositAmount);
                Assert.Equal(1000000, savedBooking.TotalAmount);
            }
        }

        [Fact]
        public async Task WKB_TC_006_ProcessWalkInBookingAsync_WithPOSPayment_Success()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Nguyen Van Card",
                    PhoneNumber = "0988888888",
                    CheckInDate = DateTime.Today.AddDays(2),
                    CheckOutDate = DateTime.Today.AddDays(3),
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = false,
                    PaidAmount = 1000000,
                    PaymentMethod = "POS",
                    PdpdConsentChecked = true
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.True(result.Success);
                
                var savedBooking = await context.Bookings.FindAsync(result.BookingId);
                Assert.Equal(1000000, savedBooking.DepositAmount);
            }
        }

        [Fact]
        public async Task WKB_TC_007_ProcessWalkInBookingAsync_RoomDoubleBooked_ReturnsFailure()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                
                // Existing overlapping booking
                var existingBooking = new Booking
                {
                    BookingId = 20,
                    UserId = 1,
                    CheckInDate = DateTime.Today.AddDays(1),
                    CheckOutDate = DateTime.Today.AddDays(5),
                    BookingStatus = BookingStatus.Reserved
                };
                existingBooking.BookingDetails.Add(new BookingDetail { RoomId = roomId, RoomCategoryId = 1 });
                context.Bookings.Add(existingBooking);

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Walk-in Guest",
                    PhoneNumber = "0999999999",
                    CheckInDate = DateTime.Today.AddDays(2),
                    CheckOutDate = DateTime.Today.AddDays(4), // Overlaps
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = false
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("WKB-002", result.ErrorCode);
                Assert.Contains("đã được đặt", result.ErrorMessage);
            }
        }

        [Fact]
        public async Task WKB_TC_008_ProcessWalkInBookingAsync_InstantCheckInMissingPassport_ReturnsFailure()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Le Van C",
                    PhoneNumber = "0955555555",
                    CheckInDate = DateTime.Today,
                    CheckOutDate = DateTime.Today.AddDays(1),
                    SelectedRoomIds = new List<int> { roomId },
                    InstantCheckIn = true,
                    PaidAmount = 1000000,
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true,
                    Guests = new List<BookingGuestDto>
                    {
                        new BookingGuestDto
                        {
                            FullName = "Le Van C",
                            PassportNumber = "", // Empty Passport
                            IsPrimaryBooker = true
                        }
                    }
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("WKB-003", result.ErrorCode);
                Assert.Contains("CCCD/Passport", result.ErrorMessage);
            }
        }

        [Fact]
        public async Task WKB_TC_009_ProcessWalkInBookingAsync_PaidAmountLessThan40Percent_ReturnsFailure()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int roomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Standard", PricePerNight = 1000000 });
                context.Rooms.Add(new Room { RoomId = roomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Test Guest",
                    PhoneNumber = "0987654321",
                    CheckInDate = DateTime.Today,
                    CheckOutDate = DateTime.Today.AddDays(1), // 1 night = 1,000,000. 40% = 400,000.
                    SelectedRoomIds = new List<int> { roomId },
                    PaidAmount = 300000, // Less than 400,000
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true,
                    InstantCheckIn = false
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("WKB-005", result.ErrorCode);
                Assert.Contains("tối thiểu 40%", result.ErrorMessage);
            }
        }

        [Fact]
        public async Task WKB_TC_010_ProcessWalkInBookingAsync_NoRoomsSelected_ReturnsFailure()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var model = new WalkInBookingViewModel
                {
                    CustomerName = "Test Guest",
                    PhoneNumber = "0987654321",
                    CheckInDate = DateTime.Today,
                    CheckOutDate = DateTime.Today.AddDays(1),
                    SelectedRoomIds = new List<int>(), // Empty list
                    PaidAmount = 0,
                    PaymentMethod = "Cash",
                    PdpdConsentChecked = true,
                    InstantCheckIn = false
                };

                // Act
                var result = await service.ProcessWalkInBookingAsync(model);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("WKB-006", result.ErrorCode);
                Assert.Contains("chọn ít nhất một phòng", result.ErrorMessage);
            }
        }
    }
}
