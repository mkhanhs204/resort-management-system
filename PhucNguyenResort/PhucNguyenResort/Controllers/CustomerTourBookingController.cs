using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.Services.Tours;
using PhucNguyenResort.ViewModels.Payments;
using PhucNguyenResort.ViewModels.Tours;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers
{
    /// <summary>
    /// CustomerTourBookingController — Customer/Guest facing tour booking.
    /// Handles browsing, booking (with anti-overbooking), payment, and history.
    /// </summary>
    public class CustomerTourBookingController : Controller
    {
        private readonly ICustomerTourBookingService _tourBookingService;
        private readonly IVnPayService _vnPayService;
        private readonly ResortDbContext _context;

        public CustomerTourBookingController(
            ICustomerTourBookingService tourBookingService,
            IVnPayService vnPayService,
            ResortDbContext context)
        {
            _tourBookingService = tourBookingService;
            _vnPayService = vnPayService;
            _context = context;
        }

        /// <summary>
        /// GET: /CustomerTourBooking — Browse all available tours.
        /// Accessible by Guest + Customer.
        /// </summary>
        public async Task<IActionResult> Index(string? search, string? sortBy)
        {
            var model = await _tourBookingService.GetTourListAsync(search, sortBy);
            return View(model);
        }

        /// <summary>
        /// GET: /CustomerTourBooking/Details/5 — View tour details + upcoming schedules.
        /// Accessible by Guest + Customer.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var model = await _tourBookingService.GetTourDetailAsync(id.Value);
            if (model == null) return NotFound();

            return View(model);
        }

        /// <summary>
        /// GET: /CustomerTourBooking/Book/101 — Show booking form for a schedule.
        /// Customer only (redirects to Login if not authenticated).
        /// </summary>
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Book(int? id)
        {
            if (id == null) return NotFound();

            var userId = await GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Logout", "Account");

            var model = await _tourBookingService.PrepareBookingFormAsync(id.Value, userId.Value);
            if (model == null)
            {
                TempData["Error"] = "Không tìm thấy lịch trình tour này.";
                return RedirectToAction(nameof(Index));
            }

            if (model.AvailableSlots <= 0)
            {
                TempData["Error"] = "Lịch trình này đã hết chỗ.";
                return RedirectToAction("Details", new { id = model.ScheduleId });
            }

            return View(model);
        }

        /// <summary>
        /// POST: /CustomerTourBooking/Book — Process tour booking.
        /// Uses Pessimistic Locking via service to prevent overbooking.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookTourVM model)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Logout", "Account");

            if (!ModelState.IsValid)
            {
                // Re-populate form data
                var formData = await _tourBookingService.PrepareBookingFormAsync(model.ScheduleId, userId.Value);
                if (formData != null)
                {
                    model.TourName = formData.TourName;
                    model.TourImage = formData.TourImage;
                    model.Location = formData.Location;
                    model.DurationHours = formData.DurationHours;
                    model.PricePerPerson = formData.PricePerPerson;
                    model.TourDate = formData.TourDate;
                    model.StartTime = formData.StartTime;
                    model.AvailableSlots = formData.AvailableSlots;
                    model.AvailableRoomBookings = formData.AvailableRoomBookings;
                }
                return View(model);
            }

            var result = await _tourBookingService.BookTourAsync(model, userId.Value);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Đã có lỗi xảy ra.");

                // Re-populate form data
                var formData = await _tourBookingService.PrepareBookingFormAsync(model.ScheduleId, userId.Value);
                if (formData != null)
                {
                    model.TourName = formData.TourName;
                    model.TourImage = formData.TourImage;
                    model.Location = formData.Location;
                    model.DurationHours = formData.DurationHours;
                    model.PricePerPerson = formData.PricePerPerson;
                    model.TourDate = formData.TourDate;
                    model.StartTime = formData.StartTime;
                    model.AvailableSlots = formData.AvailableSlots;
                    model.AvailableRoomBookings = formData.AvailableRoomBookings;
                }
                return View(model);
            }

            // Payment routing
            if (model.PaymentMethod == "VNPay")
            {
                // Redirect to VNPay payment gateway
                var vnPayModel = new VNPayRequestModel
                {
                    OrderId = result.TourBookingId!.Value,
                    Amount = (double)result.TotalAmount,
                    CreatedDate = DateTime.Now,
                    Description = $"Thanh toan tour booking #{result.TourBookingId.Value}"
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
                return Redirect(paymentUrl);
            }
            else
            {
                // Charge to Room — already confirmed in service
                TempData["Success"] = "Đặt tour thành công! Chi phí đã được ghi nợ vào phòng.";
                return RedirectToAction("BookingSuccess", new { id = result.TourBookingId });
            }
        }

        /// <summary>
        /// GET: /CustomerTourBooking/VNPayTourReturn — VNPay callback for tour payment.
        /// </summary>
        public async Task<IActionResult> VNPayTourReturn()
        {
            var response = _vnPayService.ExecutePayment(Request.Query);

            if (!response.Success)
            {
                TempData["Error"] = "Thanh toán không thành công. Mã lỗi: " + response.VnPayResponseCode;

                // Restore slots for failed payment
                if (int.TryParse(response.OrderId, out int failedTourBookingId))
                {
                    var failedBooking = await _context.TourBookings
                        .Include(tb => tb.Schedule)
                        .FirstOrDefaultAsync(tb => tb.TourBookingId == failedTourBookingId);

                    if (failedBooking != null && failedBooking.BookingStatus == Enums.TourBookingStatus.Pending)
                    {
                        failedBooking.BookingStatus = Enums.TourBookingStatus.Cancelled;
                        if (failedBooking.Schedule != null && failedBooking.Schedule.AvailableSlots.HasValue)
                        {
                            failedBooking.Schedule.AvailableSlots += failedBooking.Participants ?? 0;
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(MyTourBookings));
            }

            // Payment successful — confirm the booking
            if (int.TryParse(response.OrderId, out int tourBookingId))
            {
                await _tourBookingService.ConfirmPaymentAsync(tourBookingId);
                TempData["Success"] = "Thanh toán tour thành công!";
                return RedirectToAction("BookingSuccess", new { id = tourBookingId });
            }

            TempData["Error"] = "Không thể xác nhận thanh toán.";
            return RedirectToAction(nameof(MyTourBookings));
        }

        /// <summary>
        /// GET: /CustomerTourBooking/BookingSuccess/5 — Display booking confirmation.
        /// </summary>
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BookingSuccess(int? id)
        {
            if (id == null) return NotFound();

            var userId = await GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Logout", "Account");

            var model = await _tourBookingService.GetBookingSuccessInfoAsync(id.Value, userId.Value);
            if (model == null) return NotFound();

            // Generate full QR URL pointing to CheckTicket
            var request = HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            string qrData = $"{baseUrl}/Tours/CheckTicket?ticketId={model.TourBookingId}";
            model.QrCodeUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={System.Net.WebUtility.UrlEncode(qrData)}";

            return View(model);
        }

        /// <summary>
        /// GET: /CustomerTourBooking/MyTourBookings — View personal tour booking history.
        /// </summary>
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyTourBookings()
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Logout", "Account");

            var model = await _tourBookingService.GetUserTourBookingsAsync(userId.Value);
            return View(model);
        }

        /// <summary>
        /// POST: /CustomerTourBooking/CancelTourBooking — Cancel a tour booking.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelTourBooking(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            if (userId == null) return RedirectToAction("Logout", "Account");

            var result = await _tourBookingService.CancelTourBookingAsync(id, userId.Value);

            if (result.Success)
            {
                TempData["Success"] = "Hủy đặt tour thành công!";
            }
            else
            {
                TempData["Error"] = result.ErrorMessage ?? "Không thể hủy đặt tour.";
            }

            return RedirectToAction(nameof(MyTourBookings));
        }

        private async Task<int?> GetCurrentUserIdAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                if (await _context.Users.AnyAsync(u => u.UserId == userId))
                    return userId;
            }
            return null;
        }
    }
}
