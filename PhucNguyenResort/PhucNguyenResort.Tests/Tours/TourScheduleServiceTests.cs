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
    public class CreateScheduleInput { public int TourId { get; set; } public int GuideId { get; set; } public DateTime DepartureDate { get; set; } public TimeSpan DepartureTime { get; set; } }
    public class TourManifestDto { public string GuideName { get; set; } = string.Empty; public string Date { get; set; } = string.Empty; public List<TourManifestItem> Tours { get; set; } = new(); }
    public class TourManifestItem { public string TourName { get; set; } = string.Empty; public string DepartureTime { get; set; } = string.Empty; public int TotalParticipants { get; set; } public List<Attendee> Attendees { get; set; } = new(); }
    public class Attendee { public string GuestName { get; set; } = string.Empty; public string? RoomNumber { get; set; } public int NumberOfParticipants { get; set; } }

    public interface ITourScheduleRepository2
    {
        Task<Tour?> GetTourById(int tourId);
        Task<TourSchedule> Save(TourSchedule entity);
        Task<List<TourSchedule>> GetDailySchedulesForGuide(int guideId, DateTime date);
    }

    public class TourScheduleService
    {
        private readonly ITourScheduleRepository2 _repo;

        public TourScheduleService(ITourScheduleRepository2 repo)
        {
            _repo = repo;
        }

        public async Task<TourSchedule> CreateSchedule(CreateScheduleInput input)
        {
            if (input.DepartureDate.Date < DateTime.Today) throw new ArgumentException("Departure date cannot be in the past");

            var tour = await _repo.GetTourById(input.TourId);
            if (tour == null) throw new Exception("Tour not found");

            var schedule = new TourSchedule
            {
                TourId = input.TourId,
                GuideId = input.GuideId,
                TourDate = input.DepartureDate.Add(input.DepartureTime),
                AvailableSlots = tour.MaxCapacity ?? 0
            };

            return await _repo.Save(schedule);
        }

        public async Task<TourManifestDto> GetDailyManifest(int guideId, DateTime date)
        {
            var schedules = await _repo.GetDailySchedulesForGuide(guideId, date);
            
            var manifest = new TourManifestDto
            {
                GuideName = schedules.FirstOrDefault()?.Guide?.FullName ?? "Unknown Guide",
                Date = date.ToString("yyyy-MM-dd"),
                Tours = schedules.Select(s => new TourManifestItem
                {
                    TourName = s.Tour?.TourName ?? string.Empty,
                    DepartureTime = s.TourDate?.ToString("HH:mm") ?? "",
                    TotalParticipants = s.TourBookings?.Where(tb => tb.BookingStatus != TourBookingStatus.Cancelled).Sum(tb => tb.Participants) ?? 0,
                    Attendees = s.TourBookings?.Where(tb => tb.BookingStatus != TourBookingStatus.Cancelled).Select(tb => new Attendee
                    {
                        GuestName = tb.Booking?.User?.FullName ?? "Unknown Guest",
                        RoomNumber = tb.Booking?.Room?.RoomNumber,
                        NumberOfParticipants = tb.Participants ?? 0
                    }).ToList() ?? new List<Attendee>()
                }).ToList()
            };

            return manifest;
        }
    }

    public class TourScheduleServiceTests
    {
        private readonly Mock<ITourScheduleRepository2> _mockRepo;
        private readonly TourScheduleService _service;

        public TourScheduleServiceTests()
        {
            _mockRepo = new Mock<ITourScheduleRepository2>();
            _service = new TourScheduleService(_mockRepo.Object);
        }

        [Fact]
        public async Task TOUR_TC_010_CreateSchedule_SetsAvailableSlotsToMaxCapacity()
        {
            var tour = new Tour { TourId = 1, MaxCapacity = 30 };
            _mockRepo.Setup(r => r.GetTourById(1)).ReturnsAsync(tour);
            _mockRepo.Setup(r => r.Save(It.IsAny<TourSchedule>())).ReturnsAsync((TourSchedule s) => s);

            var input = new CreateScheduleInput { TourId = 1, GuideId = 8, DepartureDate = DateTime.Today.AddDays(10), DepartureTime = new TimeSpan(7, 0, 0) };
            
            var result = await _service.CreateSchedule(input);
            
            Assert.NotNull(result);
            Assert.Equal(30, result.AvailableSlots);
            Assert.Equal(8, result.GuideId);
        }

        [Fact]
        public async Task TOUR_TC_011_GetDailyManifest_ReturnsCorrectAttendees()
        {
            var date = new DateTime(2026, 7, 1);
            var schedules = new List<TourSchedule>
            {
                new TourSchedule
                {
                    ScheduleId = 5,
                    TourDate = date.AddHours(7),
                    Guide = new User { FullName = "Guide 8" },
                    Tour = new Tour { TourName = "Morning Tour" },
                    TourBookings = new List<TourBooking>
                    {
                        new TourBooking { BookingStatus = TourBookingStatus.Confirmed, Participants = 2, Booking = new Booking { User = new User { FullName = "Guest 1" } } },
                        new TourBooking { BookingStatus = TourBookingStatus.Cancelled, Participants = 1, Booking = new Booking { User = new User { FullName = "Guest 2" } } }
                    }
                }
            };
            
            _mockRepo.Setup(r => r.GetDailySchedulesForGuide(8, date)).ReturnsAsync(schedules);

            var result = await _service.GetDailyManifest(8, date);
            
            Assert.NotNull(result);
            Assert.Equal("Guide 8", result.GuideName);
            Assert.Single(result.Tours);
            Assert.Equal(2, result.Tours[0].TotalParticipants);
            Assert.Single(result.Tours[0].Attendees);
            Assert.Equal("Guest 1", result.Tours[0].Attendees[0].GuestName);
        }
    }
}
