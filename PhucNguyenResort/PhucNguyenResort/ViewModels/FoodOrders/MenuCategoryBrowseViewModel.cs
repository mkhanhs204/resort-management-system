using PhucNguyenResort.Enums;

namespace PhucNguyenResort.ViewModels.FoodOrders;

public class MenuCategoryBrowseViewModel
{
    public int BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public BookingStatus? BookingStatus { get; set; }
    public bool CanPlaceOrder { get; set; }
    public string? Message { get; set; }
    public List<MenuCategoryCardViewModel> Categories { get; set; } = new();
}

public class MenuCategoryCardViewModel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int ItemCount { get; set; }
}
