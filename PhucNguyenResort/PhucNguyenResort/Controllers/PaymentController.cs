using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Payments;
using PhucNguyenResort.ViewModels.Payments;
using System.Linq;

namespace PhucNguyenResort.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly ResortDbContext _context;

        public PaymentController(IVnPayService vnPayService, ResortDbContext context)
        {
            _vnPayService = vnPayService;
            _context = context;
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
                    var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
                    if (booking != null && booking.BookingStatus == BookingStatus.PendingPayment)
                    {
                        booking.BookingStatus = BookingStatus.Reserved;
                        _context.SaveChanges();
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
                _context.SaveChanges();

                return Json(new { RspCode = "00", Message = "Confirm Success" });
            }

            return Json(new { RspCode = "99", Message = "Unknown error" });
        }
    }
}
