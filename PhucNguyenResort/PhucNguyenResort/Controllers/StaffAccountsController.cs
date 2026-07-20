using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Users;
using PhucNguyenResort.ViewModels.Staff;
using PhucNguyenResort.Helpers;
using Microsoft.AspNetCore.Authorization;
namespace PhucNguyenResort.Controllers
{
    [Authorize(Roles = "System Admin")]
    public class StaffAccountsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ResortDbContext _context;

        public StaffAccountsController(IUserService userService, ResortDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        // GET: StaffAccounts
        public async Task<IActionResult> Index(string? name, int? roleId, string? status, int page = 1)
        {
            var query = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.FullName.Contains(name) || u.Email.Contains(name));
            }

            if (roleId.HasValue && roleId > 0)
            {
                query = query.Where(u => u.RoleId == roleId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "verified") query = query.Where(u => u.IsActive && !u.IsBlocked);
                else if (status == "unverified") query = query.Where(u => !u.IsActive);
                else if (status == "blocked") query = query.Where(u => u.IsBlocked);
            }

            int totalItems = await query.CountAsync();
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var users = await query
                .OrderByDescending(u => u.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            await PopulateRolesDropDownList(roleId);
            ViewBag.CurrentName = name;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(users);
        }

        private async Task PopulateRolesDropDownList(object? selectedRole = null)
        {
            var rolesQuery = _context.Roles.OrderBy(r => r.RoleName);
            ViewBag.RoleId = new SelectList(await rolesQuery.AsNoTracking().ToListAsync(), "RoleId", "RoleName", selectedRole);
        }

        // GET: StaffAccounts/Create
        public async Task<IActionResult> Create()
        {
            await PopulateRolesDropDownList();
            return View();
        }

        // POST: StaffAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffCreateVM model)
        {
            if (ModelState.IsValid)
            {
                // Simple check for duplicate email
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    await PopulateRolesDropDownList(model.RoleId);
                    return View(model);
                }

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    RoleId = model.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                user.PasswordHash = PasswordHelper.HashPassword(user, model.Password);

                await _userService.AddStaffAsync(user);
                TempData["SuccessMessage"] = "Tạo tài khoản mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateRolesDropDownList(model.RoleId);
            return View(model);
        }

        // GET: StaffAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            var model = new StaffEditVM
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId
            };

            await PopulateRolesDropDownList(user.RoleId);
            return View(model);
        }

        // POST: StaffAccounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StaffEditVM model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Simple check for duplicate email excluding current user
                if (await _context.Users.AnyAsync(u => u.Email == model.Email && u.UserId != id))
                {
                    ModelState.AddModelError("Email", "This email is already in use by another user.");
                    await PopulateRolesDropDownList(model.RoleId);
                    return View(model);
                }

                var userToUpdate = await _userService.GetUserByIdAsync(id);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                userToUpdate.FullName = model.FullName;
                userToUpdate.Email = model.Email;
                userToUpdate.PhoneNumber = model.PhoneNumber;
                userToUpdate.RoleId = model.RoleId;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    userToUpdate.PasswordHash = PasswordHelper.HashPassword(userToUpdate, model.Password);
                }

                await _userService.UpdateStaffAsync(userToUpdate);
                TempData["SuccessMessage"] = "Cập nhật thông tin tài khoản thành công!";
                return RedirectToAction(nameof(Index));
            }
            await PopulateRolesDropDownList(model.RoleId);
            return View(model);
        }

        // POST: StaffAccounts/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.ToggleUserStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: StaffAccounts/ToggleBlockStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBlockStatus(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.ToggleUserBlockStatusAsync(id);
            if (user.IsBlocked)
            {
                TempData["SuccessMessage"] = "Khóa tài khoản thành công!";
                TempData["MessageType"] = "danger";
            }
            else
            {
                TempData["SuccessMessage"] = "Mở khóa tài khoản thành công!";
                TempData["MessageType"] = "success";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
