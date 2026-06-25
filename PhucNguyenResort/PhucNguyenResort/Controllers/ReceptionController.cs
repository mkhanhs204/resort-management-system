using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Enums;
using PhucNguyenResort.Models;
using PhucNguyenResort.Services.Reception;
using PhucNguyenResort.Services.Bookings;
using PhucNguyenResort.ViewModels.Bookings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhucNguyenResort.Controllers
{
    [Authorize(Roles = "Receptionist, Admin")]
    public class ReceptionController : Controller
    {
        private readonly IReceptionService _receptionService;

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        // GET: /Reception/Index
        public async Task<IActionResult> Index(string? searchString, BookingStatus? statusFilter, DateTime? dateFilter, bool clearFilter = false)
        {
            if (clearFilter) return RedirectToAction(nameof(Index));

            if (Request.Query.Count == 0) dateFilter = DateTime.Today;

            var bookings = await _receptionService.GetBookingsAsync(searchString, statusFilter, dateFilter);

            var tokens = new Dictionary<int, string>();
            foreach (var b in bookings)
            {
                tokens[b.BookingId] = GeneratePreCheckinToken(b);
            }
            ViewBag.PreCheckinTokens = tokens;
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = statusFilter;
            ViewBag.CurrentDate = dateFilter?.ToString("yyyy-MM-dd");

            return View(bookings);
        }

        // GET: /Reception/CheckIn/{id}
        public async Task<IActionResult> CheckIn(int id)
        {
            var model = await _receptionService.PrepareCheckInViewModelAsync(id);
            if (model == null) return NotFound("Không tìm thấy thông tin đặt phòng.");

            var booking = await _receptionService.GetBookingByIdAsync(id);
            if (booking != null && booking.BookingStatus != BookingStatus.Reserved && booking.BookingStatus != BookingStatus.PendingPayment)
            {
                TempData["Error"] = "Đơn đặt phòng này không ở trạng thái có thể Check-in.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: /Reception/CheckIn/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(PreCheckinViewModel model)
        {
            if (model.Guests == null) model.Guests = new List<BookingGuestDto>();

            for (int i = 0; i < model.Guests.Count; i++)
            {
                var guest = model.Guests[i];
                if (guest.IsPrimaryBooker)
                {
                    if (string.IsNullOrWhiteSpace(guest.FullName))
                        ModelState.AddModelError($"Guests[{i}].FullName", "Họ tên người đặt chính không được để trống.");
                    if (guest.DateOfBirth == null)
                        ModelState.AddModelError($"Guests[{i}].DateOfBirth", "Ngày sinh người đặt chính không được để trống.");
                    if (string.IsNullOrWhiteSpace(guest.PassportNumber))
                        ModelState.AddModelError($"Guests[{i}].PassportNumber", "Số CCCD/Hộ chiếu người đặt chính không được để trống.");
                    if (string.IsNullOrWhiteSpace(guest.Nationality))
                        ModelState.AddModelError($"Guests[{i}].Nationality", "Quốc tịch người đặt chính không được để trống.");
                }
            }

            if (!ModelState.IsValid)
            {
                var fullModel = await _receptionService.PrepareCheckInViewModelAsync(model.BookingId);
                if (fullModel != null)
                {
                    model.RoomNumber = fullModel.RoomNumber;
                    model.CategoryName = fullModel.CategoryName;
                    model.CheckInDate = fullModel.CheckInDate;
                    model.CheckOutDate = fullModel.CheckOutDate;
                }
                return View(model);
            }

            var result = await _receptionService.ProcessCheckInAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Thực hiện Check-in thành công cho Booking #{model.BookingId}!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Lỗi không xác định.");
            return View(model);
        }

        private string GeneratePreCheckinToken(Booking booking)
        {
            var input = $"{booking.BookingId}-{booking.CheckInDate.Ticks}-PNSecretKey2026";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}