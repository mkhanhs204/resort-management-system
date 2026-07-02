using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.Tables;
using PhucNguyenResort.ViewModels.Tables;

namespace PhucNguyenResort.Controllers;

[Authorize(Roles = "System Admin, Business Manager, F&B Staff")]
public class TablesController : Controller
{
    private readonly IDiningTableService _tableService;

    public TablesController(IDiningTableService tableService)
    {
        _tableService = tableService;
    }

    public async Task<IActionResult> Index()
    {
        var tables = await _tableService.GetAllAsync();
        return View(tables);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new TableCreateVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TableCreateVM model)
    {
        if (ModelState.IsValid)
        {
            if (await _tableService.ExistsByTableNumberAsync(model.TableNumber))
            {
                ModelState.AddModelError("TableNumber", "Số bàn này đã tồn tại.");
                return View(model);
            }

            await _tableService.CreateAsync(model);
            TempData["SuccessMessage"] = "Thêm bàn mới thành công.";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var table = await _tableService.GetByIdAsync(id);
        if (table == null) return NotFound();

        var model = new TableEditVM
        {
            TableId = table.TableId,
            TableNumber = table.TableNumber,
            TableName = table.TableName,
            ImageUrl = table.ImageUrl,
            Description = table.Description,
            Area = table.Area,
            TableType = table.TableType,
            LocationNote = table.LocationNote,
            Capacity = table.Capacity,
            Status = table.Status,
            IsReservable = table.IsReservable,
            DisplayOrder = table.DisplayOrder
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TableEditVM model)
    {
        if (ModelState.IsValid)
        {
            if (await _tableService.ExistsByTableNumberAsync(model.TableNumber, model.TableId))
            {
                ModelState.AddModelError("TableNumber", "Số bàn này đã tồn tại ở một bàn khác.");
                return View(model);
            }

            var result = await _tableService.UpdateAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Cập nhật bàn thành công.";
                return RedirectToAction(nameof(Index));
            }
            
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _tableService.DeleteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Xóa bàn thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy bàn để xóa.";
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa bàn.";
        }

        return RedirectToAction(nameof(Index));
    }
}
