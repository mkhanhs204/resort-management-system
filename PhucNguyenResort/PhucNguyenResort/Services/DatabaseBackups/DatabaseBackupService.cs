using Microsoft.Data.SqlClient;
using PhucNguyenResort.ViewModels.DatabaseBackups;

namespace PhucNguyenResort.Services.DatabaseBackups;

public class DatabaseBackupService : IDatabaseBackupService
{
    private const string BackupExtension = ".bak";

    private readonly string _connectionString;

    public DatabaseBackupService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");

        var connectionBuilder = new SqlConnectionStringBuilder(_connectionString);
        DatabaseName = connectionBuilder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(DatabaseName))
        {
            throw new InvalidOperationException("DefaultConnection must include a database name.");
        }

        BackupDirectory = Path.Combine(environment.ContentRootPath, "Backups");
    }

    public string DatabaseName { get; }
    public string BackupDirectory { get; }

    public Task<IReadOnlyList<DatabaseBackupFileViewModel>> GetBackupFilesAsync()
    {
        EnsureBackupDirectory();

        var files = Directory
            .EnumerateFiles(BackupDirectory, $"*{BackupExtension}", SearchOption.TopDirectoryOnly)
            .Select(path =>
            {
                var fileInfo = new FileInfo(path);

                return new DatabaseBackupFileViewModel
                {
                    FileName = fileInfo.Name,
                    CreatedAt = fileInfo.CreationTime,
                    LastModifiedAt = fileInfo.LastWriteTime,
                    SizeBytes = fileInfo.Length
                };
            })
            .OrderByDescending(file => file.LastModifiedAt)
            .ToList();

        return Task.FromResult<IReadOnlyList<DatabaseBackupFileViewModel>>(files);
    }

    public async Task<DatabaseBackupResult> CreateBackupAsync()
    {
        EnsureBackupDirectory();

        var fileName = $"{DatabaseName}_{DateTime.Now:yyyyMMdd_HHmmss}{BackupExtension}";
        var fullPath = Path.Combine(BackupDirectory, fileName);

        try
        {
            var masterConnectionString = BuildMasterConnectionString();
            var databaseIdentifier = EscapeSqlIdentifier(DatabaseName);

            var commandText = $@"
BACKUP DATABASE {databaseIdentifier}
TO DISK = @backupPath
WITH COPY_ONLY, INIT, NAME = @backupName, STATS = 10;";

            await using var connection = new SqlConnection(masterConnectionString);
            await using var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = 300
            };

            command.Parameters.AddWithValue("@backupPath", fullPath);
            command.Parameters.AddWithValue("@backupName", $"{DatabaseName} full backup {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return DatabaseBackupResult.Succeeded(fileName);
        }
        catch (Exception ex)
        {
            return DatabaseBackupResult.Failed(
                $"Backup failed. Please check SQL Server backup permission and folder access. Detail: {ex.Message}");
        }
    }

    public async Task<DatabaseBackupResult> RestoreBackupAsync(string fileName)
    {
        EnsureBackupDirectory();

        var fullPath = ResolveBackupFileFullPath(fileName);
        if (fullPath == null)
        {
            return DatabaseBackupResult.Failed("Backup file was not found or is invalid.");
        }

        var masterConnectionString = BuildMasterConnectionString();
        var databaseIdentifier = EscapeSqlIdentifier(DatabaseName);

        try
        {
            SqlConnection.ClearAllPools();

            var commandText = $@"
ALTER DATABASE {databaseIdentifier} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE {databaseIdentifier}
FROM DISK = @backupPath
WITH REPLACE, RECOVERY, STATS = 10;
ALTER DATABASE {databaseIdentifier} SET MULTI_USER;";

            await using var connection = new SqlConnection(masterConnectionString);
            await using var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = 600
            };

            command.Parameters.AddWithValue("@backupPath", fullPath);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            SqlConnection.ClearAllPools();

            return DatabaseBackupResult.Succeeded(fileName);
        }
        catch (Exception ex)
        {
            await TrySetDatabaseMultiUserAsync(masterConnectionString, databaseIdentifier);
            SqlConnection.ClearAllPools();

            return DatabaseBackupResult.Failed(
                $"Restore failed. Please check SQL Server restore permission and backup file compatibility. Detail: {ex.Message}");
        }
    }

    public Task<BackupFileDownload?> GetBackupFileForDownloadAsync(string fileName)
    {
        EnsureBackupDirectory();

        var fullPath = ResolveBackupFileFullPath(fileName);
        if (fullPath == null)
        {
            return Task.FromResult<BackupFileDownload?>(null);
        }

        return Task.FromResult<BackupFileDownload?>(new BackupFileDownload
        {
            FileName = fileName,
            FullPath = fullPath
        });
    }

    private string? ResolveBackupFileFullPath(string fileName)
    {
        if (!IsSafeBackupFileName(fileName))
        {
            return null;
        }

        var fullPath = Path.GetFullPath(Path.Combine(BackupDirectory, fileName));
        var backupRoot = Path.GetFullPath(BackupDirectory);

        if (!fullPath.StartsWith(backupRoot, StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(fullPath))
        {
            return null;
        }

        return fullPath;
    }

    private static async Task TrySetDatabaseMultiUserAsync(string masterConnectionString, string databaseIdentifier)
    {
        try
        {
            var commandText = $"ALTER DATABASE {databaseIdentifier} SET MULTI_USER WITH ROLLBACK IMMEDIATE;";

            await using var connection = new SqlConnection(masterConnectionString);
            await using var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = 120
            };

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch
        {
            // Best-effort recovery only. The original restore error is returned to the UI.
        }
    }

    private string BuildMasterConnectionString()
    {
        var builder = new SqlConnectionStringBuilder(_connectionString)
        {
            InitialCatalog = "master"
        };

        return builder.ConnectionString;
    }

    private void EnsureBackupDirectory()
    {
        Directory.CreateDirectory(BackupDirectory);
    }

    private static string EscapeSqlIdentifier(string identifier)
    {
        return $"[{identifier.Replace("]", "]]")}]";
    }

    private static bool IsSafeBackupFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName) ||
            !fileName.EndsWith(BackupExtension, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
               fileName == Path.GetFileName(fileName);
    }
}
