using SqlServerSyncDatabase.Objects.SyncDatabase;
using System.ComponentModel.DataAnnotations;

namespace SqlServerSyncDatabase.Library.SyncDatabase
{
    public class DestinationSyncDatabaseJob : IDestinationSyncDatabaseJob
    {
        public void Dispose()
        {
        }

        public async Task<bool> RestoreBackupDiffAsync(InfoBackupObject infoBackup)
        {
            var file = infoBackup.PathFile;
            var dbName = infoBackup.DbConnection.Database;
            if (!File.Exists(file)) throw new Exception($"File Backup Diff not found for db [{dbName}]: {infoBackup.PathFile}");

            using var master_connection = infoBackup.DbConnection.NewOpenConnectToDatabase("master");
            var query = $"RESTORE DATABASE [{dbName}] FROM DISK = '{file}' WITH RECOVERY;";
            using var result = await master_connection.CreateFastQuery()
                .WithQuery(query)
                .ExecuteNumberOfRowsAsync();

            return true;
        }

        public async Task<bool> RestoreBackupFullAsync(InfoBackupObject infoBackup)
        {
            var file = infoBackup.PathFile;
            var dbName = infoBackup.DbConnection.Database;
            if (!File.Exists(file)) throw new Exception($"File Backup Full not found for db [{dbName}]: {infoBackup.PathFile}");

            using var master_connection = infoBackup.DbConnection.NewOpenConnectToDatabase("master");
            var db_copy = $"{dbName}_copy";

            //restore db_copy
            var queryRestore = $@"RESTORE DATABASE [{db_copy}] FROM  DISK = N'{file}' WITH REPLACE, NOUNLOAD,  STATS = 10";
            var query = $"RESTORE FILELISTONLY FROM DISK = '{file}';";
            using var result = await master_connection.CreateFastQuery().WithQuery(query).ExecuteReadAsyncAs<RESTORE_FILELISTONLY_Record>();
            if (result.Result.Any())
            {
                var getMoveQuery = (RESTORE_FILELISTONLY_Record filelistonly_record, string id) =>
                {
                    var extention = Path.GetExtension(filelistonly_record.PhysicalName) ?? throw new ArgumentNullException(nameof(filelistonly_record.PhysicalName));
                    var dir = Path.GetDirectoryName(filelistonly_record.PhysicalName) ?? throw new ArgumentNullException(nameof(filelistonly_record.PhysicalName));
                    var newPath = Path.Combine(dir, $"{db_copy}_{id}{extention}");
                    return $"MOVE N'{filelistonly_record.LogicalName}' TO N'{newPath}'";
                };

                var id = Guid.NewGuid().ToString("N");
                var queryMoves = result.Result.Select(q => getMoveQuery(q, id)).ToList();
                queryMoves.Insert(0, queryRestore);
                queryRestore = string.Join(", ", queryMoves);
            }

            using var restore_dbcopy = await master_connection.CreateFastQuery().WithQuery(queryRestore).ExecuteNumberOfRowsAsync();

            //checking db_copy
            var state = await master_connection.GetStateDatabase(db_copy);
            if (state?.Equals("ONLINE", StringComparison.OrdinalIgnoreCase) != true)
            {
                throw new Exception($"Restore db {db_copy} failed with state db = [{state}].");
            }

            //drop old db
            var checkExistsDb = await master_connection.CheckDatabaseExistsAsync(dbName);
            if (checkExistsDb)
            {
                var query_drop_old_db = $@"
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; 
                    DROP DATABASE [{dbName}]";
                using var dropResult = await master_connection
                    .CreateFastQuery()
                    .WithQuery(query_drop_old_db).ExecuteNumberOfRowsAsync();
            }

            //rename db-copy to db
            var query_rename = $@"
                ALTER DATABASE [{db_copy}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                ALTER DATABASE [{db_copy}] MODIFY NAME = [{dbName}];
                ALTER DATABASE [{dbName}] SET MULTI_USER;
                ";
            using var renameResult = await master_connection.CreateFastQuery()
                .WithQuery(query_rename)
                .ExecuteNumberOfRowsAsync();

            return await master_connection.CheckDatabaseOnline(dbName);
        }


        record RESTORE_FILELISTONLY_Record
        {
            public string? LogicalName { get; set; }
            public string? PhysicalName { get; set; }
            public string? Type { get; set; }
            public string? FileGroupName { get; set; }
        }

    }

}
