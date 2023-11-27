namespace SqlServerSyncDatabase.Objects.SyncDatabase
{
    public interface IDestinationSyncDatabaseJob : IDisposable
    {
        Task<bool> RestoreBackupFullAsync(InfoBackupObject infoBackup);
        Task<bool> RestoreBackupDiffAsync(InfoBackupObject infoBackup);
    }

}
