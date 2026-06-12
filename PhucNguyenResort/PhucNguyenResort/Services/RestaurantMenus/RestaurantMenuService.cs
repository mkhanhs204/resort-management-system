using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Models;
using PhucNguyenResort.ViewModels.RestaurantMenus;

namespace PhucNguyenResort.Services.RestaurantMenus;

public class RestaurantMenuService : IRestaurantMenuService
{
    private readonly ResortDbContext _context;

    public RestaurantMenuService(ResortDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MenuItemListViewModel>> GetMenuItemsAsync()
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Where(item => !item.IsDeleted)
            .Include(item => item.Category)
            .OrderBy(item => item.Category == null ? string.Empty : item.Category.CategoryName)
            .ThenBy(item => item.ItemName)
            .Select(item => new MenuItemListViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName ?? string.Empty,
                CategoryName = item.Category == null
                    ? "Uncategorized"
                    : item.Category.CategoryName ?? "Uncategorized",
                Price = item.Price,
                IsAvailable = item.IsAvailable
            })
            .ToListAsync();
    }

    public async Task<MenuItemIndexViewModel> BuildIndexAsync(MenuItemIndexViewModel filter)
    {
        filter.SearchTerm = NormalizeNullableDisplayText(filter.SearchTerm);

        var query = _context.MenuItems
            .AsNoTracking()
            .Where(item => !item.IsDeleted);

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(item => item.CategoryId == filter.CategoryId.Value);
        }

        if (filter.IsAvailable.HasValue)
        {
            query = query.Where(item => item.IsAvailable == filter.IsAvailable.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchPattern = $"%{filter.SearchTerm}%";
            query = query.Where(item => EF.Functions.Like(item.ItemName, searchPattern));
        }

        filter.CategoryOptions = await BuildCategoryOptionsAsync();
        filter.Items = await query
            .Include(item => item.Category)
            .OrderBy(item => item.Category == null ? string.Empty : item.Category.CategoryName)
            .ThenBy(item => item.ItemName)
            .Select(item => new MenuItemListViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                CategoryName = item.Category == null
                    ? "Uncategorized"
                    : item.Category.CategoryName,
                Price = item.Price,
                IsAvailable = item.IsAvailable
            })
            .ToListAsync();

        return filter;
    }

    public async Task<IReadOnlyList<MenuItemListViewModel>> GetAvailableMenuItemsAsync()
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Where(item => item.IsAvailable && !item.IsDeleted)
            .Include(item => item.Category)
            .OrderBy(item => item.Category == null ? string.Empty : item.Category.CategoryName)
            .ThenBy(item => item.ItemName)
            .Select(item => new MenuItemListViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                CategoryName = item.Category == null
                    ? "Uncategorized"
                    : item.Category.CategoryName,
                Price = item.Price,
                IsAvailable = item.IsAvailable
            })
            .ToListAsync();
    }

    public async Task<MenuItemDetailViewModel?> GetMenuItemDetailAsync(int id)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.MenuItemId == id && !item.IsDeleted)
            .Select(item => new MenuItemDetailViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName ?? string.Empty,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsAvailable = item.IsAvailable,
                CategoryName = item.Category == null
                    ? "Uncategorized"
                    : item.Category.CategoryName ?? "Uncategorized"
            })
            .FirstOrDefaultAsync();
    }

    public async Task<MenuItemFormViewModel> BuildCreateFormAsync()
    {
        var viewModel = new MenuItemFormViewModel();
        await PopulateCategoryOptionsAsync(viewModel);
        return viewModel;
    }

    public async Task<MenuItemFormViewModel?> BuildEditFormAsync(int id)
    {
        var viewModel = await _context.MenuItems
            .AsNoTracking()
            .Where(item => item.MenuItemId == id && !item.IsDeleted)
            .Select(item => new MenuItemFormViewModel
            {
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName ?? string.Empty,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsAvailable = item.IsAvailable,
                CategoryId = item.CategoryId
            })
            .FirstOrDefaultAsync();

        if (viewModel == null)
        {
            return null;
        }

        await PopulateCategoryOptionsAsync(viewModel);
        return viewModel;
    }

    public async Task PopulateCategoryOptionsAsync(MenuItemFormViewModel viewModel)
    {
        viewModel.CategoryOptions = await BuildCategoryOptionsAsync();
    }

    private async Task<List<SelectListItem>> BuildCategoryOptionsAsync()
    {
        return await _context.MenuCategories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .Select(category => new SelectListItem
            {
                Value = category.CategoryId.ToString(),
                Text = category.CategoryName ?? "Unnamed category"
            })
            .ToListAsync();
    }

    public async Task<bool> MenuItemNameExistsInCategoryAsync(string itemName, int? categoryId, int? excludedMenuItemId = null)
    {
        var normalizedName = NormalizeMenuItemName(itemName);

        return await _context.MenuItems
            .AsNoTracking()
            .AnyAsync(item =>
                !item.IsDeleted
                && item.ItemName.Trim().ToLower() == normalizedName
                && item.CategoryId == categoryId
                && (!excludedMenuItemId.HasValue || item.MenuItemId != excludedMenuItemId.Value));
    }

    public async Task CreateMenuItemAsync(MenuItemFormViewModel viewModel)
    {
        var menuItem = new MenuItem
        {
            ItemName = NormalizeDisplayName(viewModel.ItemName),
            Description = viewModel.Description,
            Price = viewModel.Price,
            ImageUrl = viewModel.ImageUrl,
            IsAvailable = viewModel.IsAvailable,
            IsDeleted = false,
            CategoryId = viewModel.CategoryId
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateMenuItemAsync(int id, MenuItemFormViewModel viewModel)
    {
        var menuItem = await _context.MenuItems
            .FirstOrDefaultAsync(item => item.MenuItemId == id && !item.IsDeleted);

        if (menuItem == null)
        {
            return false;
        }

        menuItem.ItemName = NormalizeDisplayName(viewModel.ItemName);
        menuItem.Description = viewModel.Description;
        menuItem.Price = viewModel.Price;
        menuItem.ImageUrl = viewModel.ImageUrl;
        menuItem.IsAvailable = viewModel.IsAvailable;
        menuItem.CategoryId = viewModel.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<MenuItemDiscontinueResult> DiscontinueMenuItemAsync(int id)
    {
        var menuItem = await _context.MenuItems
            .FirstOrDefaultAsync(item => item.MenuItemId == id && !item.IsDeleted);

        if (menuItem == null)
        {
            return MenuItemDiscontinueResult.NotFound;
        }

        menuItem.IsDeleted = true;
        menuItem.IsAvailable = false;
        await _context.SaveChangesAsync();
        return MenuItemDiscontinueResult.Discontinued;
    }

    private static string NormalizeMenuItemName(string itemName)
    {
        return itemName.Trim().ToLowerInvariant();
    }

    private static string NormalizeDisplayName(string itemName)
    {
        return itemName.Trim();
    }

    private static string? NormalizeNullableDisplayText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
