using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.ViewModels.Payments;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ResortDbContext _context;
        private readonly IVnPayService _vnPayService;

        public BookingsController(IBookingService bookingService, ResortDbContext context, IVnPayService vnPayService)
        {
            _bookingService = bookingService;
            _context = context;
            _vnPayService = vnPayService;
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
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }

            // Admin/Receptionist can cancel any booking. We might need a slightly different logic for them if the service requires strictly the owner's userId.
            // For now, if role is Admin/Receptionist, we bypass the userId check in the service by getting the actual booking's UserId, or we modify the service.
            // Let's pass the booking's owner userId if the current user is Staff.
            
            int targetUserId = userId;
            if (User.IsInRole("Admin") || User.IsInRole("Receptionist"))
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking != null)
                {
                    targetUserId = booking.UserId;
                }
            }

            var result = await _bookingService.CancelBookingAsync(id, targetUserId);

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
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(MyBookings));
        }

        // GET: /Bookings/Index (For Staff)
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }
    }
}
