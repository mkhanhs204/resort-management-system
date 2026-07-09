using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Email;
using PhucNguyenResort.Services.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Refunds
{
    public class RefundService : IRefundService
    {
        private readonly ResortDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly INotificationService _notificationService;

        public RefundService(ResortDbContext context, IEmailSender emailSender, INotificationService notificationService)
        {
            _context = context;
            _emailSender = emailSender;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Refund>> GetRefundsAsync(RefundStatus? status)
        {
            var query = _context.Refunds
                .Include(r => r.Booking)
                .ThenInclude(b => b.User)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(r => r.RefundStatus == status.Value);
            }

            return await query.OrderByDescending(r => r.RefundId).ToListAsync();
        }

        public async Task<ProcessRefundResult> ProcessRefundAsync(int refundId)
        {
            var refund = await _context.Refunds
                .Include(r => r.Booking)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(r => r.RefundId == refundId);
            if (refund == null)
            {
                return new ProcessRefundResult { Success = false, NotFound = true };
            }

            if (refund.RefundStatus != RefundStatus.Pending)
            {
                return new ProcessRefundResult 
                { 
                    Success = false, 
                    ErrorMessage = "Yêu cầu này đã được xử lý hoặc không hợp lệ." 
                };
            }

            refund.RefundStatus = RefundStatus.Completed;
            await _context.SaveChangesAsync();

            // Gửi email thông báo
            if (refund.Booking?.User?.Email != null)
            {
                var subject = $"[PhucNguyen Resort] Xác nhận hoàn tiền - Booking #{refund.BookingId}";
                var message = $@"
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h3>Kính gửi {refund.Booking.User.FullName},</h3>
                        <p>Yêu cầu hoàn tiền cho đơn đặt phòng <strong>#{refund.BookingId}</strong> của quý khách đã được xử lý thành công.</p>
                        <p>Số tiền: <strong>{refund.RefundAmount:N0} VNĐ</strong></p>
                        <p>Ngân hàng nhận: {refund.BankDetails}</p>
                        <p>Lý do hủy: {refund.RefundReason}</p>
                        <br/>
                        <p>Trân trọng,<br>Ban Quản Lý PhucNguyen Resort</p>
                    </div>
                ";
                await _emailSender.SendEmailAsync(refund.Booking.User.Email, subject, message);

                // Send In-App Notification
                await _notificationService.CreateAsync(
                    refund.Booking.UserId,
                    "Hoàn tiền thành công 💸",
                    $"Yêu cầu hoàn tiền {refund.RefundAmount:N0} VNĐ cho đơn đặt phòng #{refund.BookingId} đã được xử lý. Vui lòng kiểm tra tài khoản ngân hàng."
                );
            }

            return new ProcessRefundResult { Success = true };
        }
    }
}
