using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.TableFoodOrders;

public class TableFoodOrderListViewModel
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
    public decimal TotalAmount { get; set; }
    public bool IsChargedToRoom { get; set; }
    public bool IsPaidAtRestaurant { get; set; }
}
