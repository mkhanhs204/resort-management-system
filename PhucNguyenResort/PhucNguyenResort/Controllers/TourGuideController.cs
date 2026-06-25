using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PhucNguyenResort.Data;

namespace PhucNguyenResort.Web.Controllers
{
    public class TourGuideController : Controller
    {
        private readonly ResortDbContext _context;

        public TourGuideController(ResortDbContext context)
        {
            _context = context;
        }

        // Action hiển thị lịch trình Dashboard của Tour Guide
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.TourSchedules
                .Include(s => s.Tour)
                .OrderBy(s => s.TourDate)
                .ToListAsync();

            if (schedules != null)
            {
                foreach (var s in schedules)
                {
                    var duration = s.Tour?.DurationHours ?? 4;

                    // XỬ LÝ AN TOÀN KIỂU DỮ LIỆU Nullable TimeSpan
                    TimeSpan startTime = s.StartTime ?? TimeSpan.Zero;
                    TimeSpan endTimeSpan = startTime.Add(TimeSpan.FromHours(duration));

                    ViewData[$"EndTime_{s.ScheduleId}"] = endTimeSpan.ToString(@"hh\:mm");

                    // Tính toán trạng thái thực tế dựa theo mốc thời gian thực
                    var now = DateTime.Now;
                    var tourDate = s.TourDate ?? DateTime.Today;
                    var tourStartDateTime = tourDate.Date.Add(startTime);
                    var tourEndDateTime = tourStartDateTime.Add(TimeSpan.FromHours(duration));

                    string status = "Upcoming";
                    if (now >= tourStartDateTime && now <= tourEndDateTime)
                    {
                        status = "Ongoing";
                    }
                    else if (now > tourEndDateTime)
                    {
                        status = "Completed";
                    }

                    ViewData[$"Status_{s.ScheduleId}"] = status;
                }
            }

            return View(schedules);
        }

        // Action xem danh sách khách đi tour (Guest List)
        // Action xem danh sách khách đi tour (Guest List)
        public async Task<IActionResult> Participants(int scheduleId)
        {
            var schedule = await _context.TourSchedules
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null)
            {
                return NotFound();
            }

            ViewBag.Schedule = schedule;

            // TÍNH TOÁN TRẠNG THÁI THỰC TẾ CỦA TOUR ĐỂ CHUYỂN SANG GIAO DIỆN
            var duration = schedule.Tour?.DurationHours ?? 4;
            var startTime = schedule.StartTime ?? TimeSpan.Zero;
            var tourStartDateTime = (schedule.TourDate ?? DateTime.Today).Date.Add(startTime);
            var tourEndDateTime = tourStartDateTime.Add(TimeSpan.FromHours(duration));
            var now = DateTime.Now;

            string currentStatus = "cho-xu-ly";
            if (now >= tourStartDateTime && now <= tourEndDateTime)
            {
                currentStatus = "dang-dien-ra";
            }
            else if (now > tourEndDateTime)
            {
                currentStatus = "hoan-thanh";
            }
            ViewBag.TourStatus = currentStatus;

            // Lấy dữ liệu từ Database, Include bảng User để hiển thị Tên và Email thật
            var allBookingsInSchedule = await _context.TourBookings
                .Include(b => b.User)
                .Where(b => b.ScheduleId == scheduleId)
                .ToListAsync();

            // Lọc danh sách trạng thái Confirmed
            var confirmedParticipants = allBookingsInSchedule
                .Where(b => b.BookingStatus != null &&
                           (b.BookingStatus.ToString() == "Confirmed" || b.BookingStatus.ToString().Contains("Confirmed")))
                .ToList();

            return View(confirmedParticipants);
        }
    }
}