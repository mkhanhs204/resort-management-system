namespace PhucNguyenResort.Services.TableReservations;

public enum TableReservationCreateStatus
{
    Created,
    Unauthorized,
    TableNotFound,
    TableUnavailable,
    InvalidReservationTime,
    OutsideOperatingHours,
    InvalidPartySize,
    CapacityExceeded,
    Conflict,
    ValidationFailed
}
