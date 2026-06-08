using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class FoodOrder
{
    public int FoodOrderId { get; set; }
    public int BookingId { get; set; }
    public FoodOrderStatus OrderStatus { get; set; } = FoodOrderStatus.Pending;

    [Range(0, 999999999)]
    public decimal? TotalAmount { get; set; }

    public DateTime OrderedAt { get; set; } = DateTime.Now;

    public Booking? Booking { get; set; }
    public ICollection<FoodOrderItem> FoodOrderItems { get; set; } = new List<FoodOrderItem>();
    public Kot? Kot { get; set; }
}
