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

            // 2. Truy vấn dữ liệu từ DB theo ID tài khoản vừa lấy động
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == currentUserId);

            if (user == null)
            {
                return NotFound("Không tìm thấy dữ liệu tài khoản trong hệ thống.");
            }

            // 3. Giải mã Passport từ dạng byte[] sang chuỗi kí tự (string) hiển thị lên Form
            string? passportStr = null;
            if (user.UserProfile?.PassportNumber != null)
            {
                passportStr = Encoding.UTF8.GetString(user.UserProfile.PassportNumber);
            }

            // 4. Gán toàn bộ dữ liệu vào ViewModel để ném sang trang hiển thị
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
            // Bảo mật: Đảm bảo người dùng không cố tình sửa UserId ẩn (Hidden Field) trên Form để phá tài khoản người khác
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || model.UserId.ToString() != userIdClaim.Value)
            {
                return Forbid("Hành động không hợp lệ. Bạn không có quyền chỉnh sửa hồ sơ này.");
            }

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
                return NotFound("Tài khoản cần cập nhật không tồn tại.");
            }

            // 1. Cập nhật thông tin bảng lõi User
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            // 2. Đề phòng tài khoản mới tinh chưa từng nhập hồ sơ chi tiết (UserProfile trống)
            if (user.UserProfile == null)
            {
                user.UserProfile = new UserProfile { UserId = user.UserId };
                _context.UserProfiles.Add(user.UserProfile);
            }

            // 3. Cập nhật thông tin bổ sung sang bảng vệ tinh UserProfile
            user.UserProfile.DateOfBirth = model.DateOfBirth;
            user.UserProfile.Nationality = model.Nationality;
            user.UserProfile.Address = model.Address;

            // Đóng gói chuỗi Hộ chiếu nhập từ form thành mảng byte[] bảo mật trước khi cất vào DB
            if (!string.IsNullOrEmpty(model.PassportNumberString))
            {
                user.UserProfile.PassportNumber = Encoding.UTF8.GetBytes(model.PassportNumberString);
            }
            else
            {
                user.UserProfile.PassportNumber = null;
            }

            // 4. Xử lý logic tải ảnh lên của hệ thống (Upload File)
            if (model.PassportImageFile != null && model.PassportImageFile.Length > 0)
            {
                // Chỉ định thư mục lưu trữ thực tế trên máy chủ
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "passports");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Gán mã định danh duy nhất Guid để tránh tình trạng trùng lặp tên file giữa các khách hàng
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PassportImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PassportImageFile.CopyToAsync(fileStream);
                }

                // Cập nhật đường dẫn URL truy cập ảnh mới vào cơ sở dữ liệu
                user.UserProfile.PassportImage = "/uploads/passports/" + uniqueFileName;
            }

            // 5. Đồng bộ hóa và lưu toàn bộ thay đổi xuống Database
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thông tin thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}