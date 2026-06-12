using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.ViewModels.Tours;

namespace PhucNguyenResort.Controllers
{
    /// <summary>
    /// ToursController — Manage Tour Catalog (UC04 subset).
    /// All POST actions protected by [ValidateAntiForgeryToken] (OWASP CSRF).
    /// </summary>
    public class ToursController : Controller
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllToursAsync();
            return View(tours);
        }

        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourService.GetTourByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: Tours/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TourCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var tour = new Tour
                {
                    TourName      = model.TourName,
                    Description   = model.Description,
                    Price         = model.Price,
                    MaxCapacity   = model.MaxCapacity,
                    DurationHours = model.DurationHours,
                    Location      = model.Location,
                    ImageUrl      = model.ImageUrl
                };

                await _tourService.AddTourAsync(tour);
                TempData["SuccessMessage"] = $"Tour \"{tour.TourName}\" has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourService.GetTourByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }

            var model = new TourEditVM
            {
                TourId        = tour.TourId,
                TourName      = tour.TourName ?? "",
                Description   = tour.Description,
                Price         = tour.Price ?? 0,
                MaxCapacity   = tour.MaxCapacity ?? 0,
                DurationHours = tour.DurationHours ?? 0,
                Location      = tour.Location,
                ImageUrl      = tour.ImageUrl
            };

            return View(model);
        }

        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TourEditVM model)
        {
            if (id != model.TourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                if (tour == null)
                {
                    return NotFound();
                }

                tour.TourName      = model.TourName;
                tour.Description   = model.Description;
                tour.Price         = model.Price;
                tour.MaxCapacity   = model.MaxCapacity;
                tour.DurationHours = model.DurationHours;
                tour.Location      = model.Location;
                tour.ImageUrl      = model.ImageUrl;

                await _tourService.UpdateTourAsync(tour);
                TempData["SuccessMessage"] = $"Tour \"{tour.TourName}\" has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourService.GetTourByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }

            // Pass schedule count so the view can warn Admin if dependencies exist
            ViewBag.ScheduleCount = tour.TourSchedules?.Count ?? 0;

            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tour = await _tourService.GetTourByIdAsync(id);
                if (tour == null)
                {
                    return NotFound();
                }

                string tourName = tour.TourName ?? $"Tour #{id}";
                await _tourService.DeleteTourAsync(id);
                TempData["SuccessMessage"] = $"Tour \"{tourName}\" has been deleted successfully.";
            }
            catch (InvalidOperationException ex) when (ex.Message.StartsWith("TOUR-003"))
            {
                // BR-TOUR-005: Tour has active TourSchedules — refuse delete
                TempData["ErrorMessage"] = "Cannot delete this tour because it has active schedules (TourSchedules). Please remove all schedules first.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
