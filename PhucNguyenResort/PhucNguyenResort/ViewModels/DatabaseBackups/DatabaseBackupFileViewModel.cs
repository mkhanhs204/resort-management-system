namespace PhucNguyenResort.ViewModels.DatabaseBackups;

public class DatabaseBackupFileViewModel
{
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public long SizeBytes { get; set; }

    public string SizeDisplay => FormatFileSize(SizeBytes);

    private static string FormatFileSize(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB"];
        var size = (double)bytes;
        var unitIndex = 0;

        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:0.##} {units[unitIndex]}";
    }
}
