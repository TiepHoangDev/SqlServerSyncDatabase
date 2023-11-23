using System.Data;

namespace SqlServerSyncDatabase.Objects.SyncDatabase
{
    public class InfoBackupObject
    {
        public IDbConnection? DbConnection { get; }
        public string? PathFile { get; }
    }

}
