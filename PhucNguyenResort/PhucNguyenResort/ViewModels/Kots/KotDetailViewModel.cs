using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Kots;

public class KotDetailViewModel
{
    public int KotId { get; set; }
    public int FoodOrderId { get; set; }
    public FoodOrderType OrderType { get; set; }
    public KotStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RequestedDeliveryAt { get; set; }
    public string? CustomerNote { get; set; }
    public string? RoomNumber { get; set; }
    public string? TableName { get; set; }
    public string? TableNumber { get; set; }
    public List<KotItemViewModel> Items { get; set; } = new List<KotItemViewModel>();
}
