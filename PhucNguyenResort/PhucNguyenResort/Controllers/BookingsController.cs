using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.ViewModels.Payments;
using System.Security.Claims;
using PhucNguyenResort.Models;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ResortDbContext _context;
        private readonly IVnPayService _vnPayService;
        private readonly IBookingGuestService _bookingGuestService;

        public BookingsController(IBookingService bookingService, ResortDbContext context, IVnPayService vnPayService, IBookingGuestService bookingGuestService)
        {
            _bookingService = bookingService;
            _context = context;
            _vnPayService = vnPayService;
            _bookingGuestService = bookingGuestService;
        }

        // GET: /Bookings/Create
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(int categoryId, string? checkIn, string? checkOut, int adults = 2, int children = 0)
        {
            string[] formats = { "dd/MM/yyyy", "dd MM, yyyy", "d MMMM, yyyy", "dd MMMM, yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };
            DateTime checkInDate = DateTime.Today.AddDays(1);
            DateTime checkOutDate = DateTime.Today.AddDays(2);

            if (!string.IsNullOrEmpty(checkIn) && DateTime.TryParseExact(checkIn, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedCheckIn))
            {
                checkInDate = parsedCheckIn;
            }
            if (!string.IsNullOrEmpty(checkOut) && DateTime.TryParseExact(checkOut, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedCheckOut))
            {
                checkOutDate = parsedCheckOut;
            }

            var bookedRoomIds = await _context.Bookings
                .Where(b => b.BookingStatus != PhucNguyenResort.Enums.BookingStatus.Cancelled && b.BookingStatus != PhucNguyenResort.Enums.BookingStatus.CheckedOut)
                .Where(b => b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate)
                .Select(b => b.RoomId)
                .ToListAsync();

            var room = await _context.Rooms
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.CategoryId == categoryId 
                                     && r.Status != PhucNguyenResort.Enums.RoomStatus.Maintenance 
                                     && !bookedRoomIds.Contains(r.RoomId));

            if (room == null || room.Category == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin phòng.";
                return RedirectToAction("SearchRooms", "Home");
            }

            // Get upcoming tour schedules for add-ons
            var upcomingTours = await _context.TourSchedules
                .Include(ts => ts.Tour)
                .Where(ts => ts.TourDate != null && ts.TourDate >= DateTime.Today)
                .OrderBy(ts => ts.TourDate)
                .ToListAsync();

            ViewBag.UpcomingTours = upcomingTours;
            ViewBag.Room = room;

            var model = new CreateBookingViewModel
            {
                RoomId = room.RoomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                NumberOfAdults = adults,
                NumberOfChildren = children
            };

            return View(model);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel model)
        {
            var room = await _context.Rooms
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.RoomId == model.RoomId);

            if (room == null)
            {
                return NotFound();
            }

            if (!model.PdpdConsentChecked)
            {
                ModelState.AddModelError(nameof(model.PdpdConsentChecked), "Bạn phải đồng ý với điều khoản bảo vệ dữ liệu cá nhân (PDPD).");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Room = room;
                ViewBag.UpcomingTours = await _context.TourSchedules
                    .Include(ts => ts.Tour)
                    .Where(ts => ts.TourDate != null && ts.TourDate >= DateTime.Today)
                    .OrderBy(ts => ts.TourDate)
                    .ToListAsync();
                return View(model);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CreateBookingAsync(model, userId);

            if (result.Success)
            {
                // Save primary guest details for stay registration (Law on Residence 2020 & PDPD Compliance)
                var primaryGuest = new BookingGuestDto
                {
                    FullName = model.PrimaryGuestName,
                    DateOfBirth = model.PrimaryGuestDateOfBirth,
                    Gender = model.PrimaryGuestGender,
                    PassportNumber = model.PrimaryGuestPassportNumber,
                    Nationality = model.PrimaryGuestNationality,
                    IsPrimaryBooker = true
                };

                await _bookingGuestService.SaveBookingGuestsAsync(result.BookingId!.Value, new[] { primaryGuest }, model.PdpdConsentChecked);

                // Lấy thông tin Booking từ DB để có được DepositAmount chính xác (đã bao gồm tiền Tour)
                var newBooking = await _context.Bookings.FindAsync(result.BookingId!.Value);
                var depositAmount = (double)(newBooking?.DepositAmount ?? 0);
                
                var vnPayModel = new VNPayRequestModel
                {
                    OrderId = result.BookingId!.Value,
                    Amount = depositAmount,
                    CreatedDate = DateTime.Now,
                    Description = "Thanh toan tien coc phong cho Booking " + result.BookingId.Value
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                return Redirect(paymentUrl);
            }

            if (result.ErrorCode == "BKG-001")
            {
                ModelState.AddModelError(string.Empty, "Ngày Check-in phải trước Check-out và lớn hơn hoặc bằng hôm nay.");
            }
            else if (result.ErrorCode == "BKG-002")
            {
                ModelState.AddModelError(string.Empty, "Rất tiếc, phòng này đã có người đặt trong khoảng thời gian bạn chọn.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Lỗi không xác định khi đặt phòng.");
            }

            ViewBag.Room = room;
            ViewBag.UpcomingTours = await _context.TourSchedules
                .Include(ts => ts.Tour)
                .Where(ts => ts.TourDate != null && ts.TourDate >= DateTime.Today)
                .OrderBy(ts => ts.TourDate)
                .ToListAsync();

            return View(model);
        }

        // GET: /Bookings/MyBookings
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyBookings()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.TourBookings)
                    .ThenInclude(tb => tb.Schedule)
                        .ThenInclude(ts => ts.Tour)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // POST: /Bookings/Cancel/5
        [HttpPost]
        [Authorize(Roles = "Customer, Receptionist, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id, string? reason, string? bankDetails)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            int targetUserId = userId;
            if (User.IsInRole("Admin") || User.IsInRole("Receptionist"))
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking != null)
                {
                    targetUserId = booking.UserId;
                }
            }

            var result = await _bookingService.CancelBookingAsync(id, targetUserId, reason, bankDetails);

            if (result.Success)
            {
                if (result.RefundAmount > 0)
                {
                    TempData["SuccessMessage"] = $"Đã hủy đặt phòng thành công. Bạn sẽ được hoàn lại {result.RefundAmount:N0} VNĐ.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Đã hủy đặt phòng thành công. (Không hoàn cọc do hủy dưới 48 giờ).";
                }
            }
            else
            {
                TempData["Error"] = result.ErrorMessage ?? "Không thể hủy phòng ở trạng thái hiện tại.";
            }

            // Redirect back to the referrer or MyBookings
            if (User.IsInRole("Admin") || User.IsInRole("Receptionist"))
            {
                return RedirectToAction("Index", "Reception");
            }
            return RedirectToAction(nameof(MyBookings));
        }



        // GET: /Bookings/PreCheckin/{id}
        [AllowAnonymous]
        public async Task<IActionResult> PreCheckin(int id, string? token)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound("Không tìm thấy thông tin đặt phòng.");
            }

            bool isOwner = false;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId) && booking.UserId == userId)
            {
                isOwner = true;
            }

            if (!isOwner)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = Request.Path + Request.QueryString });
                }

                var expectedToken = GeneratePreCheckinToken(booking);
                if (expectedToken != token)
                {
                    return Forbid("Token xác thực không hợp lệ hoặc đã hết hạn.");
                }
            }

            // Load existing guests
            var guests = await _bookingGuestService.GetBookingGuestsForCheckInAsync(id);
            var guestList = guests.ToList();

            // Total slots = Adults + Children
            int maxCapacity = (booking.NumberOfAdults ?? 0) + (booking.NumberOfChildren ?? 0);
            
            // Prefill with default empty guests to fill up the capacity slots
            while (guestList.Count < maxCapacity)
            {
                guestList.Add(new BookingGuestDto
                {
                    IsPrimaryBooker = guestList.Count == 0 // First guest is primary booker by default
                });
            }

            var model = new PreCheckinViewModel
            {
                BookingId = booking.BookingId,
                RoomNumber = booking.Room?.RoomNumber ?? string.Empty,
                CategoryName = booking.Room?.Category?.CategoryName ?? string.Empty,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                PdpdConsentChecked = booking.PdpdConsentChecked,
                Guests = guestList
            };

            ViewBag.Token = token;
            ViewBag.ShareableLink = Url.Action("PreCheckin", "Bookings", new { id = booking.BookingId, token = GeneratePreCheckinToken(booking) }, Request.Scheme);
            return View(model);
        }

        // POST: /Bookings/PreCheckin/{id}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreCheckin(PreCheckinViewModel model, string? token)
        {
            var booking = await _context.Bookings.FindAsync(model.BookingId);
            if (booking == null)
            {
                return NotFound("Không tìm thấy thông tin đặt phòng.");
            }

            bool isOwner = false;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId) && booking.UserId == userId)
            {
                isOwner = true;
            }

            if (!isOwner)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Forbid();
                }

                var expectedToken = GeneratePreCheckinToken(booking);
                if (expectedToken != token)
                {
                    return Forbid("Token xác thực không hợp lệ hoặc đã hết hạn.");
                }
            }

            if (!model.PdpdConsentChecked)
            {
                ModelState.AddModelError(nameof(model.PdpdConsentChecked), "Bạn phải đồng ý với điều khoản bảo vệ dữ liệu cá nhân (PDPD) để tiếp tục.");
            }

            // Validations for guests
            for (int i = 0; i < model.Guests.Count; i++)
            {
                var guest = model.Guests[i];
                if (guest.IsPrimaryBooker)
                {
                    if (string.IsNullOrWhiteSpace(guest.FullName))
                    {
                        ModelState.AddModelError($"Guests[{i}].FullName", "Họ tên người đặt chính không được để trống.");
                    }
                    if (guest.DateOfBirth == null)
                    {
                        ModelState.AddModelError($"Guests[{i}].DateOfBirth", "Ngày sinh người đặt chính không được để trống.");
                    }
                    if (guest.Gender == null)
                    {
                        ModelState.AddModelError($"Guests[{i}].Gender", "Vui lòng chọn giới tính người đặt chính.");
                    }
                    if (string.IsNullOrWhiteSpace(guest.PassportNumber))
                    {
                        ModelState.AddModelError($"Guests[{i}].PassportNumber", "Số CCCD/Hộ chiếu người đặt chính không được để trống.");
                    }
                    if (string.IsNullOrWhiteSpace(guest.Nationality))
                    {
                        ModelState.AddModelError($"Guests[{i}].Nationality", "Quốc tịch người đặt chính không được để trống.");
                    }
                }
                else
                {
                    bool hasAnyField = !string.IsNullOrWhiteSpace(guest.FullName) 
                                       || !string.IsNullOrWhiteSpace(guest.PassportNumber) 
                                       || guest.DateOfBirth != null;
                    if (hasAnyField)
                    {
                        if (string.IsNullOrWhiteSpace(guest.FullName))
                        {
                            ModelState.AddModelError($"Guests[{i}].FullName", $"Họ tên khách thứ {i + 1} không được để trống.");
                        }
                        if (guest.DateOfBirth == null)
                        {
                            ModelState.AddModelError($"Guests[{i}].DateOfBirth", $"Ngày sinh khách thứ {i + 1} không được để trống.");
                        }
                        if (guest.Gender == null)
                        {
                            ModelState.AddModelError($"Guests[{i}].Gender", $"Vui lòng chọn giới tính khách thứ {i + 1}.");
                        }
                        if (string.IsNullOrWhiteSpace(guest.Nationality))
                        {
                            ModelState.AddModelError($"Guests[{i}].Nationality", $"Quốc tịch khách thứ {i + 1} không được để trống.");
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                var fullBooking = await _context.Bookings
                    .Include(b => b.Room)
                        .ThenInclude(r => r.Category)
                    .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);
                if (fullBooking != null)
                {
                    model.RoomNumber = fullBooking.Room?.RoomNumber ?? string.Empty;
                    model.CategoryName = fullBooking.Room?.Category?.CategoryName ?? string.Empty;
                    model.CheckInDate = fullBooking.CheckInDate;
                    model.CheckOutDate = fullBooking.CheckOutDate;
                }
                ViewBag.Token = token;
                ViewBag.ShareableLink = Url.Action("PreCheckin", "Bookings", new { id = booking.BookingId, token = GeneratePreCheckinToken(booking) }, Request.Scheme);
                return View(model);
            }

            var validGuests = model.Guests
                .Where(g => !string.IsNullOrWhiteSpace(g.FullName))
                .ToList();

            try
            {
                await _bookingGuestService.SaveBookingGuestsAsync(model.BookingId, validGuests, model.PdpdConsentChecked);
                TempData["SuccessMessage"] = "Khai báo thông tin tạm trú thành công!";
                
                if (isOwner)
                {
                    return RedirectToAction(nameof(MyBookings));
                }
                else
                {
                    return RedirectToAction("PreCheckinSuccess", new { id = model.BookingId, token = token });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra: {ex.Message}");
                ViewBag.Token = token;
                ViewBag.ShareableLink = Url.Action("PreCheckin", "Bookings", new { id = booking.BookingId, token = GeneratePreCheckinToken(booking) }, Request.Scheme);
                return View(model);
            }
        }

        // GET: /Bookings/PreCheckinSuccess/{id}
        [AllowAnonymous]
        public async Task<IActionResult> PreCheckinSuccess(int id, string? token)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound("Không tìm thấy thông tin đặt phòng.");
            }

            if (token != GeneratePreCheckinToken(booking))
            {
                return Forbid("Xác thực token thất bại.");
            }

            return View(booking);
        }



        private string GeneratePreCheckinToken(Booking booking)
        {
            var input = $"{booking.BookingId}-{booking.CheckInDate.Ticks}-PNSecretKey2026";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
