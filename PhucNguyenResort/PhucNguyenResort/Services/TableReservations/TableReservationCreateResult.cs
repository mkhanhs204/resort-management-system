namespace PhucNguyenResort.Services.TableReservations;

public class TableReservationCreateResult
{
    public TableReservationCreateStatus Status { get; set; }
    public int? ReservationId { get; set; }
    public string? Message { get; set; }

    public static TableReservationCreateResult Created(int reservationId)
    {
        return new TableReservationCreateResult
        {
            Status = TableReservationCreateStatus.Created,
            ReservationId = reservationId
        };
    }

    public static TableReservationCreateResult Failed(TableReservationCreateStatus status, string message)
    {
        return new TableReservationCreateResult
        {
            Status = status,
            Message = message
        };
    }
}
