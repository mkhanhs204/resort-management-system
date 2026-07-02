using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;

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
            // 🔥 LOẠI BỎ HỦY: Chỉ lấy những lịch trình có trạng thái thực tế không bị hủy hoàn toàn từ cấu hình (nếu có)
            var schedules = await _context.TourSchedules
                .Include(s => s.Tour)
                .OrderBy(s => s.TourDate)
                .ToListAsync();

            if (schedules != null)
            {
                var now = DateTime.Now;

                // Khởi tạo các biến đếm thẻ Thống kê (Chỉ giữ lại 3 trạng thái theo yêu cầu)
                int upcomingCount = 0;
                int ongoingCount = 0;
                int completedCount = 0;

                // Tạo danh sách tạm thời để lọc bỏ hoàn toàn các tour có trạng thái Cancelled
                var activeSchedules = new System.Collections.Generic.List<PhucNguyenResort.Models.TourSchedule>();

                foreach (var s in schedules)
                {
                    var duration = s.Tour?.DurationHours ?? 4;
                    TimeSpan startTime = s.StartTime ?? TimeSpan.Zero;
                    TimeSpan endTimeSpan = startTime.Add(TimeSpan.FromHours(duration));

                    ViewData[$"EndTime_{s.ScheduleId}"] = endTimeSpan.ToString(@"hh\:mm");

                    var tourStartDateTime = (s.TourDate ?? DateTime.Today).Date.Add(startTime);
                    var tourEndDateTime = tourStartDateTime.Add(TimeSpan.FromHours(duration));

                    var bookings = await _context.TourBookings
                        .Where(b => b.ScheduleId == s.ScheduleId && b.BookingStatus != TourBookingStatus.Pending)
                        .ToListAsync();

                    string status = "Upcoming";

                    // 🔥 LOẠI BỎ HỦY: Nếu tất cả booking bị hủy, chúng ta xem như tour này không hiển thị/không tính
                    if (bookings.Any() && bookings.All(b => b.BookingStatus == TourBookingStatus.Cancelled))
                    {
                        continue; // Bỏ qua không đưa vào danh sách hiển thị
                    }
                    else if (now >= tourStartDateTime && now <= tourEndDateTime)
                    {
                        status = "Ongoing";
                        ongoingCount++;
                    }
                    else if (now > tourEndDateTime)
                    {
                        status = "Completed";
                        completedCount++;
                    }
                    else
                    {
                        status = "Upcoming";
                        upcomingCount++;
                    }

                    ViewData[$"Status_{s.ScheduleId}"] = status;
                    activeSchedules.Add(s);
                }

                // Truyền số lượng thống kê sang View
                ViewBag.UpcomingCount = upcomingCount;
                ViewBag.OngoingCount = ongoingCount;
                ViewBag.CompletedCount = completedCount;

                return View(activeSchedules);
            }

            return View(schedules);
        }

        // Action xem danh sách vận hành hành khách (Guest List)
        public async Task<IActionResult> Participants(int scheduleId)
        {
            var schedule = await _context.TourSchedules
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null) return NotFound();

            ViewBag.Schedule = schedule;

            // Tính toán giờ kết thúc
            var duration = schedule.Tour?.DurationHours ?? 4;
            TimeSpan startTime = schedule.StartTime ?? TimeSpan.Zero;
            ViewBag.EndTime = startTime.Add(TimeSpan.FromHours(duration));

            var now = DateTime.Now;
            var tourDate = schedule.TourDate ?? DateTime.Today;
            var tourStartDateTime = tourDate.Date.Add(startTime);
            var tourEndDateTime = tourStartDateTime.Add(TimeSpan.FromHours(duration));

            // 🔥 ĐỒNG BỘ TRẠNG THÁI TỪ INDEX: Tính toán trạng thái thực tế tổng của Tour để truyền sang View
            string tourScheduleStatus = "Upcoming";
            if (now >= tourStartDateTime && now <= tourEndDateTime)
            {
                tourScheduleStatus = "Ongoing";
            }
            else if (now > tourEndDateTime)
            {
                tourScheduleStatus = "Completed";
            }
            ViewBag.TourScheduleStatus = tourScheduleStatus;

            // 🔥 LOẠI BỎ HỦY: Chỉ lấy khách hàng Confirmed, CheckedIn, NoShow, Completed (Bỏ qua Pending và Cancelled)
            var participants = await _context.TourBookings
                .Include(b => b.User)
                .Where(b => b.ScheduleId == scheduleId &&
                            b.BookingStatus != TourBookingStatus.Pending &&
                            b.BookingStatus != TourBookingStatus.Cancelled)
                .ToListAsync();

            // Sửa lỗi dữ liệu ảo dựa theo trạng thái thực tế tổng của Tour lịch trình
            if (tourScheduleStatus == "Upcoming")
            {
                bool needFix = false;
                foreach (var item in participants)
                {
                    if (item.BookingStatus == TourBookingStatus.CheckedIn ||
                        item.BookingStatus == TourBookingStatus.NoShow ||
                        item.BookingStatus == TourBookingStatus.Completed)
                    {
                        item.BookingStatus = TourBookingStatus.Confirmed;
                        needFix = true;
                    }
                }
                if (needFix)
                {
                    await _context.SaveChangesAsync();
                }
            }

            return View(participants);
        }

        // API AJAX: Thực hiện cập nhật trạng thái trực tiếp
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int tourBookingId, TourBookingStatus status)
        {
            var booking = await _context.TourBookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                .FirstOrDefaultAsync(b => b.TourBookingId == tourBookingId);

            if (booking == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dữ liệu đơn đặt tour này." });
            }

            // Chặn logic thao tác sớm trước khi tour diễn ra
            if (status == TourBookingStatus.CheckedIn || status == TourBookingStatus.NoShow || status == TourBookingStatus.Completed)
            {
                var now = DateTime.Now;
                var tourDate = booking.Schedule?.TourDate ?? DateTime.Today;
                var startTime = booking.Schedule?.StartTime ?? TimeSpan.Zero;
                var tourStartDateTime = tourDate.Date.Add(startTime);

                if (now < tourStartDateTime)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Tour này ở trạng thái [Sắp diễn ra] (Bắt đầu lúc {tourStartDateTime:HH:mm dd/MM/yyyy}). Bạn không thể vận hành hành khách trước khi tour bắt đầu!"
                    });
                }
            }

            try
            {
                booking.BookingStatus = status;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi dữ liệu: " + ex.Message });
            }
        }
    }
}