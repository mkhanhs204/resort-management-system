using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class FoodOrderCreateRequest
{
    [Required]
    public int BookingId { get; set; }

    public int? BookingDetailId { get; set; }

    public DateTime? RequestedDeliveryAt { get; set; }
    public string? CustomerNote { get; set; }

    public List<FoodOrderItemRequest> Items { get; set; } = new();
}
