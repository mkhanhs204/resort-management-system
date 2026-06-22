using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.RestaurantMenus;
using PhucNguyenResort.ViewModels.RestaurantMenus;

namespace PhucNguyenResort.Controllers;

[Authorize(Roles = "System Admin,Business Manager,F&B Staff")]
public class RestaurantMenusController : Controller
{
    private readonly IRestaurantMenuService _restaurantMenuService;

    public RestaurantMenusController(IRestaurantMenuService restaurantMenuService)
    {
        _restaurantMenuService = restaurantMenuService;
    }

    public async Task<IActionResult> Index([FromQuery] MenuItemIndexViewModel filter)
    {
        var viewModel = await _restaurantMenuService.BuildIndexAsync(filter);
        return View(viewModel);
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
        await ValidateMenuItemNameAsync(viewModel);

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

        await ValidateMenuItemNameAsync(viewModel, id);

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

    public async Task<IActionResult> Discontinue(int id)
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
    public async Task<IActionResult> DiscontinueConfirmed(int id)
    {
        var result = await _restaurantMenuService.DiscontinueMenuItemAsync(id);

        if (result == MenuItemDiscontinueResult.NotFound)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Menu item discontinued successfully. It will no longer appear in active menus or customer ordering screens.";

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateMenuItemNameAsync(MenuItemFormViewModel viewModel, int? excludedMenuItemId = null)
    {
        if (string.IsNullOrWhiteSpace(viewModel.ItemName))
        {
            return;
        }

        var exists = await _restaurantMenuService.MenuItemNameExistsInCategoryAsync(
            viewModel.ItemName,
            viewModel.CategoryId,
            excludedMenuItemId);

        if (exists)
        {
            ModelState.AddModelError(
                nameof(viewModel.ItemName),
                "A menu item with this name already exists in the selected category.");
        }
    }
}
