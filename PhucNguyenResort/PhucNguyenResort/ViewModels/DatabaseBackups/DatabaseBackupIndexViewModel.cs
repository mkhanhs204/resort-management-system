namespace PhucNguyenResort.ViewModels.DatabaseBackups;

public class DatabaseBackupIndexViewModel
{
    public IReadOnlyList<DatabaseBackupFileViewModel> BackupFiles { get; set; } =
        Array.Empty<DatabaseBackupFileViewModel>();

    public string DatabaseName { get; set; } = string.Empty;
    public string BackupDirectory { get; set; } = string.Empty;
}
