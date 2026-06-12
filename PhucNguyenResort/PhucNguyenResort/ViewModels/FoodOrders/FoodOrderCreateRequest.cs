using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class FoodOrderCreateRequest
{
    [Required]
    public int BookingId { get; set; }

    public List<FoodOrderItemRequest> Items { get; set; } = new();
}
