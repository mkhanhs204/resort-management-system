using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Services.Refunds
{
    public interface IRefundService
    {
        Task<IEnumerable<Refund>> GetRefundsAsync(RefundStatus? status);
        Task<ProcessRefundResult> ProcessRefundAsync(int refundId);
    }

    public class ProcessRefundResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
