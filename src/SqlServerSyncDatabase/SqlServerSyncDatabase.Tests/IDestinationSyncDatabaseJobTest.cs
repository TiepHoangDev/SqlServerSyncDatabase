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


        [Test]
        public void Dispose()
        {
            _IDestinationSyncDatabaseJob.Dispose();
            Assert.Pass();
        }

        [Test]
        public void RestoreBackupDiff()
        {
            _IDestinationSyncDatabaseJob.RestoreBackupDiff();
            Assert.Pass();
        }

        [Test]
        public void RestoreBackupFull()
        {
            _IDestinationSyncDatabaseJob.RestoreBackupFull();
            Assert.Pass();
        }
    }
}
