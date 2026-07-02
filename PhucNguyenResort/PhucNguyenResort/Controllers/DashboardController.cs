using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data; // Nhận diện ResortDbContext
using PhucNguyenResort.Enums; // Nhận diện trạng thái TourBookingStatus
using PhucNguyenResort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Web.Controllers
{
    // Cấu trúc ViewModel tạm thời được giữ lại để tránh lỗi tham chiếu nếu có hệ thống khác gọi đến
    public class TourGuideOverviewViewModel
    {
        public int TotalAssignedTours { get; set; }
        public double CompletionRate { get; set; }
        public int TotalGuestsServed { get; set; }
        public List<TourSchedule> ActiveSchedules { get; set; } = new List<TourSchedule>();
    }

    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;
        private readonly ResortDbContext _context;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            ResortDbContext context)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _context = context;
        }

        // Trang chính: Tự động chuyển hướng đến dashboard phù hợp với role của người dùng
        public IActionResult Index()
        {
            if (User.IsInRole("System Admin")) return RedirectToAction("Admin");
            if (User.IsInRole("Business Manager")) return RedirectToAction("Manager");
            if (User.IsInRole("Receptionist")) return RedirectToAction("Receptionist");
            if (User.IsInRole("F&B Staff")) return RedirectToAction("FnB");
            if (User.IsInRole("Kitchen Staff")) return RedirectToAction("Kitchen");
            if (User.IsInRole("Tour Guide")) return RedirectToAction("TourGuide");
            if (User.IsInRole("Housekeeping Staff")) return RedirectToAction("Housekeeping");
            if (User.IsInRole("Maintenance Staff")) return RedirectToAction("Maintenance");

            return View();
        }

        [Authorize(Roles = "System Admin")] public IActionResult Admin() => View();
        [Authorize(Roles = "Business Manager")] public IActionResult Manager() => View();
        [Authorize(Roles = "Receptionist")] public IActionResult Receptionist() => View();
        [Authorize(Roles = "F&B Staff")] public IActionResult FnB() => View();

        [Authorize(Roles = "F&B Staff")]
        public async Task<IActionResult> FoodOrders()
        {
            var orders = await _foodOrderService.GetAllFoodOrdersAsync();
            return View("~/Views/FoodOrders/FoodOrders.cshtml", orders);
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFoodOrderStatus(int foodOrderId, PhucNguyenResort.Enums.FoodOrderStatus newStatus)
        {
            var result = await _foodOrderService.UpdateOrderStatusAsync(foodOrderId, newStatus);
            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success) TempData["SuccessMessage"] = "Food order status updated successfully.";
            else TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to update food order status.";
            return RedirectToAction(nameof(FoodOrders));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        public async Task<IActionResult> ChargeFoodOrderToRoom(int foodOrderId)
        {
            var result = await _foodOrderService.ChargeToRoomAsync(foodOrderId);
            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success) TempData["SuccessMessage"] = "Successfully charged the food order to the room folio.";
            else TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to charge the food order.";
            return RedirectToAction(nameof(FoodOrders));
        }

        [Authorize(Roles = "Kitchen Staff")]
        public async Task<IActionResult> Kitchen()
        {
            var kots = await _kotService.GetAllKotsAsync();
            return View("~/Views/Dashboard/Kitchen.cshtml", kots);
        }

        [HttpPost]
        [Authorize(Roles = "Kitchen Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateKotStatus(int kotId, PhucNguyenResort.Enums.KotStatus newStatus)
        {
            var result = await _kotService.UpdateKotStatusAsync(kotId, newStatus);
            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success) TempData["SuccessMessage"] = "KOT status updated successfully.";
            else TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to update KOT status.";
            return RedirectToAction(nameof(Kitchen));
        }

        // XỬ LÝ CHÍNH: Đồng bộ hoàn hảo và ép render trỏ đích danh tới file Luxury UI trong thư mục Dashboard
        [Authorize(Roles = "Tour Guide")]
        public async Task<IActionResult> TourGuide()
        {
            var schedules = await _context.TourSchedules
                .Include(s => s.Tour)
                .OrderBy(s => s.TourDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

            var activeSchedules = new List<TourSchedule>();

            if (schedules != null)
            {
                var now = DateTime.Now;

                foreach (var s in schedules)
                {
                    var duration = s.Tour?.DurationHours ?? 4;
                    TimeSpan startTime = s.StartTime ?? TimeSpan.Zero;
                    TimeSpan endTimeSpan = startTime.Add(TimeSpan.FromHours(duration));

                    // Lưu thông tin giờ kết thúc tính toán được vào hệ thống hiển thị
                    ViewData[$"EndTime_{s.ScheduleId}"] = endTimeSpan.ToString(@"hh\:mm");

                    var tourStartDateTime = (s.TourDate ?? DateTime.Today).Date.Add(startTime);
                    var tourEndDateTime = tourStartDateTime.Add(TimeSpan.FromHours(duration));

                    var bookings = await _context.TourBookings
                        .Where(b => b.ScheduleId == s.ScheduleId && b.BookingStatus != TourBookingStatus.Pending)
                        .ToListAsync();

                    // Nếu tất cả đơn đặt bị hủy, gán trạng thái hủy để View tự động xử lý loại bỏ
                    if (bookings.Any() && bookings.All(b => b.BookingStatus == TourBookingStatus.Cancelled))
                    {
                        ViewData[$"Status_{s.ScheduleId}"] = "Cancelled";
                    }
                    else if (now >= tourStartDateTime && now <= tourEndDateTime)
                    {
                        ViewData[$"Status_{s.ScheduleId}"] = "Ongoing";
                    }
                    else if (now > tourEndDateTime)
                    {
                        ViewData[$"Status_{s.ScheduleId}"] = "Completed";
                    }
                    else
                    {
                        ViewData[$"Status_{s.ScheduleId}"] = "Upcoming";
                    }

                    activeSchedules.Add(s);
                }
            }

            // Gọi tường minh File View cụ thể nằm trong Views/Dashboard để đảm bảo render đúng giao diện mong muốn
            return View("~/Views/Dashboard/TourGuide.cshtml", activeSchedules);
        }

        [Authorize(Roles = "Housekeeping Staff")] public IActionResult Housekeeping() => View();
        [Authorize(Roles = "Maintenance Staff")] public IActionResult Maintenance() => View();
    }
}