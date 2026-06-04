using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class Kot
{
    public int KotId { get; set; }
    public int FoodOrderId { get; set; }
    public KotStatus Status { get; set; } = KotStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public FoodOrder? FoodOrder { get; set; }
}
