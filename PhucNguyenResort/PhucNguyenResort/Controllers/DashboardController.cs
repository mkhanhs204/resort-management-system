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
        private readonly PhucNguyenResort.Data.ResortDbContext _context;
        private readonly ITableReservationService _tableReservationService;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            PhucNguyenResort.Data.ResortDbContext context,
            ITableReservationService tableReservationService)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _context = context;
            _tableReservationService = tableReservationService;
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

        // 1. System Admin Dashboard
        [Authorize(Roles = "System Admin")]
        public async Task<IActionResult> Admin()
        {
            var totalStaff = await _context.Users.CountAsync();
            var activeAccounts = await _context.Users.CountAsync(u => u.IsActive && !u.IsBlocked);
            var systemRoles = await _context.Roles.CountAsync();
            var inactiveAccounts = totalStaff - activeAccounts;

            ViewBag.TotalStaff = totalStaff;
            ViewBag.ActiveAccounts = activeAccounts;
            ViewBag.SystemRoles = systemRoles;
            ViewBag.InactiveAccounts = inactiveAccounts;

            var recentStaff = await _context.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentStaff = recentStaff;

            return View();
        }

        // 2. Business Manager Dashboard
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Manager()
        {
            var model = await _tableReservationService.GetFnBDashboardAsync(DateTime.Today);
            
            ViewBag.TotalRoomCategories = await _context.RoomCategories.CountAsync();
            ViewBag.TotalTours = await _context.Tours.CountAsync();
            ViewBag.PendingRefunds = await _context.Refunds.CountAsync(r => r.RefundStatus == PhucNguyenResort.Enums.RefundStatus.Pending);

            return View(model);
        }

        // 3. Receptionist (Front Desk) Dashboard
        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> Receptionist()
        {
            var today = DateTime.Today;

            var todaysArrivals = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(
                _context.Bookings.Where(b => b.CheckInDate.Date == today && b.BookingStatus != Enums.BookingStatus.Cancelled)
            );

            var activeStayGuests = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(
                _context.Bookings.Where(b => b.BookingStatus == Enums.BookingStatus.CheckedIn)
            );

            var todaysDepartures = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(
                _context.Bookings.Where(b => b.CheckOutDate.Date == today && b.BookingStatus != Enums.BookingStatus.Cancelled)
            );

            var availableRooms = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.CountAsync(
                _context.Rooms.Where(r => r.Status == Enums.RoomStatus.Available)
            );

            var recentBookings = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
                _context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .OrderByDescending(b => b.BookingId)
                    .Take(5)
            );

            var model = new PhucNguyenResort.ViewModels.Dashboard.ReceptionistDashboardViewModel
            {
                TodaysArrivals = todaysArrivals,
                ActiveStayGuests = activeStayGuests,
                TodaysDepartures = todaysDepartures,
                AvailableRooms = availableRooms,
                RecentBookings = recentBookings
            };

            return View(model);
        }

        // 4. F&B Staff Dashboard
        [Authorize(Roles = "F&B Staff")]
        public async Task<IActionResult> FnB()
        {
            var model = await _tableReservationService.GetFnBDashboardAsync(DateTime.Today);
            return View(model);
        }

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

        [Authorize(Roles = "F&B Staff,Business Manager")]
        public async Task<IActionResult> TableReservations(DateTime? date, TableReservationStatus? status)
        {
            var reservations = await _tableReservationService.GetStaffReservationsAsync(date, status);
            ViewBag.SelectedDate = date;
            ViewBag.SelectedStatus = status;
            ViewBag.CanManageReservationStatus = User.IsInRole("F&B Staff");
            ViewBag.BackAction = User.IsInRole("Business Manager") ? nameof(Manager) : nameof(FnB);

            return View("~/Views/Dashboard/TableReservations.cshtml", reservations);
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmTableReservation(int reservationId)
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Challenge();
            }

            var success = await _tableReservationService.ConfirmReservationAsync(
                reservationId,
                currentUserId.Value);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "Table reservation confirmed successfully."
                : "Unable to confirm this table reservation.";

            return RedirectToAction(nameof(TableReservations));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SeatTableReservation(int reservationId)
        {
            var success = await _tableReservationService.SeatReservationAsync(reservationId);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "Guest has been seated successfully."
                : "Unable to seat this table reservation.";

            return RedirectToAction(nameof(TableReservations));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteTableReservation(int reservationId)
        {
            var success = await _tableReservationService.CompleteReservationAsync(reservationId);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "Table reservation completed successfully."
                : "Unable to complete this table reservation.";

            return RedirectToAction(nameof(TableReservations));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkTableReservationNoShow(int reservationId)
        {
            var success = await _tableReservationService.MarkReservationNoShowAsync(reservationId);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "Table reservation was marked as no-show."
                : "No-show can only be marked after the table hold period has passed.";

            return RedirectToAction(nameof(TableReservations));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelTableReservation(int reservationId, string? reason)
        {
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Challenge();
            }

            var success = await _tableReservationService.CancelReservationByStaffAsync(
                reservationId,
                currentUserId.Value,
                reason);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                ? "Table reservation cancelled successfully."
                : "Unable to cancel this table reservation.";

            return RedirectToAction(nameof(TableReservations));
        }

        [HttpPost]
        [Authorize(Roles = "F&B Staff")]
        [ValidateAntiForgeryToken]
        public IActionResult ChargeTableReservationToRoom(int reservationId, decimal amount)
        {
            TempData["ErrorMessage"] = "Please use table food order checkout. Manual table charge amounts are no longer supported.";
            return RedirectToAction("Checkout", "TableFoodOrders", new { reservationId });
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

        // 8. Maintenance Staff Dashboard
        [Authorize(Roles = "Maintenance Staff")]
        public IActionResult Maintenance()
        {
            return View();
        }

        private int? GetCurrentUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdValue, out var userId)
                ? userId
                : null;
        }
    }
}