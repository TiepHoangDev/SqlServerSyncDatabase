using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlServerSyncDatabase.Objects.SyncDatabase
{
    public record InfoBackupObject(SqlConnection DbConnection)
    {
        public string? PathFile { get; set; }
    }

}
