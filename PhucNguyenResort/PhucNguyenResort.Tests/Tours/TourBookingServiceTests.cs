using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Tests.Tours
{
    // DTOs & Inputs
    public class CreateTourBookingInput { public int ScheduleId { get; set; } public int Participants { get; set; } public string PaymentMethod { get; set; } = string.Empty; public int? BookingId { get; set; } }
    public class TourBookingDto { public int TourBookingId { get; set; } public TourBookingStatus BookingStatus { get; set; } public string PaymentStatus { get; set; } = string.Empty; public decimal TotalPrice { get; set; } public int AvailableSlots { get; set; } public string PaymentMethod { get; set; } = string.Empty; }
    public class CancelBookingResult { public TourBookingStatus BookingStatus { get; set; } public decimal RefundAmount { get; set; } }
    
    // Exceptions
    public class TourFullException : Exception { public TourFullException(string msg) : base(msg) { } }
    public class NoActiveRoomBookingException : Exception { public NoActiveRoomBookingException(string msg) : base(msg) { } }

    public interface ITourScheduleRepository { Task<TourSchedule> FindByIdForUpdate(int id); Task Update(TourSchedule entity); }
    public interface ITourBookingRepository { Task<TourBooking> Save(TourBooking entity); Task<List<TourBooking>> FindUnpaidByBookingId(int bookingId); Task<TourBooking> FindById(int id); Task Update(TourBooking entity); }
    public interface IPaymentService { Task<bool> ProcessOnlinePayment(decimal amount); }
    public interface INotificationService { Task SendBookingConfirmation(int userId, TourBooking booking); }

    public class TourBookingService
    {
        private readonly ITourScheduleRepository _scheduleRepo;
        private readonly ITourBookingRepository _bookingRepo;
        private readonly IPaymentService _paymentService;

        public TourBookingService(ITourScheduleRepository scheduleRepo, ITourBookingRepository bookingRepo, IPaymentService paymentService)
        {
            _scheduleRepo = scheduleRepo;
            _bookingRepo = bookingRepo;
            _paymentService = paymentService;
        }

        public async Task<TourBookingDto> CreateBooking(CreateTourBookingInput input)
        {
            var schedule = await _scheduleRepo.FindByIdForUpdate(input.ScheduleId);
            if (schedule == null) throw new Exception("Schedule not found");

            if (schedule.AvailableSlots < input.Participants)
                throw new TourFullException("TOUR-002: Tour is fully booked");

            if (input.PaymentMethod == "ChargeToRoom" && !input.BookingId.HasValue)
                throw new NoActiveRoomBookingException("TOUR-006: ChargeToRoom requires an active room booking");

            decimal price = schedule.Tour?.Price ?? 0;
            decimal totalAmount = price * input.Participants;

            var booking = new TourBooking
            {
                ScheduleId = input.ScheduleId,
                Participants = input.Participants,
                TotalAmount = totalAmount,
                BookingId = input.BookingId,
                BookingStatus = TourBookingStatus.Pending
            };

            if (input.PaymentMethod == "Online")
            {
                var paymentSuccess = await _paymentService.ProcessOnlinePayment(totalAmount);
                if (paymentSuccess)
                {
                    booking.BookingStatus = TourBookingStatus.Confirmed;
                }
            }

            schedule.AvailableSlots -= input.Participants;
            await _scheduleRepo.Update(schedule);
            
            var saved = await _bookingRepo.Save(booking);

            return new TourBookingDto
            {
                TourBookingId = saved.TourBookingId,
                BookingStatus = saved.BookingStatus,
                TotalPrice = saved.TotalAmount ?? 0,
                AvailableSlots = schedule.AvailableSlots ?? 0,
                PaymentMethod = input.PaymentMethod,
                PaymentStatus = input.PaymentMethod == "Online" ? "Paid" : "Pending"
            };
        }

        public async Task<CancelBookingResult> CancelBooking(int tourBookingId, int userId)
        {
            var booking = await _bookingRepo.FindById(tourBookingId);
            if (booking == null) throw new Exception("Booking not found");

            booking.BookingStatus = TourBookingStatus.Cancelled;
            decimal refund = 0;

            if (booking.Schedule?.TourDate != null)
            {
                var timeUntilDeparture = booking.Schedule.TourDate.Value - DateTime.Now;
                if (timeUntilDeparture.TotalHours > 48)
                {
                    refund = booking.TotalAmount ?? 0;
                }
            }

            await _bookingRepo.Update(booking);

            return new CancelBookingResult
            {
                BookingStatus = booking.BookingStatus,
                RefundAmount = refund
            };
        }

        public async Task<List<TourBookingDto>> GetUnpaidChargeToRoomBookings(int bookingId)
        {
            var unpaid = await _bookingRepo.FindUnpaidByBookingId(bookingId);
            return unpaid.Select(b => new TourBookingDto
            {
                TourBookingId = b.TourBookingId,
                BookingStatus = b.BookingStatus,
                TotalPrice = b.TotalAmount ?? 0,
                PaymentMethod = "ChargeToRoom",
                PaymentStatus = "Pending"
            }).ToList();
        }
    }

    public class TourBookingServiceTests
    {
        private readonly Mock<ITourScheduleRepository> _mockScheduleRepo;
        private readonly Mock<ITourBookingRepository> _mockBookingRepo;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly TourBookingService _service;

        public TourBookingServiceTests()
        {
            _mockScheduleRepo = new Mock<ITourScheduleRepository>();
            _mockBookingRepo = new Mock<ITourBookingRepository>();
            _mockPaymentService = new Mock<IPaymentService>();
            _service = new TourBookingService(_mockScheduleRepo.Object, _mockBookingRepo.Object, _mockPaymentService.Object);
        }

        [Fact]
        public async Task TOUR_TC_003_CreateBooking_OnlinePayment_Success()
        {
            var schedule = new TourSchedule { ScheduleId = 5, AvailableSlots = 15, Tour = new Tour { Price = 1500000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(5)).ReturnsAsync(schedule);
            _mockPaymentService.Setup(p => p.ProcessOnlinePayment(It.IsAny<decimal>())).ReturnsAsync(true);
            _mockBookingRepo.Setup(r => r.Save(It.IsAny<TourBooking>())).ReturnsAsync((TourBooking b) => { b.TourBookingId = 15; return b; });

            var input = new CreateTourBookingInput { ScheduleId = 5, Participants = 2, PaymentMethod = "Online" };
            
            var result = await _service.CreateBooking(input);
            
            Assert.NotNull(result);
            Assert.Equal(TourBookingStatus.Confirmed, result.BookingStatus);
            Assert.Equal("Paid", result.PaymentStatus);
            Assert.Equal(3000000, result.TotalPrice);
            Assert.Equal(13, result.AvailableSlots);
        }

        [Fact]
        public async Task TOUR_TC_004_CreateBooking_ChargeToRoom_Success()
        {
            var schedule = new TourSchedule { ScheduleId = 5, AvailableSlots = 15, Tour = new Tour { Price = 1500000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(5)).ReturnsAsync(schedule);
            _mockBookingRepo.Setup(r => r.Save(It.IsAny<TourBooking>())).ReturnsAsync((TourBooking b) => { b.TourBookingId = 16; return b; });

            var input = new CreateTourBookingInput { ScheduleId = 5, Participants = 3, PaymentMethod = "ChargeToRoom", BookingId = 102 };
            
            var result = await _service.CreateBooking(input);
            
            Assert.NotNull(result);
            Assert.Equal(TourBookingStatus.Pending, result.BookingStatus);
            Assert.Equal("Pending", result.PaymentStatus);
            Assert.Equal(12, result.AvailableSlots);
        }

        [Fact]
        public async Task TOUR_TC_005_CreateBooking_Overbooking_ThrowsException()
        {
            var schedule = new TourSchedule { ScheduleId = 6, AvailableSlots = 0, Tour = new Tour { Price = 100000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(6)).ReturnsAsync(schedule);

            var input = new CreateTourBookingInput { ScheduleId = 6, Participants = 1, PaymentMethod = "Online" };
            
            var ex = await Assert.ThrowsAsync<TourFullException>(() => _service.CreateBooking(input));
            Assert.Contains("TOUR-002", ex.Message);
        }

        [Fact]
        public async Task TOUR_TC_006_CreateBooking_ChargeToRoomWithoutBooking_ThrowsException()
        {
            var schedule = new TourSchedule { ScheduleId = 5, AvailableSlots = 15, Tour = new Tour { Price = 100000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(5)).ReturnsAsync(schedule);

            var input = new CreateTourBookingInput { ScheduleId = 5, Participants = 1, PaymentMethod = "ChargeToRoom", BookingId = null };
            
            var ex = await Assert.ThrowsAsync<NoActiveRoomBookingException>(() => _service.CreateBooking(input));
            Assert.Contains("TOUR-006", ex.Message);
        }

        [Fact]
        public async Task TOUR_TC_013_GetUnpaidChargeToRoomBookings()
        {
            _mockBookingRepo.Setup(r => r.FindUnpaidByBookingId(102)).ReturnsAsync(new List<TourBooking> { 
                new TourBooking { TourBookingId = 1, TotalAmount = 500 }, 
                new TourBooking { TourBookingId = 2, TotalAmount = 1000 } 
            });

            var result = await _service.GetUnpaidChargeToRoomBookings(102);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1500, result.Sum(x => x.TotalPrice));
        }

        [Fact]
        public async Task TOUR_TC_014_TotalPriceCalculation()
        {
            var schedule = new TourSchedule { ScheduleId = 5, AvailableSlots = 15, Tour = new Tour { Price = 1500000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(5)).ReturnsAsync(schedule);
            _mockPaymentService.Setup(p => p.ProcessOnlinePayment(It.IsAny<decimal>())).ReturnsAsync(true);
            _mockBookingRepo.Setup(r => r.Save(It.IsAny<TourBooking>())).ReturnsAsync((TourBooking b) => b);

            var input = new CreateTourBookingInput { ScheduleId = 5, Participants = 3, PaymentMethod = "Online" };
            var result = await _service.CreateBooking(input);
            
            Assert.Equal(4500000, result.TotalPrice);
        }

        [Fact]
        public async Task TOUR_TC_015_AvailableSlotsInvariant()
        {
            var schedule = new TourSchedule { ScheduleId = 5, AvailableSlots = 15, Tour = new Tour { Price = 1500000 } };
            _mockScheduleRepo.Setup(r => r.FindByIdForUpdate(5)).ReturnsAsync(schedule);
            _mockPaymentService.Setup(p => p.ProcessOnlinePayment(It.IsAny<decimal>())).ReturnsAsync(true);
            _mockBookingRepo.Setup(r => r.Save(It.IsAny<TourBooking>())).ReturnsAsync((TourBooking b) => b);

            var input = new CreateTourBookingInput { ScheduleId = 5, Participants = 3, PaymentMethod = "Online" };
            var result = await _service.CreateBooking(input);
            
            Assert.Equal(12, result.AvailableSlots);
            _mockScheduleRepo.Verify(r => r.Update(It.Is<TourSchedule>(s => s.AvailableSlots == 12)), Times.Once);
        }

        [Fact]
        public async Task TOUR_TC_007_CancelBooking_GreaterThan48h_Refunds100()
        {
            var booking = new TourBooking { TourBookingId = 16, TotalAmount = 3000000, BookingStatus = TourBookingStatus.Confirmed, Schedule = new TourSchedule { TourDate = DateTime.Now.AddDays(5) } };
            _mockBookingRepo.Setup(r => r.FindById(16)).ReturnsAsync(booking);

            var result = await _service.CancelBooking(16, 10);
            
            Assert.Equal(TourBookingStatus.Cancelled, result.BookingStatus);
            Assert.Equal(3000000, result.RefundAmount);
        }

        [Fact]
        public async Task TOUR_TC_008_CancelBooking_LessThan48h_NoRefund()
        {
            var booking = new TourBooking { TourBookingId = 16, TotalAmount = 1500000, BookingStatus = TourBookingStatus.Confirmed, Schedule = new TourSchedule { TourDate = DateTime.Now.AddDays(1) } };
            _mockBookingRepo.Setup(r => r.FindById(16)).ReturnsAsync(booking);

            var result = await _service.CancelBooking(16, 10);
            
            Assert.Equal(TourBookingStatus.Cancelled, result.BookingStatus);
            Assert.Equal(0, result.RefundAmount);
        }
    }
}
