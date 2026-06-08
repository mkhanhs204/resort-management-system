using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.MenuCategories;

namespace PhucNguyenResort.Services.MenuCategories;

public class MenuCategoryService : IMenuCategoryService
{
    private readonly ResortDbContext _context;

    public MenuCategoryService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MenuCategoryListViewModel>> GetMenuCategoriesAsync()
    {
        return await _context.MenuCategories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .Select(category => new MenuCategoryListViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName ?? string.Empty,
                MenuItemCount = category.MenuItems.Count
            })
            .ToListAsync();
    }

    public async Task<MenuCategoryDetailViewModel?> GetMenuCategoryDetailAsync(int id)
    {
        return await _context.MenuCategories
            .AsNoTracking()
            .Where(category => category.CategoryId == id)
            .Select(category => new MenuCategoryDetailViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName ?? string.Empty,
                MenuItemCount = category.MenuItems.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MenuCategoryFormViewModel?> BuildEditFormAsync(int id)
    {
        return await _context.MenuCategories
            .AsNoTracking()
            .Where(category => category.CategoryId == id)
            .Select(category => new MenuCategoryFormViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName ?? string.Empty
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CategoryNameExistsAsync(string categoryName, int? excludedCategoryId = null)
    {
        var normalizedName = categoryName.Trim().ToLower();

        return await _context.MenuCategories
            .AsNoTracking()
            .AnyAsync(category =>
                category.CategoryName != null
                && category.CategoryName.ToLower() == normalizedName
                && (!excludedCategoryId.HasValue || category.CategoryId != excludedCategoryId.Value));
    }

    public async Task CreateMenuCategoryAsync(MenuCategoryFormViewModel viewModel)
    {
        var menuCategory = new MenuCategory
        {
            CategoryName = viewModel.CategoryName.Trim()
        };

        _context.MenuCategories.Add(menuCategory);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateMenuCategoryAsync(int id, MenuCategoryFormViewModel viewModel)
    {
        var menuCategory = await _context.MenuCategories.FindAsync(id);

        if (menuCategory == null)
        {
            return false;
        }

        menuCategory.CategoryName = viewModel.CategoryName.Trim();

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<MenuCategoryDeleteResult> DeleteMenuCategoryAsync(int id)
    {
        var menuCategory = await _context.MenuCategories
            .Include(category => category.MenuItems)
            .FirstOrDefaultAsync(category => category.CategoryId == id);

        if (menuCategory == null)
        {
            return MenuCategoryDeleteResult.NotFound;
        }

        if (menuCategory.MenuItems.Count > 0)
        {
            return MenuCategoryDeleteResult.HasMenuItems;
        }

        _context.MenuCategories.Remove(menuCategory);
        await _context.SaveChangesAsync();
        return MenuCategoryDeleteResult.Deleted;
    }
}
