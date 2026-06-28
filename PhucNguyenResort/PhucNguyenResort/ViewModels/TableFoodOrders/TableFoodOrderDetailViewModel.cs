using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableFoodOrders;

public class TableFoodOrderDetailViewModel
{
    public int FoodOrderId { get; set; }
    public int ReservationId { get; set; }
    public string ReservationCode { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string TableNumber { get; set; } = string.Empty;
    public int PartySize { get; set; }
    public FoodOrderStatus OrderStatus { get; set; }
    public KotStatus? KitchenStatus { get; set; }
    public DateTime OrderedAt { get; set; }
    public string? CustomerNote { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsChargedToRoom { get; set; }
    public bool IsPaidAtRestaurant { get; set; }
    public List<TableFoodOrderDetailItemViewModel> Items { get; set; } = new();
}

public class TableFoodOrderDetailItemViewModel
{
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? ItemNote { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
