namespace SqlServerSyncDatabase.Objects.SyncDatabase
{
    public interface ISourceSyncDatabaseJob : IDisposable
    {
        Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject);
        Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject);
    }
}
