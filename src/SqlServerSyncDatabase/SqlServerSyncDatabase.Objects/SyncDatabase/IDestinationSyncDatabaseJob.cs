namespace SqlServerSyncDatabase.Objects.SyncDatabase
{
    public interface IDestinationSyncDatabaseJob : IDisposable
    {
        void RestoreBackupFull();
        void RestoreBackupDiff();
    }

}
