using PhucNguyenResort.ViewModels.DatabaseBackups;

namespace PhucNguyenResort.Services.DatabaseBackups;

public interface IDatabaseBackupService
{
    string DatabaseName { get; }
    string BackupDirectory { get; }

    Task<IReadOnlyList<DatabaseBackupFileViewModel>> GetBackupFilesAsync();
    Task<DatabaseBackupResult> CreateBackupAsync();
    Task<DatabaseBackupResult> RestoreBackupAsync(string fileName);
    Task<BackupFileDownload?> GetBackupFileForDownloadAsync(string fileName);
}
