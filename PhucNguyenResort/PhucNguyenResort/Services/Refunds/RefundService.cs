using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Refunds
{
    public class RefundService : IRefundService
    {
        private readonly ResortDbContext _context;

        public RefundService(ResortDbContext context)
        {
            _context = context;
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
            var refund = await _context.Refunds.FindAsync(refundId);
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

            return new ProcessRefundResult { Success = true };
        }
    }
}
