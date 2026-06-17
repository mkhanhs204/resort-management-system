using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.Users;
using System.Security.Claims;
using System.Text;

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

        // GET: /Profile
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

            // Giải mã Passport từ byte[] sang string để hiển thị
            string? passportStr = null;
            if (user.UserProfile?.PassportNumber != null)
            {
                passportStr = Encoding.UTF8.GetString(user.UserProfile.PassportNumber);
            }

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
                Address = user.UserProfile?.Address
            };

            return View(viewModel);
        }

        // POST: /Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int currentUserId) || currentUserId != model.UserId)
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            // 1. Cập nhật bảng User
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            // 2. Kiểm tra và khởi tạo UserProfile nếu chưa tồn tại (One-to-One)
            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfile { UserId = user.UserId };
                _context.UserProfiles.Add(user.UserProfile);
            }

            // 3. Cập nhật thông tin chi tiết
            user.UserProfile.DateOfBirth = model.DateOfBirth;
            user.UserProfile.Nationality = model.Nationality;
            user.UserProfile.Address = model.Address;

            // Chuyển chuỗi hộ chiếu thành byte[] để lưu bảo mật vào DB
            if (!string.IsNullOrEmpty(model.PassportNumberString))
            {
                user.UserProfile.PassportNumber = Encoding.UTF8.GetBytes(model.PassportNumberString);
            }
            else
            {
                user.UserProfile.PassportNumber = null;
            }

            // 4. Xử lý File tải lên (Upload Passport Image) nếu có
            if (model.PassportImageFile != null && model.PassportImageFile.Length > 0)
            {
                // Tạo thư mục lưu trữ trong wwwroot/uploads/passports
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "passports");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Đổi tên file để tránh trùng lặp
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PassportImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PassportImageFile.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn tương đối vào database
                user.UserProfile.PassportImage = "/uploads/passports/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thông tin thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}