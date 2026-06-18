using Microsoft.EntityFrameworkCore;
using Moq;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.ViewModels.Bookings;
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
                context.Rooms.Add(new Room { RoomId = testRoomId, Status = RoomStatus.Available });
                context.Bookings.Add(new Booking
                {
                    BookingId = testBookingId,
                    RoomId = testRoomId,
                    BookingStatus = BookingStatus.Reserved
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                // Setup mock for SaveBookingGuestsAsync to return success
                mockGuestService
                    .Setup(s => s.SaveBookingGuestsAsync(testBookingId, It.IsAny<List<BookingGuestDto>>(), It.IsAny<bool>()))
                    .ReturnsAsync(true);

                var service = new ReceptionService(context, mockGuestService.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>
                    {
                        new BookingGuestDto { FullName = "Nguyen Van A", PassportNumber = "0123456789" }
                    }
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.True(result.Success);
                Assert.Empty(result.ErrorMessage);

                var updatedBooking = await context.Bookings.FindAsync(testBookingId);
                Assert.NotNull(updatedBooking);
                Assert.Equal(BookingStatus.CheckedIn, updatedBooking.BookingStatus);

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
                context.Bookings.Add(new Booking
                {
                    BookingId = testBookingId,
                    BookingStatus = BookingStatus.Cancelled // Invalid status for check-in
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ResortDbContext(options))
            {
                var mockGuestService = new Mock<IBookingGuestService>();
                var service = new ReceptionService(context, mockGuestService.Object);

                var viewModel = new PreCheckinViewModel
                {
                    BookingId = testBookingId,
                    Guests = new List<BookingGuestDto>()
                };

                // Act
                var result = await service.ProcessCheckInAsync(viewModel);

                // Assert
                Assert.False(result.Success);
                Assert.Equal("Đơn đặt phòng này không ở trạng thái có thể Check-in.", result.ErrorMessage);

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
                var service = new ReceptionService(context, mockGuestService.Object);

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
    }
}
