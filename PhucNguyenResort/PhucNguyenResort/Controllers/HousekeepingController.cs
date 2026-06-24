using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Housekeeping;

namespace PhucNguyenResort.Controllers
{
    public class HousekeepingController : Controller
    {
        private readonly IHousekeepingService _housekeepingService;

        public HousekeepingController(IHousekeepingService housekeepingService)
        {
            _housekeepingService = housekeepingService;
        }

        public async Task<IActionResult> ViewDirtyRooms()
        {
            var dirtyRooms = await _housekeepingService.GetDirtyRoomsAsync();
            return View(dirtyRooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCleaningStatus(int roomId)
        {
            var result = await _housekeepingService.UpdateCleaningStatusAsync(roomId);
            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật trạng thái phòng thành công! Phòng đã sẵn sàng đón khách.";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra hoặc không tìm thấy thông tin phòng.";
            }
            return RedirectToAction(nameof(ViewDirtyRooms));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportMaintenance(int roomId, string description)
        {
            var result = await _housekeepingService.ReportMaintenanceAsync(roomId, description);
            if (result)
            {
                TempData["SuccessMessage"] = "Đã chuyển trạng thái phòng sang Bảo trì và tạo phiếu sửa chữa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra hoặc không tìm thấy thông tin phòng.";
            }
            return RedirectToAction(nameof(ViewDirtyRooms));
        }
    }
}