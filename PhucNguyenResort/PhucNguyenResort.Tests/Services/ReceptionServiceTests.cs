using Microsoft.EntityFrameworkCore;
using Moq;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.ViewModels.Bookings;
using PhucNguyenResort.Services.Email;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhucNguyenResort.Tests.Services
{
    public class ReceptionServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task REC_TC_001_ProcessCheckInAsync_ValidBooking_UpdatesBookingAndRoomStatus()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int testBookingId = 1;
            int testRoomId = 101;

            using (var context = new ResortDbContext(options))
            {
                context.Rooms.Add(new Room { RoomId = testRoomId, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                var user = new User { UserId = 1, FullName = "Test User", Email = "test@example.com" };
                context.Users.Add(user);
                var booking = new Booking
                {
                    UserId = 1,
                    User = user,
                    BookingId = testBookingId,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { BookingDetailId = 1, RoomCategoryId = 1, RoomId = null, BookingId = testBookingId });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                // Setup mock for SaveBookingGuestsAsync to return success
                mockGuestService
                    .Setup(s => s.SaveBookingGuestsAsync(testBookingId, It.IsAny<List<BookingGuestDto>>(), It.IsAny<bool>()))
                    .ReturnsAsync(true);

                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>
                    {
                        new BookingGuestDto { FullName = "Nguyen Van A", PassportNumber = "0123456789" }
                    },
                    RoomAssignments = new List<BookingDetailAssignmentViewModel>
                    {
                        new BookingDetailAssignmentViewModel
                        {
                            BookingDetailId = 1,
                            RoomCategoryId = 1,
                            SelectedRoomId = testRoomId
                        }
                    }
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.True(result.Success);
                Assert.Empty(result.ErrorMessage);

                var updatedBooking = await context.Bookings.Include(b => b.BookingDetails).FirstOrDefaultAsync(b => b.BookingId == testBookingId);
                Assert.NotNull(updatedBooking);
                Assert.Equal(BookingStatus.CheckedIn, updatedBooking.BookingStatus);
                Assert.Equal(testRoomId, updatedBooking.BookingDetails.First().RoomId);

                var updatedRoom = await context.Rooms.FindAsync(testRoomId);
                Assert.NotNull(updatedRoom);
                Assert.Equal(RoomStatus.Occupied, updatedRoom.Status);
            }
        }

        [Fact]
        public async Task REC_TC_002_ProcessCheckInAsync_InvalidStatus_ReturnsFalse()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int testBookingId = 2;

            using (var context = new ResortDbContext(options))
            {
                var user = new User { UserId = 1, FullName = "Test User", Email = "test@example.com" };
                context.Users.Add(user);
                context.Bookings.Add(new Booking
                {
                    UserId = 1,
                    User = user,
                    BookingId = testBookingId,
                    BookingStatus = BookingStatus.Cancelled // Invalid status for check-in
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>()
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Đơn đặt phòng này không ở trạng thái có thể Check-in hoặc giao phòng.", result.ErrorMessage);

                var unchangedBooking = await context.Bookings.FindAsync(testBookingId);
                Assert.NotNull(unchangedBooking);
                Assert.Equal(BookingStatus.Cancelled, unchangedBooking.BookingStatus); // Status should not change
            }
        }
        
        [Fact]
        public async Task REC_TC_003_ProcessCheckInAsync_BookingNotFound_ReturnsFalse()
        {
            // Arrange
            var options = CreateNewContextOptions();
            
            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = 999, // Does not exist
                    Guests = new List<BookingGuestDto>()
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Không tìm thấy thông tin đặt phòng.", result.ErrorMessage);
            }
        }

        [Fact]
        public async Task REC_TC_004_GetAvailableRoomsForCategory_ExcludesMaintenanceAndOccupiedRooms()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var checkIn = new DateTime(2026, 6, 20);
            var checkOut = new DateTime(2026, 6, 25);

            using (var context = new ResortDbContext(options))
            {
                // Room 101: Available, same category
                context.Rooms.Add(new Room { RoomId = 101, RoomNumber = "101", CategoryId = 1, Status = RoomStatus.Available });
                
                // Room 102: Maintenance, same category -> Excluded
                context.Rooms.Add(new Room { RoomId = 102, RoomNumber = "102", CategoryId = 1, Status = RoomStatus.Maintenance });

                // Room 103: Available, but occupied in overlapping dates -> Excluded
                context.Rooms.Add(new Room { RoomId = 103, RoomNumber = "103", CategoryId = 1, Status = RoomStatus.Available });
                var overlappingBooking = new Booking
                {
                    BookingId = 10,
                    BookingStatus = BookingStatus.Reserved,
                    CheckInDate = new DateTime(2026, 6, 22),
                    CheckOutDate = new DateTime(2026, 6, 28)
                };
                overlappingBooking.BookingDetails.Add(new BookingDetail { BookingDetailId = 100, RoomCategoryId = 1, RoomId = 103, BookingId = 10 });
                context.Bookings.Add(overlappingBooking);

                // Room 104: Available, assigned to a booking that does not overlap -> Included
                context.Rooms.Add(new Room { RoomId = 104, RoomNumber = "104", CategoryId = 1, Status = RoomStatus.Available });
                var nonOverlappingBooking = new Booking
                {
                    BookingId = 11,
                    BookingStatus = BookingStatus.Reserved,
                    CheckInDate = new DateTime(2026, 6, 15),
                    CheckOutDate = new DateTime(2026, 6, 20) // ends on check-in day (no overlap)
                };
                nonOverlappingBooking.BookingDetails.Add(new BookingDetail { BookingDetailId = 101, RoomCategoryId = 1, RoomId = 104, BookingId = 11 });
                context.Bookings.Add(nonOverlappingBooking);

                // Room 201: Different category -> Excluded
                context.Rooms.Add(new Room { RoomId = 201, RoomNumber = "201", CategoryId = 2, Status = RoomStatus.Available });

                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                // Act
                var availableRooms = await service.GetAvailableRoomsForCategoryAsync(1, checkIn, checkOut);

                // Assert
                Assert.NotNull(availableRooms);
                // Should contain 101 and 104. Not 102 (maintenance), not 103 (overlapping booking), not 201 (wrong category).
                Assert.Equal(2, availableRooms.Count);
                Assert.Contains(availableRooms, r => r.RoomId == 101);
                Assert.Contains(availableRooms, r => r.RoomId == 104);
            }
        }

        [Fact]
        public async Task REC_TC_005_ProcessCheckInAsync_MissingRoomAssignment_ReturnsFalse()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int testBookingId = 3;

            using (var context = new ResortDbContext(options))
            {
                var user = new User { UserId = 1, FullName = "Test User", Email = "test@example.com" };
                context.Users.Add(user);
                var booking = new Booking
                {
                    UserId = 1,
                    User = user,
                    BookingId = testBookingId,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { BookingDetailId = 301, RoomCategoryId = 1, BookingId = testBookingId });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>(),
                    RoomAssignments = new List<BookingDetailAssignmentViewModel>
                    {
                        new BookingDetailAssignmentViewModel
                        {
                            BookingDetailId = 301,
                            RoomCategoryId = 1,
                            SelectedRoomId = null // Missing assignment
                        }
                    }
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Vui lòng gán phòng vật lý cho tất cả các chi tiết đặt phòng.", result.ErrorMessage);
            }
        }
        // fix bug 

        [Fact]
        public async Task REC_TC_006_ProcessCheckInAsync_EmptyRoomAssignments_ReturnsFalse()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int testBookingId = 4;

            using (var context = new ResortDbContext(options))
            {
                var user = new User { UserId = 1, FullName = "Test User", Email = "test@example.com" };
                context.Users.Add(user);
                var booking = new Booking
                {
                    UserId = 1,
                    User = user,
                    BookingId = testBookingId,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { BookingDetailId = 401, RoomCategoryId = 1, BookingId = testBookingId });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var mockEmailSender = new Mock<IEmailSender>();
                var mockEnv = new Mock<IWebHostEnvironment>();
                var service = new ReceptionService(context, mockGuestService.Object, mockEmailSender.Object, mockEnv.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>(),
                    RoomAssignments = new List<BookingDetailAssignmentViewModel>() // Empty assignments list
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Danh sách gán phòng trống.", result.ErrorMessage);
            }
        }
    }
}
