using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.ViewModels.Bookings;

namespace PhucNguyenResort.Tests.Services
{
    public class ReceptionServiceCheckOutTests
    {
        private ResortDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ResortDbContext(options);
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_WhenBookingNotCheckedIn_ReturnsNull()
        {
            // Arrange
            var context = GetDbContext();
            var service = new ReceptionService(context, null!);
            var user = new User { UserId = 2, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 2)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 2,
                User = user,
                BookingId = 2,
                BookingStatus = BookingStatus.Reserved,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1)
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_WhenNoGuestFolio_ReturnsNull()
        {
            // Arrange
            var context = GetDbContext();
            var service = new ReceptionService(context, null!);
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1)
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.Null(result);
        }
        
        // Surcharge tests using a subclass to mock DateTime.Now
        private class TestableReceptionService : ReceptionService
        {
            private readonly DateTime _mockNow;
            public TestableReceptionService(ResortDbContext context, DateTime mockNow) : base(context, null!)
            {
                _mockNow = mockNow;
            }

            protected override DateTime GetCurrentDateTime()
            {
                return _mockNow;
            }
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_LateCheckout_1400_Adds30PercentSurcharge()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 14, 0, 0); // 14:00 PM
            var service = new TestableReceptionService(context, mockNow);
            
            var roomCategory = new RoomCategory { CategoryId = 1, PricePerNight = 1000000 };
            var room = new Room { RoomId = 1, CategoryId = 1, Category = roomCategory, Status = RoomStatus.Occupied };
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalRoomCharges = 2000000,
                    Status = FolioStatus.Unpaid
                },
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 1, Room = room }
                }
            };
            context.RoomCategories.Add(roomCategory);
            context.Rooms.Add(room);
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(300000, result.LateCheckOutSurcharge); // 30% of 1M
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_LateCheckout_1630_Adds50PercentSurcharge()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 16, 30, 0); // 16:30 PM
            var service = new TestableReceptionService(context, mockNow);
            
            var roomCategory = new RoomCategory { CategoryId = 1, PricePerNight = 1000000 };
            var room = new Room { RoomId = 1, CategoryId = 1, Category = roomCategory, Status = RoomStatus.Occupied };
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalRoomCharges = 2000000,
                    Status = FolioStatus.Unpaid
                },
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 1, Room = room }
                }
            };
            context.RoomCategories.Add(roomCategory);
            context.Rooms.Add(room);
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500000, result.LateCheckOutSurcharge); // 50% of 1M
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_LateCheckout_1900_Adds100PercentSurcharge()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 19, 0, 0); // 19:00 PM
            var service = new TestableReceptionService(context, mockNow);
            
            var roomCategory = new RoomCategory { CategoryId = 1, PricePerNight = 1000000 };
            var room = new Room { RoomId = 1, CategoryId = 1, Category = roomCategory, Status = RoomStatus.Occupied };
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalRoomCharges = 2000000,
                    Status = FolioStatus.Unpaid
                },
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 1, Room = room }
                }
            };
            context.RoomCategories.Add(roomCategory);
            context.Rooms.Add(room);
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000000, result.LateCheckOutSurcharge); // 100% of 1M
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_Checkout_1130_NoSurcharge()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // 11:30 AM
            var service = new TestableReceptionService(context, mockNow);
            
            var roomCategory = new RoomCategory { CategoryId = 1, PricePerNight = 1000000 };
            var room = new Room { RoomId = 1, CategoryId = 1, Category = roomCategory, Status = RoomStatus.Occupied };
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalRoomCharges = 2000000,
                    Status = FolioStatus.Unpaid
                },
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 1, Room = room }
                }
            };
            context.RoomCategories.Add(roomCategory);
            context.Rooms.Add(room);
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.LateCheckOutSurcharge);
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_BalanceDue_CalculatedCorrectly_TotalGreaterThanDeposit()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // no surcharge
            var service = new TestableReceptionService(context, mockNow);
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                DepositAmount = 500000,
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalAmount = 2800000,
                    Status = FolioStatus.Unpaid
                }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2300000, result.BalanceDue); // 2.8M - 0.5M
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_BalanceDue_ZeroWhenDepositGreaterOrEqual()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // no surcharge
            var service = new TestableReceptionService(context, mockNow);
            
            var user = new User { UserId = 3, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 3)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 3,
                User = user,
                BookingId = 3,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                DepositAmount = 5000000,
                GuestFolio = new GuestFolio
                {
                    BookingId = 3,
                    FolioId = 3,
                    TotalAmount = 2000000,
                    Status = FolioStatus.Unpaid
                }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.BalanceDue);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_Success_CreatesInvoiceWithPaidStatus()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0);
            var service = new TestableReceptionService(context, mockNow);
            
            var guestFolio = new GuestFolio
            {
                FolioId = 1,
                TotalAmount = 2000000,
                Status = FolioStatus.Unpaid
            };
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                DepositAmount = 500000,
                GuestFolio = guestFolio
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                SelectedPaymentMethod = PaymentMethod.Cash,
                TotalAmount = 2000000,
                DepositAmount = 500000,
                BalanceDue = 1500000,
                FolioId = 1
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.True(result.Success);
            var invoice = await context.Invoices.FirstOrDefaultAsync(i => i.FolioId == 1);
            Assert.NotNull(invoice);
            Assert.Equal(InvoiceStatus.Paid, invoice.InvoiceStatus);
            Assert.Equal(2000000, invoice.FinalAmount);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_Success_CreatesPayment()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0);
            var service = new TestableReceptionService(context, mockNow);
            
            var guestFolio = new GuestFolio
            {
                FolioId = 1,
                TotalAmount = 2000000,
                Status = FolioStatus.Unpaid
            };
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                DepositAmount = 500000,
                GuestFolio = guestFolio
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                SelectedPaymentMethod = PaymentMethod.Cash,
                TotalAmount = 2000000,
                DepositAmount = 500000,
                BalanceDue = 1500000,
                FolioId = 1
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.True(result.Success);
            var invoice = await context.Invoices.FirstOrDefaultAsync(i => i.FolioId == 1);
            var payment = await context.Payments.FirstOrDefaultAsync(p => p.InvoiceId == invoice.InvoiceId);
            
            Assert.NotNull(payment);
            Assert.Equal(PaymentMethod.Cash, payment.PaymentMethod);
            Assert.Equal(1500000, payment.Amount);
            Assert.Equal(PaymentStatus.Success, payment.PaymentStatus);
            Assert.NotNull(payment.PaidAt);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_Success_ChangesBookingStatusAndCheckOutTime()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0);
            var service = new TestableReceptionService(context, mockNow);
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1, TotalAmount = 1000, Status = FolioStatus.Unpaid }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                SelectedPaymentMethod = PaymentMethod.CreditCard,
                BalanceDue = 1000,
                FolioId = 1
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.True(result.Success);
            var updatedBooking = await context.Bookings.FindAsync(1);
            Assert.Equal(BookingStatus.CheckedOut, updatedBooking.BookingStatus);
            Assert.NotNull(updatedBooking.ActualCheckOutTime);
            Assert.Equal(mockNow, updatedBooking.ActualCheckOutTime);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_Success_ChangesAllRoomsToNeedsCleaning()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0);
            var service = new TestableReceptionService(context, mockNow);
            
            var room1 = new Room { RoomId = 101, Status = RoomStatus.Occupied };
            var room2 = new Room { RoomId = 102, Status = RoomStatus.Occupied };
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1, TotalAmount = 1000, Status = FolioStatus.Unpaid },
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 101, Room = room1 },
                    new BookingDetail { RoomId = 102, Room = room2 }
                }
            };
            context.Rooms.AddRange(room1, room2);
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                SelectedPaymentMethod = PaymentMethod.BankTransfer,
                BalanceDue = 1000,
                FolioId = 1
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.True(result.Success);
            var updatedRoom1 = await context.Rooms.FindAsync(101);
            var updatedRoom2 = await context.Rooms.FindAsync(102);
            Assert.Equal(RoomStatus.NeedsCleaning, updatedRoom1.Status);
            Assert.Equal(RoomStatus.NeedsCleaning, updatedRoom2.Status);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_Success_ChangesFolioStatusToPaid()
        {
             // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0);
            var service = new TestableReceptionService(context, mockNow);
            
            var guestFolio = new GuestFolio { FolioId = 1, TotalAmount = 1000, Status = FolioStatus.Unpaid };
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                GuestFolio = guestFolio
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                SelectedPaymentMethod = PaymentMethod.VNPay,
                BalanceDue = 1000,
                FolioId = 1
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.True(result.Success);
            var updatedFolio = await context.GuestFolios.FindAsync(1);
            Assert.Equal(FolioStatus.Paid, updatedFolio.Status);
        }

        [Fact]
        public async Task ProcessCheckOutAsync_MissingPaymentMethod_ReturnsFalse()
        {
            // Arrange
            var context = GetDbContext();
            var service = new ReceptionService(context, null!);
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                GuestFolio = new GuestFolio { FolioId = 1, BookingId = 1, Status = FolioStatus.Unpaid }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            var model = new CheckOutViewModel
            {
                BookingId = 1,
                FolioId = 1,
                SelectedPaymentMethod = null // Missing payment method
            };

            // Act
            var result = await service.ProcessCheckOutAsync(model);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("phương thức thanh toán", result.ErrorMessage);
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_AggregatesFoodChargesCorrectly()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // no surcharge
            var service = new TestableReceptionService(context, mockNow);
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalFoodCharges = 500000,
                    Status = FolioStatus.Unpaid,
                    FolioTransactions = new List<FolioTransaction>
                    {
                        new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 300000 },
                        new FolioTransaction { TransactionType = FolioTransactionType.FoodCharge, Amount = 200000 }
                    }
                }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500000, result.TotalFoodCharges);
        }

        [Fact]
        public async Task PrepareCheckOutViewModelAsync_AggregatesTourChargesCorrectly()
        {
            // Arrange
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // no surcharge
            var service = new TestableReceptionService(context, mockNow);
            
            var user = new User { UserId = 1, FullName = "Test User" };
            if (!context.Users.Any(u => u.UserId == 1)) context.Users.Add(user);

            var booking = new Booking
            {
                UserId = 1,
                User = user,
                BookingId = 1,
                BookingStatus = BookingStatus.CheckedIn,
                CheckOutDate = new DateTime(2026, 7, 1),
                GuestFolio = new GuestFolio
                {
                    BookingId = 1,
                    FolioId = 1,
                    TotalTourCharges = 300000,
                    Status = FolioStatus.Unpaid
                }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // Act
            var result = await service.PrepareCheckOutViewModelAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(300000, result.TotalTourCharges);
        }
    }
}

