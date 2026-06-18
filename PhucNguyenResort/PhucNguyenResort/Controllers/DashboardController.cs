using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
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
