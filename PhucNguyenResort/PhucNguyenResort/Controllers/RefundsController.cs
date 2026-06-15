using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Services.Refunds;
using System.Threading.Tasks;

namespace PhucNguyenResort.Controllers
{
    [Authorize(Roles = "System Admin,Business Manager")]
    public class RefundsController : Controller
    {
        private readonly IRefundService _refundService;

        public RefundsController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        public async Task<IActionResult> Index(RefundStatus? status)
        {
            if (status.HasValue)
            {
                ViewBag.CurrentStatus = status.Value;
            }
            
            var refunds = await _refundService.GetRefundsAsync(status);
            return View(refunds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessRefund(int id)
        {
            var result = await _refundService.ProcessRefundAsync(id);
            
            if (result.NotFound)
            {
                return NotFound();
            }

            if (!result.Success)
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = $"Đã xác nhận hoàn tiền thành công cho yêu cầu #{id}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
