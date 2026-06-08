using PhucNguyenResort.ViewModels.MenuCategories;

namespace PhucNguyenResort.Services.MenuCategories;

public interface IMenuCategoryService
{
    Task<IReadOnlyList<MenuCategoryListViewModel>> GetMenuCategoriesAsync();
    Task<MenuCategoryDetailViewModel?> GetMenuCategoryDetailAsync(int id);
    Task<MenuCategoryFormViewModel?> BuildEditFormAsync(int id);
    Task<bool> CategoryNameExistsAsync(string categoryName, int? excludedCategoryId = null);
    Task CreateMenuCategoryAsync(MenuCategoryFormViewModel viewModel);
    Task<bool> UpdateMenuCategoryAsync(int id, MenuCategoryFormViewModel viewModel);
    Task<MenuCategoryDeleteResult> DeleteMenuCategoryAsync(int id);
}
