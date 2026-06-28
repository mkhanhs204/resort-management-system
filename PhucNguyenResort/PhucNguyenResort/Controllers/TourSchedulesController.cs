using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhucNguyenResort.Services.Users;
using System.Linq;

namespace PhucNguyenResort.Controllers
{
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

        public async Task<IActionResult> Create()
        {
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName");
            ViewBag.Guides = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourSchedule schedule)
        {
            if (ModelState.IsValid)
            {
                await _scheduleService.AddScheduleAsync(schedule);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            ViewBag.Guides = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", schedule.GuideId);
            return View(schedule);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var schedule = await _scheduleService.GetScheduleByIdAsync(id.Value);
            if (schedule == null) return NotFound();

            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            ViewBag.Guides = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", schedule.GuideId);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourSchedule schedule)
        {
            if (id != schedule.ScheduleId) return NotFound();

            if (ModelState.IsValid)
            {
                await _scheduleService.UpdateScheduleAsync(schedule);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tours = new SelectList(await _tourService.GetAllToursAsync(), "TourId", "TourName", schedule.TourId);
            ViewBag.Guides = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", schedule.GuideId);
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
            await _scheduleService.DeleteScheduleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
