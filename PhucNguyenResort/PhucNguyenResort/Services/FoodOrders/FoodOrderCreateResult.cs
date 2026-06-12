namespace PhucNguyenResort.Services.FoodOrders;

public class FoodOrderCreateResult
{
    public FoodOrderCreateStatus Status { get; set; }
    public int? FoodOrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Message { get; set; }

    public static FoodOrderCreateResult Created(int foodOrderId, decimal totalAmount)
    {
        return new FoodOrderCreateResult
        {
            Status = FoodOrderCreateStatus.Created,
            FoodOrderId = foodOrderId,
            TotalAmount = totalAmount
        };
    }

    public static FoodOrderCreateResult Failed(FoodOrderCreateStatus status, string message)
    {
        return new FoodOrderCreateResult
        {
            Status = status,
            Message = message
        };
    }
}
