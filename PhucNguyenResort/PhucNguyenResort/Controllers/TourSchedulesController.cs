using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhucNguyenResort.Services.Users;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PhucNguyenResort.Controllers
{
    [Authorize(Roles = "Business Manager")]
    public class TourSchedulesController : Controller
    {
        private readonly ITourScheduleService _scheduleService;
        private readonly ITourCatalogService _tourService;
        private readonly IUserService _userService;

        public TourSchedulesController(ITourScheduleService scheduleService, ITourCatalogService tourService, IUserService userService)
        {
            _scheduleService = scheduleService;
            _tourService = tourService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return View(schedules);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetScheduleByIdAsync(id.Value);
            if (schedule == null) return NotFound();

            return View(schedule);
        }

        public async Task<IActionResult> Create(int? tourId)
        {
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", tourId);
            var allUsers = await _userService.GetAllUsersAsync();
            ViewBag.Guides = new SelectList(allUsers.Where(u => u.Role?.RoleName == "Tour Guide" || u.Role?.RoleName == "Hướng Dẫn Viên"), "UserId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourSchedule schedule)
        {
            if (schedule.TourDate.HasValue && schedule.TourDate.Value.Date < System.DateTime.Now.Date)
            {
                ModelState.AddModelError("TourDate", "Ngày khởi hành không được nằm trong quá khứ.");
            }

            if (schedule.TourId > 0)
            {
                var tour = await _tourService.GetTourByIdAsync(schedule.TourId);
                if (tour != null && schedule.AvailableSlots > tour.MaxCapacity)
                {
                    ModelState.AddModelError("AvailableSlots", $"Số chỗ trống không được vượt quá số lượng tối đa của tour ({tour.MaxCapacity} chỗ).");
                }
            }

            if (schedule.GuideId.HasValue && schedule.TourDate.HasValue)
            {
                bool hasSchedule = await _scheduleService.HasScheduleOnDateAsync(schedule.GuideId.Value, schedule.TourDate.Value);
                if (hasSchedule)
                {
                    ModelState.AddModelError("GuideId", "Hướng dẫn viên này đã có lịch trình vào ngày này. Mỗi hướng dẫn viên chỉ được nhận 1 tour/ngày.");
                }
            }

            if (ModelState.IsValid)
            {
                await _scheduleService.AddScheduleAsync(schedule);
                TempData["SuccessMessage"] = "Tạo lịch trình tour mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            var allUsers = await _userService.GetAllUsersAsync();
            ViewBag.Guides = new SelectList(allUsers.Where(u => u.Role?.RoleName == "Tour Guide"), "UserId", "FullName", schedule.GuideId);
            return View(schedule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetScheduleByIdAsync(id.Value);
            if (schedule == null) return NotFound();

            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            var allUsers = await _userService.GetAllUsersAsync();
            ViewBag.Guides = new SelectList(allUsers.Where(u => u.Role?.RoleName == "Tour Guide"), "UserId", "FullName", schedule.GuideId);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourSchedule schedule)
        {
            if (id != schedule.ScheduleId) return NotFound();

            if (schedule.TourDate.HasValue && schedule.TourDate.Value.Date < System.DateTime.Now.Date)
            {
                ModelState.AddModelError("TourDate", "Ngày khởi hành không được nằm trong quá khứ.");
            }

            if (schedule.TourId > 0)
            {
                var tour = await _tourService.GetTourByIdAsync(schedule.TourId);
                if (tour != null && schedule.AvailableSlots > tour.MaxCapacity)
                {
                    ModelState.AddModelError("AvailableSlots", $"Số chỗ trống không được vượt quá số lượng tối đa của tour ({tour.MaxCapacity} chỗ).");
                }
            }

            if (schedule.GuideId.HasValue && schedule.TourDate.HasValue)
            {
                bool hasSchedule = await _scheduleService.HasScheduleOnDateAsync(schedule.GuideId.Value, schedule.TourDate.Value, schedule.ScheduleId);
                if (hasSchedule)
                {
                    ModelState.AddModelError("GuideId", "Hướng dẫn viên này đã có lịch trình vào ngày này. Mỗi hướng dẫn viên chỉ được nhận 1 tour/ngày.");
                }
            }

            if (ModelState.IsValid)
            {
                var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
                if (existingSchedule == null) return NotFound();

                existingSchedule.TourId = schedule.TourId;
                existingSchedule.TourDate = schedule.TourDate;
                existingSchedule.StartTime = schedule.StartTime;
                existingSchedule.AvailableSlots = schedule.AvailableSlots;
                existingSchedule.GuideId = schedule.GuideId;

                await _scheduleService.UpdateScheduleAsync(existingSchedule);
                TempData["SuccessMessage"] = "Cập nhật lịch trình tour thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            var allUsers = await _userService.GetAllUsersAsync();
            ViewBag.Guides = new SelectList(allUsers.Where(u => u.Role?.RoleName == "Tour Guide"), "UserId", "FullName", schedule.GuideId);
            return View(schedule);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetScheduleByIdAsync(id.Value);
            if (schedule == null) return NotFound();

            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _scheduleService.DeleteScheduleAsync(id);
                TempData["SuccessMessage"] = "Xóa lịch trình tour thành công!";
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] = "Không thể xóa lịch trình này vì đã có khách hàng đặt tour (có vé). Vui lòng hủy các vé liên quan trước.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
