using System.ComponentModel.DataAnnotations;

namespace PhucNguyenResort.ViewModels.TableFoodOrders;

public class TableFoodOrderMenuItemViewModel
{
    public int MenuItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = "Uncategorized";
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }

    [Range(0, 1000)]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? ItemNote { get; set; }
}
