using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Housekeeping;
using PhucNguyenResort.Services.Maintenance;

namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;
        private readonly IHousekeepingService _housekeepingService;
        private readonly IMaintenanceService _maintenanceService;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            IHousekeepingService housekeepingService,
            IMaintenanceService maintenanceService)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _housekeepingService = housekeepingService;
            _maintenanceService = maintenanceService;
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
        public IActionResult Manager()
        {
            return View();
        }

        // 3. Receptionist (Front Desk) Dashboard
        [Authorize(Roles = "Receptionist")]
        public IActionResult Receptionist()
        {
            return View();
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
    }
}