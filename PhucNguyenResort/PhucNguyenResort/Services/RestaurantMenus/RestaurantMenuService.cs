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

    public async Task<MenuItemDetailViewModel?> GetMenuItemDetailAsync(int id)
    {
        return await _context.MenuItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.MenuItemId == id)
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
            .Where(item => item.MenuItemId == id)
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
        viewModel.CategoryOptions = await _context.MenuCategories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .Select(category => new SelectListItem
            {
                Value = category.CategoryId.ToString(),
                Text = category.CategoryName ?? "Unnamed category"
            })
            .ToListAsync();
    }

    public async Task CreateMenuItemAsync(MenuItemFormViewModel viewModel)
    {
        var menuItem = new MenuItem
        {
            ItemName = viewModel.ItemName,
            Description = viewModel.Description,
            Price = viewModel.Price,
            ImageUrl = viewModel.ImageUrl,
            IsAvailable = viewModel.IsAvailable,
            CategoryId = viewModel.CategoryId
        };

        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateMenuItemAsync(int id, MenuItemFormViewModel viewModel)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);

        if (menuItem == null)
        {
            return false;
        }

        menuItem.ItemName = viewModel.ItemName;
        menuItem.Description = viewModel.Description;
        menuItem.Price = viewModel.Price;
        menuItem.ImageUrl = viewModel.ImageUrl;
        menuItem.IsAvailable = viewModel.IsAvailable;
        menuItem.CategoryId = viewModel.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);

        if (menuItem == null)
        {
            return false;
        }

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync();
        return true;
    }
}
