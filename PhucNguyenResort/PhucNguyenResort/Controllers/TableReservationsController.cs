using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.TableReservations;
using PhucNguyenResort.ViewModels.TableReservations;

namespace PhucNguyenResort.Controllers;

public class TableReservationsController : Controller
{
    private const int DefaultDurationMinutes = 120;
    private static readonly TimeSpan DefaultReservationTime = new(18, 0, 0);

    private readonly ITableReservationService _reservationService;

    public TableReservationsController(ITableReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Search()
    {
        var model = new TableReservationSearchViewModel
        {
            ReservationDate = DateTime.Today,
            ReservationTime = DefaultReservationTime,
            PartySize = 2,
            Tables = (await _reservationService.GetPublicDiningTablesAsync()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(TableReservationSearchViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Tables = (await _reservationService.GetPublicDiningTablesAsync()).ToList();
            return View(model);
        }

        model.Tables = (await _reservationService.GetPublicDiningTablesAsync()).ToList();
        model.AvailableTables = (await _reservationService.SearchAvailableTablesAsync(model)).ToList();
        if (model.AvailableTables.Count == 0)
        {
            TempData["ErrorMessage"] = "No available tables match your reservation request.";
        }

        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(
        int tableId,
        DateTime reservationDate,
        TimeSpan reservationTime,
        int partySize,
        int durationMinutes = DefaultDurationMinutes)
    {
        var search = new TableReservationSearchViewModel
        {
            ReservationDate = reservationDate.Date,
            ReservationTime = reservationTime,
            PartySize = partySize
        };

        var availableTables = await _reservationService.SearchAvailableTablesAsync(search);
        var selectedTable = availableTables.FirstOrDefault(t => t.TableId == tableId);
        if (selectedTable == null)
        {
            TempData["ErrorMessage"] = "Table is no longer available.";
            return RedirectToAction(nameof(Search));
        }

        var model = new TableReservationCreateViewModel
        {
            TableId = selectedTable.TableId,
            TableNumber = selectedTable.TableNumber,
            TableName = selectedTable.TableName,
            ImageUrl = selectedTable.ImageUrl,
            Description = selectedTable.Description,
            Area = selectedTable.Area,
            TableType = selectedTable.TableType,
            LocationNote = selectedTable.LocationNote,
            Capacity = selectedTable.Capacity,
            ReservationDate = selectedTable.ReservationDate,
            ReservationTime = selectedTable.ReservationTime,
            DurationMinutes = durationMinutes > 0 ? durationMinutes : selectedTable.DurationMinutes,
            PartySize = partySize
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TableReservationCreateViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _reservationService.CreateReservationAsync(model, currentUserId.Value);
        if (result.Status == TableReservationCreateStatus.Created)
        {
            TempData["SuccessMessage"] = "Your table reservation request has been submitted.";
            return RedirectToAction(nameof(Details), new { id = result.ReservationId });
        }

        ModelState.AddModelError(string.Empty, GetCreateErrorMessage(result.Status));

        var search = new TableReservationSearchViewModel
        {
            ReservationDate = model.ReservationDate,
            ReservationTime = model.ReservationTime,
            PartySize = model.PartySize
        };
        var selectedTable = (await _reservationService.SearchAvailableTablesAsync(search))
            .FirstOrDefault(t => t.TableId == model.TableId);

        if (selectedTable != null)
        {
            model.TableNumber = selectedTable.TableNumber;
            model.TableName = selectedTable.TableName;
            model.ImageUrl = selectedTable.ImageUrl;
            model.Description = selectedTable.Description;
            model.Area = selectedTable.Area;
            model.TableType = selectedTable.TableType;
            model.LocationNote = selectedTable.LocationNote;
            model.Capacity = selectedTable.Capacity;
        }

        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Challenge();
        }

        var reservation = await _reservationService.GetReservationDetailAsync(id, currentUserId.Value);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyReservations()
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Challenge();
        }

        var reservations = await _reservationService.GetReservationsForCustomerAsync(currentUserId.Value);
        return View(reservations);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, string? reason)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return Challenge();
        }

        var success = await _reservationService.CancelReservationAsync(id, currentUserId.Value, reason);
        if (success)
        {
            TempData["SuccessMessage"] = "Your table reservation has been cancelled.";
        }
        else
        {
            TempData["ErrorMessage"] = "This reservation cannot be cancelled.";
        }

        return RedirectToAction(nameof(MyReservations));
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUserId)
            ? currentUserId
            : null;
    }

    private static string GetCreateErrorMessage(TableReservationCreateStatus status)
    {
        return status switch
        {
            TableReservationCreateStatus.Unauthorized => "Please sign in before reserving a table.",
            TableReservationCreateStatus.TableNotFound => "Selected table was not found.",
            TableReservationCreateStatus.TableUnavailable => "Selected table is not available.",
            TableReservationCreateStatus.InvalidReservationTime => "Reservation time must be in the future.",
            TableReservationCreateStatus.OutsideOperatingHours => "Restaurant operates from 06:00 to 22:00.",
            TableReservationCreateStatus.InvalidPartySize => "Party size is invalid.",
            TableReservationCreateStatus.CapacityExceeded => "Party size exceeds table capacity.",
            TableReservationCreateStatus.Conflict => "This table is already reserved for the selected time.",
            TableReservationCreateStatus.ValidationFailed => "Please check your reservation information.",
            _ => "Table reservation could not be created."
        };
    }
}
