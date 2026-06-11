using PhucNguyenResort.ViewModels.RestaurantMenus;

namespace PhucNguyenResort.Services.RestaurantMenus;

public interface IRestaurantMenuService
{
    Task<IReadOnlyList<MenuItemListViewModel>> GetMenuItemsAsync();
    Task<MenuItemIndexViewModel> BuildIndexAsync(MenuItemIndexViewModel filter);
    Task<IReadOnlyList<MenuItemListViewModel>> GetAvailableMenuItemsAsync();
    Task<MenuItemDetailViewModel?> GetMenuItemDetailAsync(int id);
    Task<MenuItemFormViewModel> BuildCreateFormAsync();
    Task<MenuItemFormViewModel?> BuildEditFormAsync(int id);
    Task PopulateCategoryOptionsAsync(MenuItemFormViewModel viewModel);
    Task<bool> MenuItemNameExistsInCategoryAsync(string itemName, int? categoryId, int? excludedMenuItemId = null);
    Task CreateMenuItemAsync(MenuItemFormViewModel viewModel);
    Task<bool> UpdateMenuItemAsync(int id, MenuItemFormViewModel viewModel);
    Task<MenuItemDiscontinueResult> DiscontinueMenuItemAsync(int id);
}
