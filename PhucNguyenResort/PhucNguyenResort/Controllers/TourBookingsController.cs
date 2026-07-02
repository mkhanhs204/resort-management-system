using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhucNguyenResort.Services.Users;

namespace PhucNguyenResort.Controllers
{
    public class TourBookingsController : Controller
    {
        private readonly ITourBookingService _bookingService;
        private readonly ITourScheduleService _scheduleService;
        private readonly IUserService _userService;

        public TourBookingsController(ITourBookingService bookingService, ITourScheduleService scheduleService, IUserService userService)
        {
            _bookingService = bookingService;
            _scheduleService = scheduleService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _bookingService.GetBookingByIdAsync(id.Value);
            if (booking == null) return NotFound();

            return View(booking);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Schedules = new SelectList(await _scheduleService.GetAllSchedulesAsync(), "ScheduleId", "ScheduleId");
            ViewBag.Users = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourBooking booking)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookingService.AddBookingAsync(booking);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.Schedules = new SelectList(await _scheduleService.GetAllSchedulesAsync(), "ScheduleId", "ScheduleId", booking.ScheduleId);
            ViewBag.Users = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", booking.UserId);
            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _bookingService.GetBookingByIdAsync(id.Value);
            if (booking == null) return NotFound();

            ViewBag.Schedules = new SelectList(await _scheduleService.GetAllSchedulesAsync(), "ScheduleId", "ScheduleId", booking.ScheduleId);
            ViewBag.Users = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", booking.UserId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourBooking booking)
        {
            if (id != booking.TourBookingId) return NotFound();

            if (ModelState.IsValid)
            {
                await _bookingService.UpdateBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Schedules = new SelectList(await _scheduleService.GetAllSchedulesAsync(), "ScheduleId", "ScheduleId", booking.ScheduleId);
            ViewBag.Users = new SelectList(await _userService.GetAllUsersAsync(), "UserId", "FullName", booking.UserId);
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _bookingService.GetBookingByIdAsync(id.Value);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
