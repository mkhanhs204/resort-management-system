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
    [Authorize(Roles = "Receptionist, Admin, Business Manager")]
    public class ReceptionController : Controller
    {
        private readonly IReceptionService _receptionService;

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        // GET: /Reception/Index
        public async Task<IActionResult> Index(string? searchString, BookingStatus? statusFilter, DateTime? dateFilter, bool clearFilter = false, int page = 1)
        {
            if (clearFilter) return RedirectToAction(nameof(Index));

            if (Request.Query.Count == 0 && !Request.Query.ContainsKey("page")) dateFilter = DateTime.Today;

            var bookings = await _receptionService.GetBookingsAsync(searchString, statusFilter, dateFilter);

            int pageSize = 10;
            var totalItems = bookings.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages && totalPages > 0 ? totalPages : page;

            var pagedBookings = bookings.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var tokens = new Dictionary<int, string>();
            foreach (var b in pagedBookings)
            {
                tokens[b.BookingId] = GeneratePreCheckinToken(b);
            }
            ViewBag.PreCheckinTokens = tokens;
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = statusFilter;
            ViewBag.CurrentDate = dateFilter?.ToString("yyyy-MM-dd");
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            return View(pagedBookings);
        }

        // GET: /Reception/CheckIn/{id}
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> CheckIn(int id)
        {
            var model = await _receptionService.PrepareCheckInViewModelAsync(id);
            if (model == null) return NotFound("Không tìm thấy thông tin đặt phòng.");

            var booking = await _receptionService.GetBookingByIdAsync(id);
            if (booking != null && booking.BookingStatus != BookingStatus.Reserved && booking.BookingStatus != BookingStatus.PendingPayment && booking.BookingStatus != BookingStatus.Arrived)
            {
                TempData["Error"] = "Đơn đặt phòng này không ở trạng thái có thể Check-in hoặc Giao phòng.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: /Reception/CheckIn/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Receptionist, Admin")]
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

            // Validation for room assignments
            if (!model.IsArrivalOnly)
            {
                if (model.RoomAssignments == null || model.RoomAssignments.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Danh sách gán phòng trống.");
                }
                else
                {
                    var selectedRoomIds = new List<int>();
                    for (int i = 0; i < model.RoomAssignments.Count; i++)
                    {
                        var assignment = model.RoomAssignments[i];
                        if (!assignment.SelectedRoomId.HasValue)
                        {
                            ModelState.AddModelError($"RoomAssignments[{i}].SelectedRoomId", "Vui lòng chọn một phòng vật lý.");
                        }
                        else
                        {
                            if (selectedRoomIds.Contains(assignment.SelectedRoomId.Value))
                            {
                                ModelState.AddModelError($"RoomAssignments[{i}].SelectedRoomId", "Một phòng vật lý không thể được gán cho nhiều chi tiết đặt phòng cùng lúc.");
                            }
                            selectedRoomIds.Add(assignment.SelectedRoomId.Value);
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                var fullModel = await _receptionService.PrepareCheckInViewModelAsync(model.BookingId);
                if (fullModel != null)
                {
                    model.CategoryName = fullModel.CategoryName;
                    model.CheckInDate = fullModel.CheckInDate;
                    model.CheckOutDate = fullModel.CheckOutDate;

                    if (model.RoomAssignments != null)
                    {
                        foreach (var assignment in model.RoomAssignments)
                        {
                            var fullAssignment = fullModel.RoomAssignments.FirstOrDefault(a => a.BookingDetailId == assignment.BookingDetailId);
                            if (fullAssignment != null)
                            {
                                assignment.AvailableRooms = fullAssignment.AvailableRooms;
                                assignment.CategoryName = fullAssignment.CategoryName;
                            }
                        }
                    }
                    else
                    {
                        model.RoomAssignments = fullModel.RoomAssignments;
                    }
                }
                return View(model);
            }

            var result = await _receptionService.ProcessCheckInAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Thực hiện Check-in thành công cho Booking #{model.BookingId}!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (result.ErrorMessage == "Không tìm thấy thông tin đặt phòng.")
                {
                    return NotFound(result.ErrorMessage);
                }
                
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                var fullModel = await _receptionService.PrepareCheckInViewModelAsync(model.BookingId);
                if (fullModel != null)
                {
                    model.CategoryName = fullModel.CategoryName;
                    model.CheckInDate = fullModel.CheckInDate;
                    model.CheckOutDate = fullModel.CheckOutDate;

                    if (model.RoomAssignments != null)
                    {
                        foreach (var assignment in model.RoomAssignments)
                        {
                            var fullAssignment = fullModel.RoomAssignments.FirstOrDefault(a => a.BookingDetailId == assignment.BookingDetailId);
                            if (fullAssignment != null)
                            {
                                assignment.AvailableRooms = fullAssignment.AvailableRooms;
                                assignment.CategoryName = fullAssignment.CategoryName;
                            }
                        }
                    }
                    else
                    {
                        model.RoomAssignments = fullModel.RoomAssignments;
                    }
                }
                return View(model);
            }
        }

        // GET: /Reception/ExportManifest/{id}
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> ExportManifest(int id)
        {
            var model = await _receptionService.PrepareCheckInViewModelAsync(id);
            if (model == null)
            {
                return NotFound("Không tìm thấy thông tin đặt phòng.");
            }
            return View("PrintManifest", model);
        }

        // GET: /Reception/WalkInBooking
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> WalkInBooking()
        {
            var model = new WalkInBookingViewModel
            {
                CheckInDate = DateTime.Today,
                CheckOutDate = DateTime.Today.AddDays(1),
                Guests = new List<BookingGuestDto> { new BookingGuestDto { IsPrimaryBooker = true } }
            };

            ViewBag.AvailableRooms = await _receptionService.GetAvailableRoomsForDatesAsync(model.CheckInDate, model.CheckOutDate);
            return View(model);
        }

        // POST: /Reception/WalkInBooking
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> WalkInBooking(WalkInBookingViewModel model)
        {
            if (model.InstantCheckIn)
            {
                if (model.Guests == null || !model.Guests.Any())
                {
                    ModelState.AddModelError(string.Empty, "Vui lòng điền thông tin khai báo tạm trú khi nhận phòng ngay.");
                }
                else
                {
                    for (int i = 0; i < model.Guests.Count; i++)
                    {
                        var guest = model.Guests[i];
                        if (guest.IsPrimaryBooker)
                        {
                            if (string.IsNullOrWhiteSpace(guest.FullName))
                            {
                                ModelState.AddModelError($"Guests[{i}].FullName", "Họ tên người đặt chính không được để trống.");
                            }
                            if (guest.DateOfBirth == null)
                            {
                                ModelState.AddModelError($"Guests[{i}].DateOfBirth", "Ngày sinh người đặt chính không được để trống.");
                            }
                            if (guest.Gender == null)
                            {
                                ModelState.AddModelError($"Guests[{i}].Gender", "Vui lòng chọn giới tính người đặt chính.");
                            }
                            if (string.IsNullOrWhiteSpace(guest.PassportNumber))
                            {
                                ModelState.AddModelError($"Guests[{i}].PassportNumber", "Số CCCD/Hộ chiếu người đặt chính không được để trống.");
                            }
                            if (string.IsNullOrWhiteSpace(guest.Nationality))
                            {
                                ModelState.AddModelError($"Guests[{i}].Nationality", "Quốc tịch người đặt chính không được để trống.");
                            }
                        }
                        else
                        {
                            bool hasAnyField = !string.IsNullOrWhiteSpace(guest.FullName) 
                                               || !string.IsNullOrWhiteSpace(guest.PassportNumber) 
                                               || guest.DateOfBirth != null;
                            if (hasAnyField)
                            {
                                if (string.IsNullOrWhiteSpace(guest.FullName))
                                {
                                    ModelState.AddModelError($"Guests[{i}].FullName", $"Họ tên khách thứ {i + 1} không được để trống.");
                                }
                                if (guest.DateOfBirth == null)
                                {
                                    ModelState.AddModelError($"Guests[{i}].DateOfBirth", $"Ngày sinh khách thứ {i + 1} không được để trống.");
                                }
                                if (guest.Gender == null)
                                {
                                    ModelState.AddModelError($"Guests[{i}].Gender", $"Vui lòng chọn giới tính khách thứ {i + 1}.");
                                }
                                if (string.IsNullOrWhiteSpace(guest.Nationality))
                                {
                                    ModelState.AddModelError($"Guests[{i}].Nationality", $"Quốc tịch khách thứ {i + 1} không được để trống.");
                                }
                            }
                        }
                    }
                }

                if (!model.PdpdConsentChecked)
                {
                    ModelState.AddModelError(nameof(model.PdpdConsentChecked), "Vui lòng xác nhận sự đồng ý bảo vệ dữ liệu cá nhân (PDPD) của khách hàng.");
                }
            }

            if (model.CheckInDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.CheckInDate), "Ngày nhận phòng không được ở quá khứ.");
            }
            if (model.CheckOutDate.Date <= model.CheckInDate.Date)
            {
                ModelState.AddModelError(nameof(model.CheckOutDate), "Ngày trả phòng phải sau ngày nhận phòng.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.AvailableRooms = await _receptionService.GetAvailableRoomsForDatesAsync(model.CheckInDate, model.CheckOutDate);
                return View(model);
            }

            var result = await _receptionService.ProcessWalkInBookingAsync(model);
            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Đặt phòng tại quầy thành công cho Booking #{result.BookingId}!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Lỗi không xác định khi đặt phòng tại quầy.");
            ViewBag.AvailableRooms = await _receptionService.GetAvailableRoomsForDatesAsync(model.CheckInDate, model.CheckOutDate);
            return View(model);
        }

        // GET: /Reception/GetAvailableRoomsJson
        [HttpGet]
        public async Task<IActionResult> GetAvailableRoomsJson(DateTime checkIn, DateTime checkOut)
        {
            var rooms = await _receptionService.GetAvailableRoomsForDatesAsync(checkIn, checkOut);
            var result = rooms.Select(r => new
            {
                roomId = r.RoomId,
                roomNumber = r.RoomNumber,
                categoryId = r.CategoryId,
                categoryName = r.Category?.CategoryName ?? "Loại phòng",
                price = r.Category?.PricePerNight ?? 0,
                maxAdults = r.Category?.MaxAdults ?? 0,
                maxChildren = r.Category?.MaxChildren ?? 0
            });
            return Json(result);
        }

        // GET: /Reception/CheckOut/{id}
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var model = await _receptionService.PrepareCheckOutViewModelAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin đặt phòng hoặc đơn đặt phòng không ở trạng thái có thể Check-out.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: /Reception/CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Receptionist, Admin")]
        public async Task<IActionResult> CheckOut(CheckOutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload model data
                var fullModel = await _receptionService.PrepareCheckOutViewModelAsync(model.BookingId);
                if (fullModel != null)
                {
                    model.GuestName = fullModel.GuestName;
                    model.CheckInDate = fullModel.CheckInDate;
                    model.CheckOutDate = fullModel.CheckOutDate;
                    model.ActualCheckInTime = fullModel.ActualCheckInTime;
                    model.RoomNumbers = fullModel.RoomNumbers;
                    model.TotalRoomCharges = fullModel.TotalRoomCharges;
                    model.TotalFoodCharges = fullModel.TotalFoodCharges;
                    model.TotalTourCharges = fullModel.TotalTourCharges;
                    model.LateCheckOutSurcharge = fullModel.LateCheckOutSurcharge;
                    model.TotalAmount = fullModel.TotalAmount;
                    model.DepositAmount = fullModel.DepositAmount;
                    model.BalanceDue = fullModel.BalanceDue;
                    model.Transactions = fullModel.Transactions;
                }
                return View(model);
            }

            var result = await _receptionService.ProcessCheckOutAsync(model);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = $"Thực hiện Check-out thành công cho Booking #{model.BookingId}!";
                return RedirectToAction(nameof(PrintInvoice), new { id = model.BookingId });
            }
            
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            var reloadedModel = await _receptionService.PrepareCheckOutViewModelAsync(model.BookingId);
            if (reloadedModel != null)
            {
                model.GuestName = reloadedModel.GuestName;
                model.CheckInDate = reloadedModel.CheckInDate;
                model.CheckOutDate = reloadedModel.CheckOutDate;
                model.ActualCheckInTime = reloadedModel.ActualCheckInTime;
                model.RoomNumbers = reloadedModel.RoomNumbers;
                model.TotalRoomCharges = reloadedModel.TotalRoomCharges;
                model.TotalFoodCharges = reloadedModel.TotalFoodCharges;
                model.TotalTourCharges = reloadedModel.TotalTourCharges;
                model.LateCheckOutSurcharge = reloadedModel.LateCheckOutSurcharge;
                model.TotalAmount = reloadedModel.TotalAmount;
                model.DepositAmount = reloadedModel.DepositAmount;
                model.BalanceDue = reloadedModel.BalanceDue;
                model.Transactions = reloadedModel.Transactions;
            }
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

        // GET: /Reception/PrintInvoice/{id}
        public async Task<IActionResult> PrintInvoice(int id)
        {
            var model = await _receptionService.PrepareInvoiceViewModelAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin hóa đơn cho đặt phòng này.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}