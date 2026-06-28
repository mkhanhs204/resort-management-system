using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.TableReservations;

namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager,Receptionist,F&B Staff,Kitchen Staff,Tour Guide,Housekeeping Staff,Maintenance Staff")]
    public class DashboardController : Controller
    {
        private readonly PhucNguyenResort.Services.FoodOrders.IFoodOrderService _foodOrderService;
        private readonly PhucNguyenResort.Services.Kots.IKotService _kotService;
        private readonly ITableReservationService _tableReservationService;

        public DashboardController(
            PhucNguyenResort.Services.FoodOrders.IFoodOrderService foodOrderService,
            PhucNguyenResort.Services.Kots.IKotService kotService,
            ITableReservationService tableReservationService)
        {
            _foodOrderService = foodOrderService;
            _kotService = kotService;
            _tableReservationService = tableReservationService;
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
        public async Task<IActionResult> Manager()
        {
            var model = await _tableReservationService.GetFnBDashboardAsync(DateTime.Today);
            return View(model);
        }

        // 3. Receptionist (Front Desk) Dashboard
        [Authorize(Roles = "Receptionist")]
        public IActionResult Receptionist()
        {
            return View();
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

        private int? GetCurrentUserId()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdValue, out var userId)
                ? userId
                : null;
        }
    }
}
