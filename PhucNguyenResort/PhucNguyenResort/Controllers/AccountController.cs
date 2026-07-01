using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Helpers;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Auth;
using PhucNguyenResort.Services.Email;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers
{
    public class AccountController : Controller
    {
        private readonly ResortDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(ResortDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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

            var existingUser = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser != null && existingUser.IsActive)
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

            User user;
            if (existingUser != null)
            {
                user = existingUser;
                user.FullName = model.FullName.Trim();
                user.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim();
                user.PasswordHash = PasswordHelper.HashPassword(user, model.Password);
                user.CreatedAt = DateTime.Now;

                if (user.UserProfile != null)
                {
                    user.UserProfile.DateOfBirth = model.DateOfBirth;
                    user.UserProfile.Nationality = string.IsNullOrWhiteSpace(model.Nationality) ? null : model.Nationality.Trim();
                    user.UserProfile.Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim();
                    user.UserProfile.ConsentGiven = model.ConsentGiven;
                }
                else
                {
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
                }
            }
            else
            {
                user = new User
                {
                    FullName = model.FullName.Trim(),
                    Email = email,
                    PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? null : model.PhoneNumber.Trim(),
                    RoleId = customerRole.RoleId,
                    IsActive = false, // Set to inactive until email is verified
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
            }

            // Generate OTP Verification Code
            var random = new Random();
            var otpCode = random.Next(100000, 999999).ToString();
            var emailOtp = new EmailOtp
            {
                Email = email,
                OtpCode = otpCode,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false,
                Attempts = 0,
                CreatedAt = DateTime.Now
            };
            _context.EmailOtps.Add(emailOtp);

            await _context.SaveChangesAsync();

            // Send Email OTP
            var subject = "Xác nhận tài khoản - PhucNguyen Resort";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
                    <h2 style='color: #d4a373; text-align: center;'>Phúc Nguyễn Resort</h2>
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Cảm ơn bạn đã đăng ký tài khoản tại Phúc Nguyễn Resort. Mã xác thực đăng ký tài khoản của bạn là:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #1d3557; background-color: #f1faee; padding: 10px 20px; border-radius: 5px; border: 1px dashed #d4a373;'>{otpCode}</span>
                    </div>
                    <p style='color: #e63946;'><strong>Lưu ý:</strong> Mã OTP này có hiệu lực trong vòng 5 phút. Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                    <p style='font-size: 12px; color: #888; text-align: center;'>Đây là email tự động, vui lòng không phản hồi email này.</p>
                </div>";

            try
            {
                await _emailSender.SendEmailAsync(email, subject, body);
            }
            catch (Exception)
            {
                // In a production system, we'd log the exception: ex.Message
                TempData["ErrorMessage"] = "Tài khoản đã được tạo nhưng không thể gửi mã xác nhận qua email. Vui lòng thử lại.";
            }

            return RedirectToAction(nameof(VerifyOtp), new { email = email });
        }

        [AllowAnonymous]
        public IActionResult VerifyOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Register));
            }
            return View(new VerifyOtpViewModel { Email = email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();
            var code = model.OtpCode.Trim();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                return View(model);
            }

            if (user.IsActive)
            {
                TempData["SuccessMessage"] = "Tài khoản đã được xác thực trước đó. Vui lòng đăng nhập.";
                return RedirectToAction(nameof(Login));
            }

            var latestOtp = await _context.EmailOtps
                .Where(o => o.Email == email && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (latestOtp == null || latestOtp.ExpiryTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(model.OtpCode), "Mã xác thực không đúng hoặc đã hết hạn.");
                return View(model);
            }

            if (latestOtp.Attempts >= 5)
            {
                ModelState.AddModelError(nameof(model.OtpCode), "Mã xác thực này đã bị khóa do nhập sai quá nhiều lần. Vui lòng nhấn gửi lại mã.");
                return View(model);
            }

            if (latestOtp.OtpCode != code)
            {
                latestOtp.Attempts++;
                await _context.SaveChangesAsync();
                ModelState.AddModelError(nameof(model.OtpCode), $"Mã xác thực không chính xác. Bạn còn {5 - latestOtp.Attempts} lần thử.");
                return View(model);
            }

            // Correct OTP
            latestOtp.IsUsed = true;
            user.IsActive = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xác thực tài khoản thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email))
            {
                return Json(new { success = false, message = "Email không hợp lệ." });
            }

            var email = model.Email.Trim();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Json(new { success = false, message = "Tài khoản không tồn tại." });
            }

            if (user.IsActive)
            {
                return Json(new { success = false, message = "Tài khoản đã được xác thực." });
            }

            // Check rate limit (last OTP generated < 60 seconds ago)
            var lastOtp = await _context.EmailOtps
                .Where(o => o.Email == email)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (lastOtp != null && (DateTime.Now - lastOtp.CreatedAt).TotalSeconds < 60)
            {
                var remaining = 60 - (int)(DateTime.Now - lastOtp.CreatedAt).TotalSeconds;
                return Json(new { success = false, message = $"Vui lòng đợi {remaining} giây trước khi gửi lại mã." });
            }

            // Create new OTP
            var random = new Random();
            var otpCode = random.Next(100000, 999999).ToString();
            var emailOtp = new EmailOtp
            {
                Email = email,
                OtpCode = otpCode,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false,
                Attempts = 0,
                CreatedAt = DateTime.Now
            };

            _context.EmailOtps.Add(emailOtp);
            await _context.SaveChangesAsync();

            var subject = "Gửi lại mã xác thực - PhucNguyen Resort";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
                    <h2 style='color: #d4a373; text-align: center;'>Phúc Nguyễn Resort</h2>
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Yêu cầu gửi lại mã xác thực đăng ký tài khoản của bạn đã được thực hiện. Mã xác thực mới là:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #1d3557; background-color: #f1faee; padding: 10px 20px; border-radius: 5px; border: 1px dashed #d4a373;'>{otpCode}</span>
                    </div>
                    <p style='color: #e63946;'><strong>Lưu ý:</strong> Mã OTP này có hiệu lực trong vòng 5 phút. Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                    <p style='font-size: 12px; color: #888; text-align: center;'>Đây là email tự động, vui lòng không phản hồi email này.</p>
                </div>";

            try
            {
                await _emailSender.SendEmailAsync(email, subject, body);
                return Json(new { success = true, message = "Mã xác thực mới đã được gửi thành công!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Không thể gửi email. Vui lòng thử lại sau." });
            }
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

            if (user.IsBlocked)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa bởi quản trị viên.");
                return View(model);
            }

            var isValidPassword = PasswordHelper.VerifyPassword(user, user.PasswordHash, model.Password);
            if (!isValidPassword)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            var roleName = user.Role?.RoleName ?? "Customer";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

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

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (roleName != "Customer")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");
            if (!result.Succeeded || result.Principal == null)
            {
                TempData["ErrorMessage"] = "Đăng nhập bằng Google thất bại.";
                return RedirectToAction(nameof(Login));
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Không thể lấy thông tin email từ tài khoản Google của bạn.";
                return RedirectToAction(nameof(Login));
            }

            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer")
                                   ?? await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == 3);

                if (customerRole == null)
                {
                    TempData["ErrorMessage"] = "Hệ thống gặp sự cố cấu hình phân quyền (Role).";
                    return RedirectToAction(nameof(Login));
                }

                user = new User
                {
                    FullName = name ?? "Google User",
                    Email = email,
                    RoleId = customerRole.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var profile = new UserProfile
                {
                    UserId = user.UserId,
                    ConsentGiven = true,
                    DeletionRequested = false
                };
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();

                user.Role = customerRole;
            }
            else if (!user.IsActive)
            {
                user.IsActive = true;
                await _context.SaveChangesAsync();
            }

            if (user.IsBlocked)
            {
                TempData["ErrorMessage"] = "Tài khoản của bạn đã bị khóa bởi quản trị viên.";
                return RedirectToAction(nameof(Login));
            }

            await HttpContext.SignOutAsync("ExternalCookie");

            var roleName = user.Role?.RoleName ?? "Customer";
            var identityClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var claimsIdentity = new ClaimsIdentity(identityClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(14)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !user.IsActive || user.IsBlocked)
            {
                // To prevent email enumeration, we just redirect to confirmation
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // Generate reset token
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            
            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.Now.AddHours(24);
            await _context.SaveChangesAsync();

            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
            
            string subject = "Đặt lại mật khẩu - Phúc Nguyên Resort";
            string body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 5px;'>
                    <h2 style='color: #d4a373; text-align: center;'>Phúc Nguyên Resort</h2>
                    <p>Xin chào <strong>{user.FullName}</strong>,</p>
                    <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. Vui lòng click vào nút bên dưới để tiến hành thiết lập mật khẩu mới (Link có hiệu lực trong 24 giờ):</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{callbackUrl}' style='display: inline-block; padding: 12px 25px; background-color: #1d3557; color: #ffffff; text-decoration: none; border-radius: 5px; font-weight: bold;'>ĐẶT LẠI MẬT KHẨU</a>
                    </div>
                    <p style='color: #e63946;'><strong>Lưu ý:</strong> Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này. Tài khoản của bạn vẫn an toàn.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                    <p style='font-size: 12px; color: #888; text-align: center;'>Đây là email tự động, vui lòng không phản hồi email này.</p>
                </div>";

            try 
            {
                await _emailSender.SendEmailAsync(model.Email, subject, body);
            } 
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Không thể gửi email lúc này. Vui lòng thử lại sau.");
                return View(model);
            }

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string? token, string? email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError(string.Empty, "Mã khôi phục hoặc email không hợp lệ.");
                return View();
            }
            
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            if (user.ResetPasswordToken != model.Token || user.ResetPasswordTokenExpiry == null || user.ResetPasswordTokenExpiry < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Đường link đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
                return View(model);
            }

            user.PasswordHash = PasswordHelper.HashPassword(user, model.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}