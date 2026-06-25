using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class FoodOrderDetailViewModel
{
    public int FoodOrderId { get; set; }
    public int BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public FoodOrderStatus OrderStatus { get; set; }
    public KotStatus? KitchenStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderedAt { get; set; }
    public DateTime? RequestedDeliveryAt { get; set; }
    public string? CustomerNote { get; set; }
    public bool IsChargedToRoom { get; set; }
    public DateTime? ChargedAt { get; set; }
    public List<FoodOrderDetailItemViewModel> Items { get; set; } = new();
}

public class FoodOrderDetailItemViewModel
{
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ItemNote { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
