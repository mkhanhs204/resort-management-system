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

        public async Task<IActionResult> Index(RefundStatus? status, int page = 1)
        {
            int pageSize = 10;
            if (status.HasValue)
            {
                ViewBag.CurrentStatus = status.Value;
            }
            
            var refunds = await _refundService.GetRefundsAsync(status);
            
            var totalItems = refunds.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages && totalPages > 0 ? totalPages : page;

            var pagedRefunds = refunds.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedRefunds);
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
