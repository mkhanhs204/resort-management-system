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
    [Authorize(Roles = "System Admin,Business Manager")]
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
        public async Task<IActionResult> Index()
        {
            var staff = await _userService.GetAllStaffAsync();
            return View(staff);
        }

        private async Task PopulateRolesDropDownList(object? selectedRole = null)
        {
            var rolesQuery = _context.Roles.Where(r => r.RoleId != 3).OrderBy(r => r.RoleName);
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

            var user = await _userService.GetStaffByIdAsync(id.Value);
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

                var userToUpdate = await _userService.GetStaffByIdAsync(id);
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
            var user = await _userService.GetStaffByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.ToggleStaffStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
