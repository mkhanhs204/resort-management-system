namespace PhucNguyenResort.Services.DatabaseBackups;

public class DatabaseBackupResult
{
    public bool Success { get; set; }
    public string? FileName { get; set; }
    public string? ErrorMessage { get; set; }

    public static DatabaseBackupResult Succeeded(string fileName)
        => new() { Success = true, FileName = fileName };

    public static DatabaseBackupResult Failed(string message)
        => new() { Success = false, ErrorMessage = message };
}
