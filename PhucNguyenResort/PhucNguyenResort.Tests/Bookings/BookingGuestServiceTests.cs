using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Moq;
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
    public class BookingGuestServiceTests
    {
        private DbContextOptions<ResortDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Mock<IDataProtectionProvider> CreateMockDataProtectionProvider()
        {
            var mockProvider = new Mock<IDataProtectionProvider>();
            var mockProtector = new Mock<IDataProtector>();

            // Mock Protect/Unprotect by applying a simple XOR encryption so it changes the data but is reversible
            mockProtector.Setup(p => p.Protect(It.IsAny<byte[]>()))
                .Returns((byte[] data) => data.Select(b => (byte)(b ^ 11)).ToArray());

            mockProtector.Setup(p => p.Unprotect(It.IsAny<byte[]>()))
                .Returns((byte[] data) => data.Select(b => (byte)(b ^ 11)).ToArray());

            mockProvider.Setup(p => p.CreateProtector(It.IsAny<string>()))
                .Returns(mockProtector.Object);

            return mockProvider;
        }

        [Fact]
        public async Task REG_TC_001_SaveBookingGuests_Success_WithPdpdConsent()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockProvider = CreateMockDataProtectionProvider();

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Deluxe", MaxAdults = 2, MaxChildren = 1 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1 });
                var booking = new Booking
                {
                    BookingId = 15,
                    UserId = 1,
                    NumberOfAdults = 2,
                    NumberOfChildren = 1,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, BookingId = 15 });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            var guests = new List<BookingGuestDto>
            {
                new BookingGuestDto { FullName = "Nguyen Van A", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderType.Male, PassportNumber = "012345678901", Nationality = "Vietnam", IsPrimaryBooker = true },
                new BookingGuestDto { FullName = "Nguyen Thi B", DateOfBirth = new DateTime(1995, 10, 20), Gender = GenderType.Female, PassportNumber = "012345678902", Nationality = "Vietnam", IsPrimaryBooker = false },
                new BookingGuestDto { FullName = "Nguyen An C", DateOfBirth = new DateTime(2020, 12, 12), Gender = GenderType.Male, PassportNumber = "", Nationality = "Vietnam", IsPrimaryBooker = false }
            };

            // Act
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingGuestService(context, mockProvider.Object);
                var result = await service.SaveBookingGuestsAsync(15, guests, true);

                // Assert
                Assert.True(result);

                var savedGuests = await context.BookingGuests.Where(bg => bg.BookingId == 15).ToListAsync();
                Assert.Equal(3, savedGuests.Count);

                var booking = await context.Bookings.FindAsync(15);
                Assert.True(booking.PdpdConsentChecked);
                Assert.NotNull(booking.ConsentTimestamp);

                var primaryGuest = savedGuests.First(g => g.IsPrimaryBooker);
                Assert.Equal("Nguyen Van A", primaryGuest.FullName);
                // Passport number should be stored encrypted (different from plaintext)
                Assert.NotEqual("012345678901", primaryGuest.PassportNumber);
            }
        }

        [Fact]
        public async Task REG_TC_002_SaveBookingGuests_Error_WithoutPdpdConsent_WhenCccdProvided()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockProvider = CreateMockDataProtectionProvider();

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Deluxe", MaxAdults = 2, MaxChildren = 1 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1 });
                var booking = new Booking
                {
                    BookingId = 16,
                    UserId = 1,
                    NumberOfAdults = 2,
                    NumberOfChildren = 1,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, BookingId = 16 });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            var guests = new List<BookingGuestDto>
            {
                new BookingGuestDto { FullName = "Nguyen Van A", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderType.Male, PassportNumber = "012345678901", Nationality = "Vietnam", IsPrimaryBooker = true }
            };

            // Act & Assert
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingGuestService(context, mockProvider.Object);
                
                var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                    service.SaveBookingGuestsAsync(16, guests, false)
                );

                Assert.Contains("REG-001", exception.Message);

                var savedGuests = await context.BookingGuests.Where(bg => bg.BookingId == 16).ToListAsync();
                Assert.Empty(savedGuests);
            }
        }

        [Fact]
        public async Task REG_TC_003_SaveBookingGuests_Error_WhenGuestCountExceedsCapacity()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockProvider = CreateMockDataProtectionProvider();

            using (var context = new ResortDbContext(options))
            {
                context.RoomCategories.Add(new RoomCategory { CategoryId = 1, CategoryName = "Single", MaxAdults = 1, MaxChildren = 0 });
                context.Rooms.Add(new Room { RoomId = 1, CategoryId = 1 });
                var booking = new Booking
                {
                    BookingId = 16,
                    UserId = 1,
                    NumberOfAdults = 1,
                    NumberOfChildren = 0,
                    BookingStatus = BookingStatus.Reserved
                };
                booking.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, BookingId = 16 });
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
            }

            // Booking is for 1 guest, but we try to save 2 guests
            var guests = new List<BookingGuestDto>
            {
                new BookingGuestDto { FullName = "Nguyen Van A", DateOfBirth = new DateTime(1990, 5, 15), Gender = GenderType.Male, PassportNumber = "012345678901", Nationality = "Vietnam", IsPrimaryBooker = true },
                new BookingGuestDto { FullName = "Nguyen Van B", DateOfBirth = new DateTime(1992, 8, 12), Gender = GenderType.Male, PassportNumber = "012345678902", Nationality = "Vietnam", IsPrimaryBooker = false }
            };

            // Act & Assert
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingGuestService(context, mockProvider.Object);
                
                var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                    service.SaveBookingGuestsAsync(16, guests, true)
                );

                Assert.Contains("REG-002", exception.Message);

                var savedGuests = await context.BookingGuests.Where(bg => bg.BookingId == 16).ToListAsync();
                Assert.Empty(savedGuests);
            }
        }

        [Fact]
        public async Task REG_TC_004_GetBookingGuestsForCheckIn_Success_DecryptsPassportNumber()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockProvider = CreateMockDataProtectionProvider();

            // Set up a helper protector to manually encrypt the value for seed data
            var protector = mockProvider.Object.CreateProtector("PhucNguyenResort.BookingGuestService");
            var encryptedPassport = protector.Protect("012345678901");

            using (var context = new ResortDbContext(options))
            {
                context.BookingGuests.Add(new BookingGuest
                {
                    BookingGuestId = 1,
                    BookingId = 15,
                    FullName = "Nguyen Van A",
                    PassportNumber = encryptedPassport,
                    Nationality = "Vietnam",
                    Gender = GenderType.Male,
                    IsPrimaryBooker = true
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingGuestService(context, mockProvider.Object);
                var result = await service.GetBookingGuestsForCheckInAsync(15);

                // Assert
                var guestList = result.ToList();
                Assert.Single(guestList);

                var guest = guestList.First();
                Assert.Equal("Nguyen Van A", guest.FullName);
                // Passport number should be correctly decrypted back to plaintext
                Assert.Equal("012345678901", guest.PassportNumber);
            }
        }

        [Fact]
        public async Task REG_TC_005_PurgeExpiredPiiData_Success_ClearsCccdAndDobForOldBookings()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var mockProvider = CreateMockDataProtectionProvider();

            using (var context = new ResortDbContext(options))
            {
                // Old booking (checked out 8 days ago)
                var booking17 = new Booking
                {
                    BookingId = 17,
                    UserId = 1,
                    CheckInDate = DateTime.Today.AddDays(-10),
                    CheckOutDate = DateTime.Today.AddDays(-8),
                    BookingStatus = BookingStatus.CheckedOut
                };
                booking17.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, BookingId = 17 });
                context.Bookings.Add(booking17);

                context.BookingGuests.Add(new BookingGuest
                {
                    BookingGuestId = 10,
                    BookingId = 17,
                    FullName = "Nguyen Van A",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    PassportNumber = "EncryptedCCCD1",
                    Nationality = "Vietnam",
                    IsPrimaryBooker = true
                });

                // Recent booking (checked out 3 days ago)
                var booking18 = new Booking
                {
                    BookingId = 18,
                    UserId = 2,
                    CheckInDate = DateTime.Today.AddDays(-5),
                    CheckOutDate = DateTime.Today.AddDays(-3),
                    BookingStatus = BookingStatus.CheckedOut
                };
                booking18.BookingDetails.Add(new BookingDetail { RoomCategoryId = 1, BookingId = 18 });
                context.Bookings.Add(booking18);

                context.BookingGuests.Add(new BookingGuest
                {
                    BookingGuestId = 11,
                    BookingId = 18,
                    FullName = "Nguyen Van B",
                    DateOfBirth = new DateTime(1992, 8, 12),
                    PassportNumber = "EncryptedCCCD2",
                    Nationality = "Vietnam",
                    IsPrimaryBooker = true
                });

                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ResortDbContext(options))
            {
                var service = new BookingGuestService(context, mockProvider.Object);
                var purgedCount = await service.PurgeExpiredPiiDataAsync();

                // Assert
                Assert.Equal(1, purgedCount); // Only booking 17 should be purged

                var guest17 = await context.BookingGuests.FindAsync(10);
                Assert.Null(guest17.PassportNumber);
                Assert.Null(guest17.DateOfBirth);
                Assert.Equal("Nguyen Van A", guest17.FullName); // Name remains

                var guest18 = await context.BookingGuests.FindAsync(11);
                Assert.Equal("EncryptedCCCD2", guest18.PassportNumber); // Recent booking remains intact
                Assert.Equal(new DateTime(1992, 8, 12), guest18.DateOfBirth);
            }
        }
    }
}
