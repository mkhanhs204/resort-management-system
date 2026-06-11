using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhucNguyenResort.ViewModels.RestaurantMenus;

public class MenuItemIndexViewModel
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsAvailable { get; set; }
    public List<SelectListItem> CategoryOptions { get; set; } = new();
    public IReadOnlyList<MenuItemListViewModel> Items { get; set; } = new List<MenuItemListViewModel>();
}
