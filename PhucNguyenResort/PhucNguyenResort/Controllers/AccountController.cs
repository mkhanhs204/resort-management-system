using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Helpers;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Auth;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers
{
    public class AccountController : Controller
    {
        private readonly ResortDbContext _context;

        public AccountController(ResortDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();

            var emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Email này đã được sử dụng.");
                return View(model);
            }

            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer")
                               ?? await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 3);

            if (customerRole == null)
            {
                ModelState.AddModelError(string.Empty, "Chưa tìm thấy role Customer trong database.");
                return View(model);
            }

            var user = new User
            {
                FullName = model.FullName.Trim(),
                Email = email,
                PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim(),
                RoleId = customerRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            user.PasswordHash = PasswordHelper.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var profile = new UserProfile
            {
                UserId = user.UserId,
                DateOfBirth = model.DateOfBirth,
                Nationality = string.IsNullOrWhiteSpace(model.Nationality) ? null : model.Nationality.Trim(),
                Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim(),
                ConsentGiven = model.ConsentGiven,
                DeletionRequested = false
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            var isValidPassword = PasswordHelper.VerifyPassword(user, user.PasswordHash, model.Password);
            if (!isValidPassword)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            // Lấy Tên Role và chuẩn hóa chuỗi (Xóa khoảng trắng thừa)
            var roleName = (user.Role?.RoleName ?? "Customer").Trim();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email), // Để User.Identity.Name hiển thị Email giống hình bạn chụp ban đầu
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName) // Nạp Role gốc từ DB
            };

            // 🔥 ĐỒNG BỘ NÂNG CAO: Kiểm tra không phân biệt hoa thường, dấu cách để nạp full biến thể Role Tour Guide
            var normalizedRole = roleName.ToLower().Replace(" ", "");
            if (normalizedRole == "tourguide" || normalizedRole == "huongdanvien")
            {
                // Thêm đầy đủ 3 kiểu viết phổ biến để Layout và Controller nào cũng khớp quyền thành công
                if (!roleName.Equals("Tour Guide", StringComparison.OrdinalIgnoreCase))
                    claims.Add(new Claim(ClaimTypes.Role, "Tour Guide"));

                if (!roleName.Equals("TourGuide", StringComparison.OrdinalIgnoreCase))
                    claims.Add(new Claim(ClaimTypes.Role, "TourGuide"));

                if (!roleName.Equals("Hướng Dẫn Viên", StringComparison.OrdinalIgnoreCase))
                    claims.Add(new Claim(ClaimTypes.Role, "Hướng Dẫn Viên"));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(14)
                    : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Xử lý chuyển hướng an toàn sau khi Login thành công
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (!roleName.Equals("Customer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}