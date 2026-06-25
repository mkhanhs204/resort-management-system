using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class RoomServiceOrderViewModel
{
    [Required]
    [Display(Name = "Booking")]
    public int BookingId { get; set; }

    public int? BookingDetailId { get; set; }
    public DateTime? RequestedDeliveryAt { get; set; }
    public string? CustomerNote { get; set; }

    public string CustomerName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public BookingStatus? BookingStatus { get; set; }
    public bool CanPlaceOrder { get; set; }
    public string? Message { get; set; }
    public int? ActiveCategoryId { get; set; }
    public string? ActiveCategoryName { get; set; }
    public List<RoomServiceRoomOptionViewModel> RoomOptions { get; set; } = new();
    public List<RoomServiceMenuItemViewModel> MenuItems { get; set; } = new();
}

public class RoomServiceRoomOptionViewModel
{
    public int BookingDetailId { get; set; }
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}

public class RoomServiceMenuItemViewModel
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ItemNote { get; set; }
}
