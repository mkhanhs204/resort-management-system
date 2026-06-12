using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class RoomServiceOrderViewModel
{
    [Required]
    [Display(Name = "Booking")]
    public int BookingId { get; set; }

    public string CustomerName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public BookingStatus? BookingStatus { get; set; }
    public bool CanPlaceOrder { get; set; }
    public string? Message { get; set; }
    public List<RoomServiceMenuItemViewModel> MenuItems { get; set; } = new();
}

public class RoomServiceMenuItemViewModel
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
