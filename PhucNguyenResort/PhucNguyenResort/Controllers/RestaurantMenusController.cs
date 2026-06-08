using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.RestaurantMenus;
using PhucNguyenResort.ViewModels.RestaurantMenus;

namespace PhucNguyenResort.Controllers;

public class RestaurantMenusController : Controller
{
    private readonly IRestaurantMenuService _restaurantMenuService;

    public RestaurantMenusController(IRestaurantMenuService restaurantMenuService)
    {
        _restaurantMenuService = restaurantMenuService;
    }

    public async Task<IActionResult> Index()
    {
        var menuItems = await _restaurantMenuService.GetMenuItemsAsync();
        return View(menuItems);
    }

    public async Task<IActionResult> Details(int id)
    {
        var menuItem = await _restaurantMenuService.GetMenuItemDetailAsync(id);

        if (menuItem == null)
        {
            return NotFound();
        }

        return View(menuItem);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = await _restaurantMenuService.BuildCreateFormAsync();
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuItemFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await _restaurantMenuService.PopulateCategoryOptionsAsync(viewModel);
            return View(viewModel);
        }

        await _restaurantMenuService.CreateMenuItemAsync(viewModel);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _restaurantMenuService.BuildEditFormAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MenuItemFormViewModel viewModel)
    {
        if (id != viewModel.MenuItemId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await _restaurantMenuService.PopulateCategoryOptionsAsync(viewModel);
            return View(viewModel);
        }

        var updated = await _restaurantMenuService.UpdateMenuItemAsync(id, viewModel);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var menuItem = await _restaurantMenuService.GetMenuItemDetailAsync(id);

        if (menuItem == null)
        {
            return NotFound();
        }

        return View(menuItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await _restaurantMenuService.DeleteMenuItemAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
