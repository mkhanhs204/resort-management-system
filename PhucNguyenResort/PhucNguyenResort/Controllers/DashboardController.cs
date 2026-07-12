using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data; // Nhận diện ResortDbContext
using PhucNguyenResort.Enums; // Nhận diện trạng thái TourBookingStatus
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Notifications;
using PhucNguyenResort.Services.TableReservations;
using PhucNguyenResort.Services.Dashboard;
using PhucNguyenResort.Services.Housekeeping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;
        private readonly PhucNguyenResort.Data.ResortDbContext _context;
        private readonly ITableReservationService _tableReservationService;
        private readonly INotificationService _notificationService;
        private readonly IHousekeepingService _housekeepingService;

        private readonly IDashboardReportService _reportService;
        private readonly IForecastService _forecastService;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            PhucNguyenResort.Data.ResortDbContext context,
            ITableReservationService tableReservationService,
            INotificationService notificationService,
            IHousekeepingService housekeepingService,
            IDashboardReportService reportService,
            IForecastService forecastService)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _context = context;
            _tableReservationService = tableReservationService;
            _notificationService = notificationService;
            _housekeepingService = housekeepingService;
            _reportService = reportService;
            _forecastService = forecastService;
        }

        // =========================================================================
        // TRANG CỬA NGÕ: Tự động điều hướng User về đúng trang Overview theo vai trò
        // =========================================================================
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("System Admin"))
                return RedirectToAction("Admin");
            if (User.IsInRole("Business Manager"))
                return RedirectToAction("Manager");
            if (User.IsInRole("Receptionist"))
                return RedirectToAction("Receptionist");
            if (User.IsInRole("F&B Staff"))
                return RedirectToAction("FnB");
            if (User.IsInRole("Kitchen Staff"))
                return RedirectToAction("Kitchen");
            if (User.IsInRole("Tour Guide"))
                return RedirectToAction("TourGuide");

            // ĐỒNG BỘ 1: Nếu là Housekeeping, hướng về trang Overview mới đặt tên
            if (User.IsInRole("Housekeeping Staff"))
            {
                return RedirectToAction("Overview", "Housekeeping");
            }

            // ĐỒNG BỘ 2: Nếu là Maintenance, chuyển hướng thẳng sang Controller riêng để xử lý mượt mà
            if (User.IsInRole("Maintenance Staff"))
                return RedirectToAction("Overview", "Maintenance");

            return View();
        }

        // =========================================================================
        // PHÂN HỆ HOUSEKEEPING STAFF (QUẢN LÝ TẬP TRUNG TẠI DASHBOARD CONTROLLER)
        // =========================================================================

        // Trang 1: Xem số liệu tổng quan (File cũ là Index.cshtml)
        [Authorize(Roles = "Housekeeping Staff")]
        public async Task<IActionResult> HousekeepingOverview()
        {
            ViewData["RoomsNeedCleaningCount"] = await _housekeepingService.GetCountRoomsNeedCleaningAsync();
            ViewData["CurrentlyCleaningCount"] = await _housekeepingService.GetCountCurrentlyCleaningAsync();
            ViewData["CleanedTodayCount"] = await _housekeepingService.GetCountCleanedTodayAsync();

            return View("HousekeepingOverview");
        }

        // Trang 2: Hàng chờ xử lý phòng chi tiết (File cũ là Housekeeping.cshtml)
        [Authorize(Roles = "Housekeeping Staff")]
        public async Task<IActionResult> Housekeeping()
        {
            ViewData["RoomsNeedCleaningCount"] = await _housekeepingService.GetCountRoomsNeedCleaningAsync();
            ViewData["CurrentlyCleaningCount"] = await _housekeepingService.GetCountCurrentlyCleaningAsync();
            ViewData["CleanedTodayCount"] = await _housekeepingService.GetCountCleanedTodayAsync();

            var dirtyRooms = await _housekeepingService.GetDirtyRoomsAsync();

            return View("HousekeepingDetails", dirtyRooms);
        }


        // =========================================================================
        // CÁC PHÂN HỆ KHÁC (GIỮ NGUYÊN VẸN LOGIC LOG ĐĂNG NHẬP CŨ CỦA DỰ ÁN)
        // =========================================================================

        // 1. System Admin Dashboard
        [Authorize(Roles = "System Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        // 2. Business Manager Dashboard
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Manager(int? month, int? year)
        {
            var date = DateTime.Today;
            var tableReservations = await _tableReservationService.GetFnBDashboardAsync(date);
            var occupancy = await _reportService.GetOccupancyDataAsync(month ?? date.Month, year ?? date.Year);
            var revenue = await _reportService.GetRevenueBreakdownAsync(month ?? date.Month, year ?? date.Year);
            
            // If month/year is 0, we treat it as All-Time
            if (month == 0 || year == 0)
            {
                occupancy = await _reportService.GetOccupancyDataAsync(null, null);
                revenue = await _reportService.GetRevenueBreakdownAsync(null, null);
                month = 0;
                year = 0;
            }
            else
            {
                month = month ?? date.Month;
                year = year ?? date.Year;
            }

            var model = new PhucNguyenResort.ViewModels.Dashboard.ManagerDashboardViewModel
            {
                TableReservationDashboard = tableReservations,
                Occupancy = occupancy,
                Revenue = revenue,
                SelectedMonth = month,
                SelectedYear = year
            };
            
            ViewBag.TotalRoomCategories = await _context.RoomCategories.CountAsync();
            ViewBag.TotalTours = await _context.Tours.CountAsync();
            ViewBag.PendingRefunds = await _context.Refunds.CountAsync(r => r.RefundStatus == PhucNguyenResort.Enums.RefundStatus.Pending);

            return View(model);
        }

        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> FinancialReport(int? month, int? year)
        {
            if (month == 0 || year == 0)
            {
                var report = await _reportService.GetFinancialReportAsync(null, null);
                report.Month = 0;
                report.Year = 0;
                return View(report);
            }
            else
            {
                int m = month ?? DateTime.Today.Month;
                int y = year ?? DateTime.Today.Year;

                if (m < 1 || m > 12 || y < 2000 || y > 2100)
                {
                    return BadRequest("Invalid month or year.");
                }

                var report = await _reportService.GetFinancialReportAsync(m, y);
                return View(report);
            }
        }

        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Forecast()
        {
            var forecast = await _forecastService.GetForecastAsync();
            return View(forecast);
        }

        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> ExportExcel(int? month, int? year)
        {
            if (month == 0 || year == 0)
            {
                var bytes = await _reportService.ExportToExcelAsync(null, null);
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FinancialReport_AllTime.xlsx");
            }
            else
            {
                int m = month ?? DateTime.Today.Month;
                int y = year ?? DateTime.Today.Year;
                if (m < 1 || m > 12 || y < 2000 || y > 2100)
                {
                    return BadRequest("Invalid month or year.");
                }

                var bytes = await _reportService.ExportToExcelAsync(m, y);
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FinancialReport_{y}_{m:D2}.xlsx");
            }
        }

        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> ExportPdf(int? month, int? year)
        {
            if (month == 0 || year == 0)
            {
                var bytes = await _reportService.ExportToPdfAsync(null, null);
                return File(bytes, "application/pdf", $"FinancialReport_AllTime.pdf");
            }
            else
            {
                int m = month ?? DateTime.Today.Month;
                int y = year ?? DateTime.Today.Year;
                if (m < 1 || m > 12 || y < 2000 || y > 2100)
                {
                    return BadRequest("Invalid month or year.");
                }

                var bytes = await _reportService.ExportToPdfAsync(m, y);
                return File(bytes, "application/pdf", $"FinancialReport_{y}_{m:D2}.pdf");
            }
        }

        // 3. Receptionist (Front Desk) Dashboard
        [Authorize(Roles = "Receptionist")]
        public async Task<IActionResult> Receptionist()
        {
            var today = DateTime.Today;

            var todaysArrivals = await _context.Bookings
                .CountAsync(b => b.CheckInDate.Date == today && (b.BookingStatus == BookingStatus.Reserved || b.BookingStatus == BookingStatus.Arrived || b.BookingStatus == BookingStatus.PendingPayment));

            var activeStayGuests = await _context.Bookings
                .CountAsync(b => b.BookingStatus == BookingStatus.CheckedIn);

            var todaysDepartures = await _context.Bookings
                .CountAsync(b => b.CheckOutDate.Date == today && b.BookingStatus == BookingStatus.CheckedIn);

            var availableRooms = await _context.Rooms
                .CountAsync(r => r.Status == RoomStatus.Available);

            var recentBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToListAsync();

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
        public IActionResult FnB()
        {
            return View();
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

            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success)
            {
                TempData["SuccessMessage"] = "Successfully charged the food order to the room folio.";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to charge the food order.";
            }

            return RedirectToAction(nameof(FoodOrders));
        }

        // 5. Kitchen Staff Dashboard
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
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var schedules = await _context.TourSchedules
                .Include(s => s.Tour)
                .Where(s => s.GuideId == currentUserId.Value)
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

        private int? GetCurrentUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdValue, out var userId)
                ? userId
                : null;
        }
    }
}
