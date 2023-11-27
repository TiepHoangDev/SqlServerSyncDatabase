using SqlServerSyncDatabase.Library;
using SqlServerSyncDatabase.Library.SyncDatabase;
using SqlServerSyncDatabase.Objects.SyncDatabase;

namespace SqlServerSyncDatabase.Tests
{
    public class IDestinationSyncDatabaseJobTest : IDestinationSyncDatabaseJob
    {
        private IDestinationSyncDatabaseJob _IDestinationSyncDatabaseJob;

        [SetUp]
        public void Init()
        {
            _IDestinationSyncDatabaseJob = new DestinationSyncDatabaseJob();
        }


        [TearDown]
        public void Dispose()
        {
            _IDestinationSyncDatabaseJob.Dispose();
        }

        public Task<bool> RestoreBackupFullAsync(InfoBackupObject infoBackup) => _IDestinationSyncDatabaseJob.RestoreBackupFullAsync(infoBackup);
        public Task<bool> RestoreBackupDiffAsync(InfoBackupObject infoBackup) => _IDestinationSyncDatabaseJob.RestoreBackupDiffAsync(infoBackup);

        [TestCaseSource(nameof(CreateBackupTest_Inputtest))]
        public async Task RestoreBackupFull(CreateBackupFullAsync_TestCaseSource infoBackup)
        {
            if (!await infoBackup.InfoBackupObject.DbConnection.CheckDatabaseExistsAsync())
            {
                using var master = infoBackup.InfoBackupObject.DbConnection.NewOpenConnectToDatabase("master");
                await master.CreateFastQuery()
                    .WithQuery("CREATE DATABASE [A]")
                    .ExecuteNumberOfRowsAsync();
            }
            var backup = await new SourceSyncDatabaseJob().CreateBackupFullAsync(infoBackup.InfoBackupObject);
            Assert.IsTrue(backup);

            var data = await RestoreBackupFullAsync(infoBackup.InfoBackupObject);
            Assert.IsTrue(data);

            File.Delete(infoBackup.InfoBackupObject.PathFile!);
        }

        [TestCaseSource(nameof(CreateBackupTest_Inputtest))]
        public async Task RestoreBackupDiffTest(CreateBackupFullAsync_TestCaseSource infoBackup)
        {
            Assert.ThrowsAsync<Exception>(async () => await RestoreBackupDiffAsync(infoBackup.InfoBackupObject));

            using var master = infoBackup.InfoBackupObject.DbConnection.NewOpenConnectToDatabase("master");
            var dbTest = $"source_{Guid.NewGuid():N}";

            await master.CreateFastQuery()
                    .WithQuery($"CREATE DATABASE [{dbTest}]")
                    .ExecuteNumberOfRowsAsync();


            using var source = infoBackup.InfoBackupObject.DbConnection.NewOpenConnectToDatabase(dbTest);

            var infoFull = infoBackup.InfoBackupObject with
            {
                DbConnection = source,
            };
            var infoDiff = infoBackup.InfoBackupObject with
            {
                DbConnection = source,
                PathFile = null
            };

            var backupFull = await new SourceSyncDatabaseJob().CreateBackupFullAsync(infoFull);
            Assert.IsTrue(backupFull);

            var backupDiff = await new SourceSyncDatabaseJob().CreateBackupDiffAsync(infoDiff);
            Assert.IsTrue(backupDiff);

            var datainfoFull = await RestoreBackupFullAsync(infoFull);
            Assert.IsTrue(datainfoFull);

            var data = await RestoreBackupDiffAsync(infoDiff);
            Assert.IsTrue(data);

            File.Delete(infoFull.PathFile!);
            File.Delete(infoDiff.PathFile!);
        }

        public static IEnumerable<CreateBackupFullAsync_TestCaseSource> CreateBackupTest_Inputtest() => CreateBackupFullAsync_TestCaseSource.Samples();

    }
}
