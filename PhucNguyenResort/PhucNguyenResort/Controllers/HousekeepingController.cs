using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Housekeeping;

namespace PhucNguyenResort.Web.Controllers
{
    [Authorize(Roles = "Housekeeping Staff,System Admin")]
    public class HousekeepingController : Controller
    {
        private readonly IHousekeepingService _housekeepingService;

        public HousekeepingController(IHousekeepingService housekeepingService)
        {
            _housekeepingService = housekeepingService;
        }

        // =========================================================================
        // TRANG 1: TỔNG QUAN BUỒNG PHÒNG (KPI CARDS)
        // =========================================================================
        public async Task<IActionResult> Overview()
        {
            ViewData["RoomsNeedCleaningCount"] = await _housekeepingService.GetCountRoomsNeedCleaningAsync();
            ViewData["CurrentlyCleaningCount"] = await _housekeepingService.GetCountCurrentlyCleaningAsync();
            ViewData["CleanedTodayCount"] = await _housekeepingService.GetCountCleanedTodayAsync();

            return View("HousekeepingOverview");
        }

        // =========================================================================
        // TRANG 2: CHI TIẾT HÀNG CHỜ DỌN PHÒNG (ĐÃ SỬA LỖI ĐI LẠC THÔNG BÁO)
        // =========================================================================
        public async Task<IActionResult> Details()
        {
            string currentStaff = User.Identity?.Name ?? "Unknown Staff";

            // 1. Thống kê số liệu ở các thẻ Card phía trên
            ViewData["RoomsNeedCleaningCount"] = await _housekeepingService.GetCountRoomsNeedCleaningAsync();
            ViewData["CurrentlyCleaningCount"] = await _housekeepingService.GetCountCurrentlyCleaningAsync();
            ViewData["CleanedTodayCount"] = await _housekeepingService.GetCountCleanedTodayAsync();

            // 2. Lấy danh sách phòng bẩn/đang dọn
            var dirtyRooms = await _housekeepingService.GetDirtyRoomsAsync();

            // 3. Lấy danh sách phòng chính nhân viên này đã dọn xong hôm nay
            var cleanedRoomsToday = await _housekeepingService.GetCleanedRoomsByStaffTodayAsync(currentStaff);
            ViewBag.CleanedRoomsToday = cleanedRoomsToday;

            // XỬ LÝ CHỦ ĐỘNG: Tiêu hủy TempData ngay tại đây để không bị đi lạc sang trang đăng nhập khi logout
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

            return View("HousekeepingDetails", dirtyRooms);
        }

        // =========================================================================
        // TÁC VỤ 1: NHẬN DỌN PHÒNG (START CLEANING)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartCleaning(int roomId)
        {
            try
            {
                string currentStaff = User.Identity?.Name ?? "Unknown Staff";
                bool result = await _housekeepingService.UpdateRoomStatusWithStaffAsync(roomId, RoomStatus.CleaningInProgress, currentStaff);

                if (result) TempData["SuccessMessage"] = "Đã ghi nhận: Bạn đã nhận dọn dẹp phòng này!";
                else TempData["ErrorMessage"] = "Không thể nhận phòng. Có thể phòng đã được nhân viên khác tiếp nhận trước.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
            }

            return RedirectToAction(nameof(Details));
        }

        // =========================================================================
        // TÁC VỤ 2: HOÀN THÀNH DỌN PHÒNG (MARK CLEANED)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCleaned(int roomId)
        {
            string currentStaff = User.Identity?.Name ?? "Unknown Staff";
            var result = await _housekeepingService.UpdateCleaningStatusAsync(roomId, currentStaff);

            if (result) TempData["SuccessMessage"] = "Cập nhật trạng thái thành công! Phòng đã sạch sẽ và đưa vào danh sách thành tích của bạn.";
            else TempData["ErrorMessage"] = "Không thể cập nhật. Bạn không có quyền hoàn thành phòng do người khác đang dọn!";

            return RedirectToAction(nameof(Details));
        }

        // =========================================================================
        // TÁC VỤ 3: BÁO CÁO SỰ CỐ HỎNG PHÒNG (REPORT MAINTENANCE)
        // =========================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportMaintenance(int roomId, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập mô tả sự cố hỏng hóc.";
                return RedirectToAction(nameof(Details));
            }

            var result = await _housekeepingService.ReportMaintenanceAsync(roomId, description);
            if (result) TempData["SuccessMessage"] = "Báo cáo sự cố thành công! Phòng đã được chuyển sang trạng thái Bảo trì.";
            else TempData["ErrorMessage"] = "Gửi báo cáo thất bại. Vui lòng kiểm tra lại.";

            return RedirectToAction(nameof(Details));
        }
    }
}