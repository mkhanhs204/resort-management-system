using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.RoomCategories;
using PhucNguyenResort.ViewModels.RoomCategories;
using System.IO;

namespace PhucNguyenResort.Controllers
{
    public class RoomCategoriesController : Controller
    {
        private readonly IRoomCategoryService _roomCategoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomCategoriesController(IRoomCategoryService roomCategoryService, IWebHostEnvironment webHostEnvironment)
        {
            _roomCategoryService = roomCategoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: RoomCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _roomCategoryService.GetAllAsync();
            return View(categories);
        }

        // GET: RoomCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _roomCategoryService.GetByIdAsync(id.Value);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        // GET: RoomCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomCategoryCreateVM model)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                if (model.ImageUpload != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms");
                    Directory.CreateDirectory(uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(fileStream);
                    }
                }

                var roomCategory = new RoomCategory
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    MaxAdults = model.MaxAdults,
                    MaxChildren = model.MaxChildren,
                    PricePerNight = model.PricePerNight,
                    ImageUrl = uniqueFileName != null ? "/images/rooms/" + uniqueFileName : null
                };

                // Process Gallery Images
                if (model.GalleryUploads != null && model.GalleryUploads.Count > 0)
                {
                    string galleryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms", "gallery");
                    Directory.CreateDirectory(galleryFolder);

                    foreach (var file in model.GalleryUploads)
                    {
                        if (file.Length > 0)
                        {
                            string galleryFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string galleryFilePath = Path.Combine(galleryFolder, galleryFileName);
                            using (var stream = new FileStream(galleryFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            roomCategory.Images.Add(new RoomCategoryImage
                            {
                                ImageUrl = "/images/rooms/gallery/" + galleryFileName
                            });
                        }
                    }
                }

                await _roomCategoryService.AddAsync(roomCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: RoomCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _roomCategoryService.GetByIdAsync(id.Value);
            if (roomCategory == null)
            {
                return NotFound();
            }

            var model = new RoomCategoryEditVM
            {
                CategoryId = roomCategory.CategoryId,
                CategoryName = roomCategory.CategoryName,
                Description = roomCategory.Description,
                MaxAdults = roomCategory.MaxAdults,
                MaxChildren = roomCategory.MaxChildren,
                PricePerNight = roomCategory.PricePerNight,
                ImageUrl = roomCategory.ImageUrl,
                ExistingGalleryImages = roomCategory.Images.ToList()
            };

            return View(model);
        }

        // POST: RoomCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomCategoryEditVM model)
        {
            if (id != model.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var roomCategory = await _roomCategoryService.GetByIdAsync(id);
                if (roomCategory == null)
                {
                    return NotFound();
                }

                if (model.ImageUpload != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUpload.CopyToAsync(fileStream);
                    }

                    // Delete old image if it exists and it's from our uploads
                    if (!string.IsNullOrEmpty(roomCategory.ImageUrl))
                    {
                        var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, roomCategory.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    roomCategory.ImageUrl = "/images/rooms/" + uniqueFileName;
                }

                roomCategory.CategoryName = model.CategoryName;
                roomCategory.Description = model.Description;
                roomCategory.MaxAdults = model.MaxAdults;
                roomCategory.MaxChildren = model.MaxChildren;
                roomCategory.PricePerNight = model.PricePerNight;

                // Process Gallery Images
                if (model.GalleryUploads != null && model.GalleryUploads.Count > 0)
                {
                    string galleryFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms", "gallery");
                    Directory.CreateDirectory(galleryFolder);

                    foreach (var file in model.GalleryUploads)
                    {
                        if (file.Length > 0)
                        {
                            string galleryFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string galleryFilePath = Path.Combine(galleryFolder, galleryFileName);
                            using (var stream = new FileStream(galleryFilePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            roomCategory.Images.Add(new RoomCategoryImage
                            {
                                ImageUrl = "/images/rooms/gallery/" + galleryFileName
                            });
                        }
                    }
                }

                await _roomCategoryService.UpdateAsync(roomCategory);
                return RedirectToAction(nameof(Index));
            }
            
            // Reload existing images on validation failure
            var reloadCategory = await _roomCategoryService.GetByIdAsync(id);
            if (reloadCategory != null)
            {
                model.ExistingGalleryImages = reloadCategory.Images.ToList();
            }
            return View(model);
        }

        // POST: RoomCategories/DeleteGalleryImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGalleryImage(int id)
        {
            var image = await _roomCategoryService.GetGalleryImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var categoryId = image.CategoryId;

            // Delete physical file
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await _roomCategoryService.DeleteGalleryImageAsync(id);

            return RedirectToAction(nameof(Edit), new { id = categoryId });
        }

        // GET: RoomCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomCategory = await _roomCategoryService.GetByIdAsync(id.Value);
            if (roomCategory == null)
            {
                return NotFound();
            }

            return View(roomCategory);
        }

        // POST: RoomCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomCategory = await _roomCategoryService.GetByIdAsync(id);
            if (roomCategory != null)
            {
                await _roomCategoryService.DeleteAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
