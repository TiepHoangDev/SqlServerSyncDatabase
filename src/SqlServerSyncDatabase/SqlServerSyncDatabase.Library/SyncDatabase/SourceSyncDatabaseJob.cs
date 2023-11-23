using SqlServerSyncDatabase.Objects.SyncDatabase;

namespace SqlServerSyncDatabase.Library.SyncDatabase
{
    public class SourceSyncDatabaseJob : ISourceSyncDatabaseJob
    {
        public Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
