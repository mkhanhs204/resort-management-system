using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.Kots;

public class KotDetailViewModel
{
    public int KotId { get; set; }
    public int FoodOrderId { get; set; }
    public KotStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<KotItemViewModel> Items { get; set; } = new List<KotItemViewModel>();
}
