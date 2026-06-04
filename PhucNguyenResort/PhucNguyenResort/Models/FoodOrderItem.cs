using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.Models;

public class FoodOrderItem
{
    public int OrderItemId { get; set; }
    public int? FoodOrderId { get; set; }
    public int? MenuItemId { get; set; }

    [Range(1, 1000)]
    public int? Quantity { get; set; }

    [Range(0, 999999999)]
    public decimal? UnitPrice { get; set; }

    public FoodOrder? FoodOrder { get; set; }
    public MenuItem? MenuItem { get; set; }
}
