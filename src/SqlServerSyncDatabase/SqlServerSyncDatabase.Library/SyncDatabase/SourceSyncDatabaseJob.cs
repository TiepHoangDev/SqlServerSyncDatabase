using SqlServerSyncDatabase.Objects.SyncDatabase;
using System.Data.Common;
using System.Xml.Linq;

namespace SqlServerSyncDatabase.Library.SyncDatabase
{
    public class SourceSyncDatabaseJob : ISourceSyncDatabaseJob
    {
        public async Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject)
        {
            var dbName = infoBackupObject.DbConnection.Database;
            using var master = infoBackupObject.DbConnection.NewOpenConnectToDatabase("master");
            if (infoBackupObject.PathFile == null)
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var filename = $"{dbName}.full.{DateTime.Now:yyyy.MM.dd-HH.mm.ss.fff}.bak";
                infoBackupObject.PathFile = Path.Combine(dir, filename);
            }
            var query = $@"BACKUP DATABASE [{dbName}] TO DISK = '{infoBackupObject.PathFile}' WITH DIFFERENTIAL, INIT;";
            using var row = await master.CreateFastQuery().WithQuery(query).ExecuteNumberOfRowsAsync();
            if (!File.Exists(infoBackupObject.PathFile)) return false;
            File.SetAttributes(infoBackupObject.PathFile, FileAttributes.ReadOnly);
            return true;
        }

        public async Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject)
        {
            var dbName = infoBackupObject.DbConnection.Database;
            using var master = infoBackupObject.DbConnection.NewOpenConnectToDatabase("master");
            if (!await master.CheckDatabaseExistsAsync(dbName))
            {
                throw new Exception($"{nameof(CreateBackupFullAsync)} failed. Database=[{dbName}] not exists on server=[{infoBackupObject.DbConnection.DataSource}].");
            }

            if (infoBackupObject.PathFile == null)
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var filename = $"{dbName}.full.{DateTime.Now:yyyy.MM.dd-HH.mm.ss.fff}.bak";
                infoBackupObject.PathFile = Path.Combine(dir, filename);
            }
            var query = $@"BACKUP DATABASE [{dbName}] TO DISK = '{infoBackupObject.PathFile}' WITH INIT;";
            using var row = await master.CreateFastQuery().WithQuery(query).ExecuteNumberOfRowsAsync();
            if (!File.Exists(infoBackupObject.PathFile)) return false;
            File.SetAttributes(infoBackupObject.PathFile, FileAttributes.ReadOnly);
            return true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
