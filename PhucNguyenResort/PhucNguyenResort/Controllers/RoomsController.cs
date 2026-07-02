using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Rooms;
using PhucNguyenResort.Services.RoomCategories;
using PhucNguyenResort.ViewModels.Rooms;
using Microsoft.AspNetCore.Authorization;

namespace PhucNguyenResort.Controllers
{
    [Authorize(Roles = "Business Manager, Receptionist, Housekeeping Staff")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _roomCategoryService;

        public RoomsController(IRoomService roomService, IRoomCategoryService roomCategoryService)
        {
            _roomService = roomService;
            _roomCategoryService = roomCategoryService;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string searchString, RoomStatus? statusFilter, int? categoryId, int page = 1)
        {
            int pageSize = 10;
            var rooms = await _roomService.GetAllRoomsAsync();
            var query = rooms.AsEnumerable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r => (r.RoomNumber != null && r.RoomNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase)) || 
                                         r.FloorNumber.ToString() == searchString);
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(r => r.Status == statusFilter.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(r => r.CategoryId == categoryId.Value);
            }

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages && totalPages > 0 ? totalPages : page;

            var pagedRooms = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var categories = await _roomCategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", categoryId);
            ViewBag.SearchString = searchString;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedRooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Create()
        {
            var categories = await _roomCategoryService.GetAllAsync();
            var model = new RoomCreateVM
            {
                CategoriesList = new SelectList(categories, "CategoryId", "CategoryName")
            };
            return View(model);
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Create(RoomCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    RoomNumber = model.RoomNumber,
                    CategoryId = model.CategoryId,
                    FloorNumber = model.FloorNumber,
                    Status = RoomStatus.Available // Default status
                };

                await _roomService.AddRoomAsync(room);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _roomCategoryService.GetAllAsync();
            model.CategoriesList = new SelectList(categories, "CategoryId", "CategoryName", model.CategoryId);
            return View(model);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            var categories = await _roomCategoryService.GetAllAsync();
            var model = new RoomEditVM
            {
                RoomId = room.RoomId,
                RoomNumber = room.RoomNumber ?? "",
                CategoryId = room.CategoryId,
                FloorNumber = room.FloorNumber,
                Status = room.Status,
                CategoriesList = new SelectList(categories, "CategoryId", "CategoryName", room.CategoryId)
            };

            return View(model);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomEditVM model)
        {
            if (id != model.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var room = await _roomService.GetRoomByIdAsync(id);
                if (room == null)
                {
                    return NotFound();
                }

                room.RoomNumber = model.RoomNumber;
                room.CategoryId = model.CategoryId;
                room.FloorNumber = model.FloorNumber;
                room.Status = model.Status;

                await _roomService.UpdateRoomAsync(room);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _roomCategoryService.GetAllAsync();
            model.CategoriesList = new SelectList(categories, "CategoryId", "CategoryName", model.CategoryId);
            return View(model);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetRoomByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Business Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room != null)
            {
                await _roomService.DeleteRoomAsync(id);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
