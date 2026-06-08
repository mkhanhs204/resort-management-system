namespace PhucNguyenResort.ViewModels.RestaurantMenus;

public class MenuItemListViewModel
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}
