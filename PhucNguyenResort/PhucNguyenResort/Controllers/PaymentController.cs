using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.ViewModels.Payments;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PhucNguyenResort.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly ResortDbContext _context;
        private readonly PhucNguyenResort.Services.Email.IEmailSender _emailSender;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public PaymentController(
            IVnPayService vnPayService, 
            ResortDbContext context,
            PhucNguyenResort.Services.Email.IEmailSender emailSender,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _vnPayService = vnPayService;
            _context = context;
            _emailSender = emailSender;
            _env = env;
        }

        public IActionResult VNPayReturn()
        {
            var response = _vnPayService.ExecutePayment(Request.Query);
            if (!response.Success)
            {
                // Optionally handle fail
                TempData["Error"] = "Thanh toán không thành công. Mã lỗi: " + response.VnPayResponseCode;
            }
            else
            {
                // Cập nhật DB ngay tại Return URL cho môi trường Localhost (Vì VNPAY không gọi được IPN về localhost)
                if (int.TryParse(response.OrderId, out int bookingId))
                {
                    var booking = _context.Bookings
                        .Include(b => b.User)
                        .Include(b => b.Room)
                        .Include(b => b.TourBookings)
                            .ThenInclude(tb => tb.Schedule)
                                .ThenInclude(s => s.Tour)
                        .FirstOrDefault(b => b.BookingId == bookingId);

                    if (booking != null && booking.BookingStatus == BookingStatus.PendingPayment)
                    {
                        booking.BookingStatus = BookingStatus.Reserved;

                        // Cập nhật trạng thái các TourBooking liên quan sang Confirmed
                        foreach (var tb in booking.TourBookings.Where(t => t.BookingStatus == TourBookingStatus.Pending))
                        {
                            tb.BookingStatus = TourBookingStatus.Confirmed;
                        }

                        _context.SaveChanges();

                        // Gửi email thông báo
                        if (booking.User != null && !string.IsNullOrEmpty(booking.User.Email))
                        {
                            var templatePath = System.IO.Path.Combine(_env.ContentRootPath, "EmailTemplates", "BookingSuccess.html");
                            if (System.IO.File.Exists(templatePath))
                            {
                                // Generate Tour Details HTML
                                string tourHtml = "";
                                if (booking.TourBookings.Any())
                                {
                                    tourHtml = "<h3 style='color: #dfa974; margin-top: 30px;'>Vé Dịch Vụ Tour Đi Kèm</h3>";
                                    foreach (var tb in booking.TourBookings)
                                    {
                                        var request = HttpContext.Request;
                                        string baseUrl = $"{request.Scheme}://{request.Host}";
                                        string tourName = tb.Schedule?.Tour?.TourName ?? "Tour không xác định";
                                        string qrData = $"{baseUrl}/Tours/CheckTicket?ticketId={tb.TourBookingId}";
                                        string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={System.Net.WebUtility.UrlEncode(qrData)}";

                                        tourHtml += $@"
                                            <div style='background-color: #f9f9f9; padding: 15px; border: 1px solid #ddd; border-radius: 5px; margin-bottom: 15px; text-align: center;'>
                                                <h4 style='margin-top: 0;'>{tourName}</h4>
                                                <p>Số người: <strong>{tb.Participants}</strong></p>
                                                <img src='{qrUrl}' alt='QR Code' style='margin-top: 10px; border: 2px solid #dfa974; border-radius: 5px; padding: 5px;' />
                                                <p style='font-size: 12px; color: #777;'>Mã vé: {tb.TourBookingId}</p>
                                            </div>";
                                    }
                                }

                                var htmlBody = System.IO.File.ReadAllText(templatePath);
                                htmlBody = htmlBody.Replace("{{CustomerName}}", booking.User.FullName)
                                                   .Replace("{{BookingId}}", booking.BookingId.ToString())
                                                   .Replace("{{RoomName}}", booking.Room?.RoomNumber ?? "N/A")
                                                   .Replace("{{CheckInDate}}", booking.CheckInDate.ToString("dd/MM/yyyy"))
                                                   .Replace("{{CheckOutDate}}", booking.CheckOutDate.ToString("dd/MM/yyyy"))
                                                   .Replace("{{TotalAmount}}", (booking.TotalAmount ?? 0).ToString("N0"))
                                                   .Replace("{{DepositAmount}}", (booking.DepositAmount ?? 0).ToString("N0"))
                                                   .Replace("{{TourDetails}}", tourHtml);

                                // Chạy ngầm việc gửi mail để không chặn request của khách
                                _ = _emailSender.SendEmailAsync(booking.User.Email, "Xác nhận đặt phòng và thanh toán thành công - PhucNguyen Resort", htmlBody);
                            }
                        }
                    }
                }
                
                TempData["Success"] = "Thanh toán thành công!";
            }
            
            // Redirect to My Bookings or a success page
            return RedirectToAction("MyBookings", "Bookings");
        }

        [HttpGet]
        public IActionResult VNPayIPN()
        {
            var response = _vnPayService.ExecutePayment(Request.Query);
            
            if (response.VnPayResponseCode == "97")
            {
                return Json(new { RspCode = "97", Message = "Invalid Checksum" });
            }

            // Tìm Booking trong DB
            if (!int.TryParse(response.OrderId, out int bookingId))
            {
                return Json(new { RspCode = "01", Message = "Order not found" });
            }

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                return Json(new { RspCode = "01", Message = "Order not found" });
            }

            // Kiểm tra Idempotency
            if (booking.BookingStatus != BookingStatus.PendingPayment)
            {
                return Json(new { RspCode = "02", Message = "Order already confirmed" });
            }

            // (Tuỳ chọn: Kiểm tra Amount ở đây, response.Amount là số tiền VNĐ. Bỏ qua cho đơn giản TDD)

            if (response.Success)
            {
                // Cập nhật trạng thái thành Reserved khi đã cọc
                booking.BookingStatus = BookingStatus.Reserved;

                // Cập nhật trạng thái các TourBooking liên quan sang Confirmed
                var tourBookings = _context.TourBookings.Where(tb => tb.BookingId == bookingId && tb.BookingStatus == TourBookingStatus.Pending).ToList();
                foreach (var tb in tourBookings)
                {
                    tb.BookingStatus = TourBookingStatus.Confirmed;
                }

                _context.SaveChanges();

                return Json(new { RspCode = "00", Message = "Confirm Success" });
            }

            return Json(new { RspCode = "99", Message = "Unknown error" });
        }
    }
}
