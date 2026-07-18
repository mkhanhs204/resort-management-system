using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Maintenance;

namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "Maintenance Staff,System Admin")]
    public class MaintenanceController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // =========================================================================
        // TRANG 1: TỔNG QUAN BẢO TRÌ (KPI CARDS)
        // =========================================================================
        public async Task<IActionResult> Overview()
        {
            ViewData["PendingTasksCount"] = await _maintenanceService.GetPendingTasksCountAsync();
            ViewData["ResolvedTodayCount"] = await _maintenanceService.GetResolvedTodayCountAsync();

            return View("MaintenanceOverview");
        }

        // =========================================================================
        // TRANG 2: CHI TIẾT SỰ CỐ BẢO TRÌ (BAO GỒM BỘ LỌC AN TOÀN & THÀNH TÍCH CA)
        // =========================================================================
        public async Task<IActionResult> Details()
        {
            string currentStaff = User.Identity?.Name ?? "Unknown Staff";

            // 1. Thống kê số liệu ở các thẻ Card phía trên
            ViewData["PendingTasksCount"] = await _maintenanceService.GetPendingTasksCountAsync();
            ViewData["ResolvedTodayCount"] = await _maintenanceService.GetResolvedTodayCountAsync();

            // 2. Lấy danh sách toàn bộ các sự cố đang cần xử lý (Pending hoặc Fixing)
            var activeTasks = await _maintenanceService.GetMaintenanceTasksAsync();

            // 3. LẤY LỊCH SỬ/THÀNH TÍCH: Sự cố mà CHÍNH nhân viên kỹ thuật này đã sửa xong hôm nay
            var resolvedTasksToday = await _maintenanceService.GetResolvedTasksByStaffTodayAsync(currentStaff);
            ViewBag.ResolvedTasksToday = resolvedTasksToday;

            // XỬ LÝ CHỦ ĐỘNG: Tiêu hủy TempData ngay tại đây để không đi lạc sang trang đăng nhập khi logout
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.LocalSuccessMessage = TempData["SuccessMessage"];
                TempData.Remove("SuccessMessage");
            }

            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.LocalErrorMessage = TempData["ErrorMessage"];
                TempData.Remove("ErrorMessage");
            }

            return View("MaintenanceDetails", activeTasks);
        }

        // =========================================================================
        // TÁC VỤ 1: TIẾP NHẬN TÁC VỤ SỬA CHỮA (ACCEPT / ASSIGN)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartFixing(int requestId)
        {
            string currentStaff = User.Identity?.Name ?? "Unknown Staff";

            // Gọi Service chuyển trạng thái từ Pending -> Fixing và lưu tên nhân viên sửa
            var result = await _maintenanceService.StartFixingTaskAsync(requestId, currentStaff);

            if (result) TempData["SuccessMessage"] = "Đã tiếp nhận tác vụ! Bạn đã được gán trách nhiệm sửa chữa phòng này.";
            else TempData["ErrorMessage"] = "Không thể tiếp nhận. Sự cố đã có nhân viên kỹ thuật khác xử lý trước.";

            return RedirectToAction(nameof(Details));
        }

        // =========================================================================
        // TÁC VỤ 2: HOÀN THÀNH SỬA CHỮA (RESOLVE TASK)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveTask(int requestId)
        {
            string currentStaff = User.Identity?.Name ?? "Unknown Staff";

            // Chuyển trạng thái sang Resolved, đồng thời hệ thống tự đổi RoomStatus về Needs Cleaning
            var result = await _maintenanceService.ResolveTaskAsync(requestId, currentStaff);
            if (!result)
            {
                TempData["ErrorMessage"] = "Không thể hoàn thành tác vụ. Bạn không có quyền can thiệp vào phòng do người khác phụ trách!";
                return RedirectToAction(nameof(Details));
            }

            TempData["SuccessMessage"] = "Đã xác nhận khắc phục xong sự cố thiết bị! Phòng đã được chuyển giao an toàn sang trạng thái chờ dọn dẹp.";

            return RedirectToAction(nameof(Details));
        }

        // =========================================================================
        // TAB LỊCH SỬ 1: LẤY LỊCH SỬ SỬA CHỮA (PARTIAL VIEW)
        // =========================================================================
        [HttpGet]
        public async Task<IActionResult> GetMaintenanceHistory()
        {
            // Trả về dữ liệu kiểu List<MaintenanceHistoryViewModel> đã map sẵn từ tầng Service
            var viewModel = await _maintenanceService.GetAllMaintenanceHistoryRequestsAsync();
            return PartialView("_MaintenanceHistoryTable", viewModel);
        }

        // =========================================================================
        // TAB LỊCH SỬ MỚI 2: XEM NHANH LỊCH SỬ DỌN PHÒNG ĐỒNG BỘ (PARTIAL VIEW)
        // =========================================================================
        [HttpGet]
        public async Task<IActionResult> GetCleaningHistory()
        {
            // Trả về dữ liệu kiểu List<CleaningHistoryViewModel> đã map sẵn từ tầng Service
            var viewModel = await _maintenanceService.GetAllCleaningLogsForMaintenanceAsync();
            return PartialView("_CleaningHistoryTable", viewModel);
        }

        // =========================================================================
        // TRANG CHÍNH: HIỂN THỊ TOÀN BỘ VIEW LỊCH SỬ BẢO TRÌ (ĐÃ SỬA LỖI TRUYỀN DATA)
        // =========================================================================
        [HttpGet]
        public async Task<IActionResult> History()
        {
            // Gọi tầng Service lấy toàn bộ danh sách lịch sử sửa chữa đã được map sẵn sang ViewModel
            var viewModel = await _maintenanceService.GetAllMaintenanceHistoryRequestsAsync();

            // Trả về View kèm data danh sách để file History.cshtml Render không bị lỗi trống model
            return View(viewModel);
        }
    }
}