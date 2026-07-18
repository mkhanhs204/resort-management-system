using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhucNguyenResort.Services.DatabaseBackups;
using PhucNguyenResort.ViewModels.DatabaseBackups;

namespace PhucNguyenResort.Controllers;

[Authorize(Roles = "System Admin")]
public class DatabaseBackupsController : Controller
{
    private readonly IDatabaseBackupService _databaseBackupService;

    public DatabaseBackupsController(IDatabaseBackupService databaseBackupService)
    {
        _databaseBackupService = databaseBackupService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new DatabaseBackupIndexViewModel
        {
            DatabaseName = _databaseBackupService.DatabaseName,
            BackupDirectory = _databaseBackupService.BackupDirectory,
            BackupFiles = await _databaseBackupService.GetBackupFilesAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBackup()
    {
        var result = await _databaseBackupService.CreateBackupAsync();

        if (result.Success)
        {
            TempData["SuccessMessage"] = $"Backup created successfully: {result.FileName}";
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to create database backup.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreBackup(string fileName, string confirmationText)
    {
        if (!string.Equals(confirmationText?.Trim(), "RESTORE", StringComparison.Ordinal))
        {
            TempData["ErrorMessage"] = "Restore confirmation failed. Please type RESTORE exactly before restoring a backup.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _databaseBackupService.RestoreBackupAsync(fileName);

        if (result.Success)
        {
            TempData["SuccessMessage"] = $"Database restored successfully from: {result.FileName}";
        }
        else
        {
            TempData["ErrorMessage"] = result.ErrorMessage ?? "Unable to restore database backup.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Download(string fileName)
    {
        var backupFile = await _databaseBackupService.GetBackupFileForDownloadAsync(fileName);

        if (backupFile == null)
        {
            return NotFound();
        }

        return PhysicalFile(
            backupFile.FullPath,
            backupFile.ContentType,
            backupFile.FileName);
    }
}
