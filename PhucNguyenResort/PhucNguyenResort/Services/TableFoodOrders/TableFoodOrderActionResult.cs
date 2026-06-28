namespace PhucNguyenResort.Services.TableFoodOrders;

public enum TableFoodOrderActionStatus
{
    Success,
    ReservationNotFound,
    ReservationNotSeated,
    NoOrders,
    AlreadyCharged,
    AlreadyPaid,
    NoLinkedBooking,
    NoFolio,
    InvalidState
}

public class TableFoodOrderActionResult
{
    public TableFoodOrderActionStatus Status { get; set; }
    public string? Message { get; set; }

    public static TableFoodOrderActionResult Succeeded()
        => new TableFoodOrderActionResult { Status = TableFoodOrderActionStatus.Success };

    public static TableFoodOrderActionResult Failed(TableFoodOrderActionStatus status, string message)
        => new TableFoodOrderActionResult { Status = status, Message = message };
}
