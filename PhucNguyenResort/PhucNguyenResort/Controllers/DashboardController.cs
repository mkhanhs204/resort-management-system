using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;
        private readonly PhucNguyenResort.Data.ResortDbContext _context;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            PhucNguyenResort.Data.ResortDbContext context)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _context = context;
        }

        // Trang chính: Tự động chuyển hướng đến dashboard phù hợp với role của người dùng
        public IActionResult Index()
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
            if (User.IsInRole("Housekeeping Staff"))
                return RedirectToAction("Housekeeping");
            if (User.IsInRole("Maintenance Staff"))
                return RedirectToAction("Maintenance");

            return View();
        }

        // 1. System Admin Dashboard
        [Authorize(Roles = "System Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        // 2. Business Manager Dashboard
        [Authorize(Roles = "Business Manager")]
        public IActionResult Manager()
        {
            return View();
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
            
            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success)
            {
                TempData["SuccessMessage"] = "Food order status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to update food order status.";
            }

            return RedirectToAction(nameof(FoodOrders));
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
            
            if (result.Status == PhucNguyenResort.Services.FoodOrders.FoodOrderActionStatus.Success)
            {
                TempData["SuccessMessage"] = "KOT status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to update KOT status.";
            }

            return RedirectToAction(nameof(Kitchen));
        }

        // 6. Tour Guide Dashboard
        [Authorize(Roles = "Tour Guide")]
        public IActionResult TourGuide()
        {
            return View();
        }

        // 7. Housekeeping Staff Dashboard
        [Authorize(Roles = "Housekeeping Staff")]
        public IActionResult Housekeeping()
        {
            return View();
        }

        // 8. Maintenance Staff Dashboard
        [Authorize(Roles = "Maintenance Staff")]
        public IActionResult Maintenance()
        {
            return View();
        }
    }
}
