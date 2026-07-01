using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Reviews;
using PhucNguyenResort.ViewModels.Reviews;
using System.Security.Claims;

namespace PhucNguyenResort.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> PublicList(int? tourId = null, int? rating = null, string? serviceType = null)
    {
        var reviews = await _reviewService.GetPublicReviewsAsync(tourId);
        
        if (rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
        {
            reviews = reviews.Where(r => r.Rating == rating.Value);
        }
        
        if (!string.IsNullOrEmpty(serviceType) && (serviceType == "Room" || serviceType == "Tour"))
        {
            reviews = reviews.Where(r => r.ServiceType == serviceType);
        }

        ViewBag.CurrentRating = rating;
        ViewBag.CurrentServiceType = serviceType;
        return View(reviews);
    }

    [Authorize(Roles = "Customer")]
    [HttpGet]
    public async Task<IActionResult> Submit(int? bookingId, int? tourBookingId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (bookingId == null && tourBookingId == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy dịch vụ cần đánh giá.";
            return RedirectToAction("Index", "Home");
        }

        var canSubmit = await _reviewService.CanSubmitReviewAsync(userId, bookingId, tourBookingId);
        if (!canSubmit)
        {
            TempData["ErrorMessage"] = "Bạn chỉ có thể đánh giá sau khi đã hoàn thành dịch vụ hoặc bạn đã đánh giá dịch vụ này rồi.";
            return RedirectToAction("Index", "Home");
        }

        var model = new SubmitReviewVM
        {
            BookingId = bookingId,
            TourBookingId = tourBookingId
        };

        return View(model);
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<IActionResult> Submit(SubmitReviewVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _reviewService.CreateReviewAsync(userId, model);

            TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi đánh giá!";
            return RedirectToAction(nameof(PublicList));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [Authorize(Roles = "Business Manager")]
    [HttpGet]
    public async Task<IActionResult> Manage(int? rating = null, string? serviceType = null)
    {
        var reviews = await _reviewService.GetAllReviewsAsync();
        
        if (rating.HasValue && rating.Value >= 1 && rating.Value <= 5)
        {
            reviews = reviews.Where(r => r.Rating == rating.Value);
        }
        
        if (!string.IsNullOrEmpty(serviceType) && (serviceType == "Room" || serviceType == "Tour"))
        {
            reviews = reviews.Where(r => r.ServiceType == serviceType);
        }

        ViewBag.CurrentRating = rating;
        ViewBag.CurrentServiceType = serviceType;
        return View(reviews);
    }

    [Authorize(Roles = "Business Manager")]
    [HttpPost]
    public async Task<IActionResult> ToggleVisibility(int reviewId)
    {
        try
        {
            await _reviewService.ToggleVisibilityAsync(reviewId);
            TempData["SuccessMessage"] = "Đã cập nhật trạng thái hiển thị.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Manage));
    }
}
