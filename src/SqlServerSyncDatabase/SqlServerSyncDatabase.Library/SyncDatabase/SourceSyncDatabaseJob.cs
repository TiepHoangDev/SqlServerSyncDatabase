using SqlServerSyncDatabase.Objects.SyncDatabase;
using System.Data.Common;

namespace SqlServerSyncDatabase.Library.SyncDatabase
{
    public class SourceSyncDatabaseJob : ISourceSyncDatabaseJob
    {
        public async Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject)
        {
            if (infoBackupObject.PathFile == null)
            {
                var dir = $"{AppDomain.CurrentDomain.BaseDirectory}/backup/";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                infoBackupObject.PathFile = $"{dir}{infoBackupObject.DbConnection.Database}.diff.{DateTime.Now:yyyy.MM.dd.HH.mm.ss.fff}-{Guid.NewGuid()}.bak";
            }
            var query = $@"BACKUP DATABASE [{infoBackupObject.DbConnection.Database}] TO DISK = '{infoBackupObject.PathFile}' WITH DIFFERENTIAL, INIT;";
            using var row = await new FastQuery(infoBackupObject.DbConnection).WithQuery(query).ExecuteNumberOfRowsAsync();
            return row > 0;
        }

        public async Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject)
        {
            if (infoBackupObject.PathFile == null)
            {
                var dir = $"{AppDomain.CurrentDomain.BaseDirectory}/backup/";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                infoBackupObject.PathFile = $"{dir}{infoBackupObject.DbConnection.Database}.full.{DateTime.Now:yyyy.MM.dd.HH.mm.ss.fff}-{Guid.NewGuid()}.bak";
            }
            var query = $@"BACKUP DATABASE [{infoBackupObject.DbConnection.Database}] TO DISK = '{infoBackupObject.PathFile}' WITH INIT;";
            using var row = await infoBackupObject.DbConnection.CreateFastQuery().WithQuery(query).ExecuteNumberOfRowsAsync();
            return row > 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
