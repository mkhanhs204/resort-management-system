using PhucNguyenResort.ViewModels.RestaurantMenus;

namespace PhucNguyenResort.Services.RestaurantMenus;

public interface IRestaurantMenuService
{
    Task<IReadOnlyList<MenuItemListViewModel>> GetMenuItemsAsync();
    Task<MenuItemDetailViewModel?> GetMenuItemDetailAsync(int id);
    Task<MenuItemFormViewModel> BuildCreateFormAsync();
    Task<MenuItemFormViewModel?> BuildEditFormAsync(int id);
    Task PopulateCategoryOptionsAsync(MenuItemFormViewModel viewModel);
    Task CreateMenuItemAsync(MenuItemFormViewModel viewModel);
    Task<bool> UpdateMenuItemAsync(int id, MenuItemFormViewModel viewModel);
    Task<bool> DeleteMenuItemAsync(int id);
}
