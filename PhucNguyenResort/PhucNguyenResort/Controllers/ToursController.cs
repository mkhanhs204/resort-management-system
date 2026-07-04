using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.ViewModels.Tours;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace PhucNguyenResort.Controllers
{
    /// <summary>
    /// ToursController — Manage Tour Catalog (UC04 subset).
    /// All POST actions protected by [ValidateAntiForgeryToken] (OWASP CSRF).
    /// </summary>
    [Authorize(Roles = "Business Manager")]
    public class ToursController : Controller
    {
        private readonly ITourCatalogService _tourService;
        private readonly ResortDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ToursController(ITourCatalogService tourService, ResortDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _tourService = tourService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create()
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
                    Location      = model.Location
                };

                // Process Main Image
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tours");
                    Directory.CreateDirectory(imagesFolder);
                    string fileName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(imagesFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(stream);
                    }
                    tour.ImageUrl = "/images/tours/" + fileName;
                }
                else
                {
                    tour.ImageUrl = model.ImageUrl;
                }

                // Process Gallery Images
                if (model.GalleryImages != null && model.GalleryImages.Count > 0)
                {
                    string galleryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tours", "gallery");
                    Directory.CreateDirectory(galleryFolder);

                    foreach (var file in model.GalleryImages)
                    {
                        if (file.Length > 0)
                        {
                            string galleryFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string galleryFilePath = Path.Combine(galleryFolder, galleryFileName);
                            using (var stream = new FileStream(galleryFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tour.Images.Add(new TourImage
                            {
                                ImageUrl = "/images/tours/gallery/" + galleryFileName
                            });
                        }
                    }
                }

                await _tourService.AddTourAsync(tour);
                if (TempData != null) TempData["SuccessMessage"] = $"Tạo tour \"{tour.TourName}\" thành công.";
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
                ImageUrl      = tour.ImageUrl,
                ExistingGalleryImages = tour.Images.Select(i => new TourImageVM
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl
                }).ToList()
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

                // Process Main Image
                if (model.ImageUpload != null && model.ImageUpload.Length > 0)
                {
                    string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tours");
                    Directory.CreateDirectory(imagesFolder);
                    string fileName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(imagesFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(stream);
                    }
                    tour.ImageUrl = "/images/tours/" + fileName;
                }
                else if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    tour.ImageUrl = model.ImageUrl;
                }

                // Process Gallery Images
                if (model.NewGalleryImages != null && model.NewGalleryImages.Count > 0)
                {
                    string galleryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tours", "gallery");
                    Directory.CreateDirectory(galleryFolder);

                    foreach (var file in model.NewGalleryImages)
                    {
                        if (file.Length > 0)
                        {
                            string galleryFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string galleryFilePath = Path.Combine(galleryFolder, galleryFileName);
                            using (var stream = new FileStream(galleryFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tour.Images.Add(new TourImage
                            {
                                ImageUrl = "/images/tours/gallery/" + galleryFileName
                            });
                        }
                    }
                }

                await _tourService.UpdateTourAsync(tour);
                if (TempData != null) TempData["SuccessMessage"] = $"Cập nhật tour \"{tour.TourName}\" thành công.";
                return RedirectToAction(nameof(Index));
            }
            
            // Reload existing images on validation failure
            var reloadTour = await _tourService.GetTourByIdAsync(id);
            if (reloadTour != null)
            {
                model.ExistingGalleryImages = reloadTour.Images.Select(i => new TourImageVM
                {
                    ImageId = i.ImageId,
                    ImageUrl = i.ImageUrl
                }).ToList();
            }
            return View(model);
        }

        // POST: Tours/DeleteGalleryImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGalleryImage(int id)
        {
            var image = await _tourService.GetGalleryImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var tourId = image.TourId;

            // Delete physical file
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _tourService.DeleteGalleryImageAsync(id);

            return RedirectToAction(nameof(Edit), new { id = tourId });
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
                    return RedirectToAction(nameof(Index));
                }

                string tourName = tour.TourName ?? $"Tour #{id}";
                await _tourService.DeleteTourAsync(id);
                if (TempData != null) TempData["SuccessMessage"] = $"Xóa tour \"{tourName}\" thành công.";
            }
            catch (InvalidOperationException ex) when (ex.Message.StartsWith("TOUR-003"))
            {
                // BR-TOUR-005: Tour has active TourSchedules — refuse delete
                if (TempData != null) TempData["ErrorMessage"] = "Không thể xóa tour này vì đang có lịch trình (TourSchedules) hoạt động. Vui lòng xóa hết lịch trình trước.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Tours/CheckTicket?ticketId=123
        [HttpGet]
        public async Task<IActionResult> CheckTicket(int ticketId)
        {
            var ticket = await _context.TourBookings
                .Include(tb => tb.User)
                .Include(tb => tb.Schedule)
                    .ThenInclude(s => s.Tour)
                .FirstOrDefaultAsync(tb => tb.TourBookingId == ticketId);

            if (ticket == null)
            {
                return NotFound("Không tìm thấy mã vé này trong hệ thống.");
            }

            return View(ticket);
        }
    }
}
