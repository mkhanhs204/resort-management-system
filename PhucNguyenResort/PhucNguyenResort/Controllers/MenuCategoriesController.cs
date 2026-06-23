using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.MenuCategories;
using PhucNguyenResort.ViewModels.MenuCategories;

namespace PhucNguyenResort.Controllers;

[Authorize(Roles = "System Admin,Business Manager,F&B Staff")]
public class MenuCategoriesController : Controller
{
    private readonly IMenuCategoryService _menuCategoryService;

    public MenuCategoriesController(IMenuCategoryService menuCategoryService)
    {
        _menuCategoryService = menuCategoryService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _menuCategoryService.GetMenuCategoriesAsync();
        return View(categories);
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _menuCategoryService.GetMenuCategoryDetailAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    public IActionResult Create()
    {
        return View(new MenuCategoryFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MenuCategoryFormViewModel viewModel)
    {
        await ValidateCategoryNameAsync(viewModel);

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        await _menuCategoryService.CreateMenuCategoryAsync(viewModel);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _menuCategoryService.BuildEditFormAsync(id);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MenuCategoryFormViewModel viewModel)
    {
        if (id != viewModel.CategoryId)
        {
            return BadRequest();
        }

        await ValidateCategoryNameAsync(viewModel, id);

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var updated = await _menuCategoryService.UpdateMenuCategoryAsync(id, viewModel);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _menuCategoryService.GetMenuCategoryDetailAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _menuCategoryService.DeleteMenuCategoryAsync(id);

        if (result == MenuCategoryDeleteResult.NotFound)
        {
            return NotFound();
        }

        if (result == MenuCategoryDeleteResult.HasMenuItems)
        {
            TempData["ErrorMessage"] = "Cannot delete this category because it still contains menu items.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateCategoryNameAsync(MenuCategoryFormViewModel viewModel, int? excludedCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(viewModel.CategoryName))
        {
            return;
        }

        var exists = await _menuCategoryService.CategoryNameExistsAsync(
            viewModel.CategoryName,
            excludedCategoryId);

        if (exists)
        {
            ModelState.AddModelError(nameof(viewModel.CategoryName), "A category with this name already exists.");
        }
    }
}
