using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.ViewModels.Bookings;

namespace PhucNguyenResort.Tests.Integration
{
    public class CheckOutIntegrationTests
    {
        private ResortDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ResortDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ResortDbContext(options);
        }

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
        public async Task FullCheckOutFlow_EndToEnd()
        {
            // 1. Arrange - Seed data
            var context = GetDbContext();
            var mockNow = new DateTime(2026, 7, 1, 11, 30, 0); // Before 12:00, no late surcharge
            var service = new TestableReceptionService(context, mockNow);

            var room1 = new Room { RoomId = 101, RoomNumber = "P101", Status = RoomStatus.Occupied };
            context.Rooms.Add(room1);

            var guestFolio = new GuestFolio
            {
                BookingId = 1,
                FolioId = 1,
                TotalRoomCharges = 2000000,
                TotalFoodCharges = 500000,
                TotalTourCharges = 300000,
                TotalAmount = 2800000,
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
                CheckOutDate = new DateTime(2026, 7, 1),
                DepositAmount = 500000,
                GuestFolio = guestFolio,
                BookingDetails = new List<BookingDetail>
                {
                    new BookingDetail { RoomId = 101, Room = room1 }
                }
            };
            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            // 2. Act - Prepare ViewModel
            var prepareModel = await service.PrepareCheckOutViewModelAsync(1);

            // Assert Prepare
            Assert.NotNull(prepareModel);
            Assert.Equal(2800000, prepareModel.TotalAmount);
            Assert.Equal(500000, prepareModel.DepositAmount);
            Assert.Equal(2300000, prepareModel.BalanceDue);
            Assert.Equal(0, prepareModel.LateCheckOutSurcharge);

            // 3. Act - Process Checkout
            prepareModel.SelectedPaymentMethod = PaymentMethod.Cash;
            var processResult = await service.ProcessCheckOutAsync(prepareModel);

            // 4. Assert Process & DB state
            Assert.True(processResult.Success);

            var dbBooking = await context.Bookings.FindAsync(1);
            Assert.Equal(BookingStatus.CheckedOut, dbBooking.BookingStatus);
            Assert.NotNull(dbBooking.ActualCheckOutTime);

            var dbRoom = await context.Rooms.FindAsync(101);
            Assert.Equal(RoomStatus.NeedsCleaning, dbRoom.Status);

            var dbFolio = await context.GuestFolios.FindAsync(1);
            Assert.Equal(FolioStatus.Paid, dbFolio.Status);

            var invoice = await context.Invoices.FirstOrDefaultAsync(i => i.FolioId == 1);
            Assert.NotNull(invoice);
            Assert.Equal(InvoiceStatus.Paid, invoice.InvoiceStatus);
            Assert.Equal(2800000, invoice.FinalAmount);

            var payment = await context.Payments.FirstOrDefaultAsync(p => p.InvoiceId == invoice.InvoiceId);
            Assert.NotNull(payment);
            Assert.Equal(PaymentMethod.Cash, payment.PaymentMethod);
            Assert.Equal(2300000, payment.Amount);
            Assert.Equal(PaymentStatus.Success, payment.PaymentStatus);
            Assert.NotNull(payment.PaidAt);
        }
    }
}

