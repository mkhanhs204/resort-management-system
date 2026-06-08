namespace PhucNguyenResort.ViewModels.RestaurantMenus;

public class MenuItemDetailViewModel
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public string CategoryName { get; set; } = "Uncategorized";
}
