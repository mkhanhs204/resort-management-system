namespace PhucNguyenResort.Services.FoodOrders;

// Result object cho UpdateOrderStatusAsync va ChargeToRoomAsync (UC66, UC69)
public enum FoodOrderActionStatus
{
    Success,
    NotFound,       // FOOD-003: FoodOrderId khong ton tai
    InvalidState,   // FOOD-006: Chuyen trang thai khong hop le
    AlreadyCharged, // FOOD-012: Don hang da duoc charge roi (ADR-005)
    NoFolio,        // FOOD-008: Booking khong co GuestFolio
    Forbidden       // FOOD-010: Khong du quyen
}

public class FoodOrderActionResult
{
    public FoodOrderActionStatus Status { get; set; }
    public string? ErrorMessage { get; set; }

    public static FoodOrderActionResult Succeeded()
        => new FoodOrderActionResult { Status = FoodOrderActionStatus.Success };

    public static FoodOrderActionResult Failed(FoodOrderActionStatus status, string message)
        => new FoodOrderActionResult { Status = status, ErrorMessage = message };
}
