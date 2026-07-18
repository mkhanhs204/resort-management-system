namespace PhucNguyenResort.Services.DatabaseBackups;

public class BackupFileDownload
{
    public string FileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}
