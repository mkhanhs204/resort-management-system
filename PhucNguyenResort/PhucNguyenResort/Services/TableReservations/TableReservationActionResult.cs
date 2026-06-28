namespace PhucNguyenResort.Services.TableReservations;

public enum TableReservationActionStatus
{
    Success,
    NotFound,
    InvalidState,
    AlreadyCharged,
    NoCheckedInBooking,
    NoFolio,
    InvalidAmount
}

public class TableReservationActionResult
{
    public TableReservationActionStatus Status { get; set; }
    public string? ErrorMessage { get; set; }

    public static TableReservationActionResult Succeeded()
        => new() { Status = TableReservationActionStatus.Success };

    public static TableReservationActionResult Failed(TableReservationActionStatus status, string message)
        => new() { Status = status, ErrorMessage = message };
}
