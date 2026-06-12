using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class FoodOrderItemRequest
{
    [Required]
    public int MenuItemId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }
}
