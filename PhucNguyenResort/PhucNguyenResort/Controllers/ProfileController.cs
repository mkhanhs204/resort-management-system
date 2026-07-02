using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Users;
using System.Security.Claims;
using System.Text;
using PhucNguyenResort.Helpers;

namespace PhucNguyenResort.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ResortDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(ResortDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentUserId))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == currentUserId);

            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin tài khoản.");
            }

            string? passportStr = null;
            if (user.UserProfile?.PassportNumber != null)
            {
                passportStr = Encoding.UTF8.GetString(user.UserProfile.PassportNumber);
            }

            var checkedInBookings = await _context.Bookings
                .AsNoTracking()
                .Include(booking => booking.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .Where(booking =>
                    booking.UserId == currentUserId
                    && booking.BookingStatus == BookingStatus.CheckedIn)
                .OrderByDescending(booking => booking.CheckInDate)
                .Select(booking => new ProfileBookingVM
                {
                    BookingId = booking.BookingId,
                    RoomNumber = booking.BookingDetails
                        .Where(bd => bd.Room != null && bd.Room.RoomNumber != null)
                        .Select(bd => bd.Room!.RoomNumber!)
                        .FirstOrDefault() ?? string.Empty,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate
                })
                .ToListAsync();

            var viewModel = new ProfileVM
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.UserProfile?.DateOfBirth,
                Nationality = user.UserProfile?.Nationality,
                PassportNumberString = passportStr,
                PassportImage = user.UserProfile?.PassportImage,
                Address = user.UserProfile?.Address,
                CheckedInBookings = checkedInBookings
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileVM model)
        {
            if (!TryGetCurrentUserId(out var currentUserId))
            {
                return Challenge();
            }

            if (model.UserId != currentUserId)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                model.CheckedInBookings = await GetCheckedInBookingsAsync(currentUserId);
                return View("Index", model);
            }


            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == currentUserId);

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfile { UserId = user.UserId };
                _context.UserProfiles.Add(user.UserProfile);
            }

            user.UserProfile.DateOfBirth = model.DateOfBirth;
            user.UserProfile.Nationality = model.Nationality;
            user.UserProfile.Address = model.Address;

            if (!string.IsNullOrEmpty(model.PassportNumberString))
            {
                user.UserProfile.PassportNumber = Encoding.UTF8.GetBytes(model.PassportNumberString);
            }
            else
            {
                user.UserProfile.PassportNumber = null;
            }

            if (model.PassportImageFile != null && model.PassportImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "passports");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PassportImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PassportImageFile.CopyToAsync(fileStream);
                }

                user.UserProfile.PassportImage = "/uploads/passports/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thông tin thành công!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<ProfileBookingVM>> GetCheckedInBookingsAsync(int currentUserId)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(booking => booking.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .Where(booking =>
                    booking.UserId == currentUserId
                    && booking.BookingStatus == BookingStatus.CheckedIn)
                .OrderByDescending(booking => booking.CheckInDate)
                .Select(booking => new ProfileBookingVM
                {
                    BookingId = booking.BookingId,
                    RoomNumber = booking.BookingDetails
                        .Where(bd => bd.Room != null && bd.Room.RoomNumber != null)
                        .Select(bd => bd.Room!.RoomNumber!)
                        .FirstOrDefault() ?? string.Empty,
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate
                })
                .ToListAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (!TryGetCurrentUserId(out var currentUserId))
            {
                return Challenge();
            }

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Thông tin mật khẩu không hợp lệ hoặc mật khẩu xác nhận không khớp.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Tài khoản của bạn đăng nhập qua Google, không thể đổi mật khẩu.";
                return RedirectToAction(nameof(Index));
            }

            var isValidPassword = PasswordHelper.VerifyPassword(user, user.PasswordHash, currentPassword);
            if (!isValidPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng.";
                return RedirectToAction(nameof(Index));
            }

            user.PasswordHash = PasswordHelper.HashPassword(user, newPassword);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool TryGetCurrentUserId(out int currentUserId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out currentUserId);
        }
    }
}
