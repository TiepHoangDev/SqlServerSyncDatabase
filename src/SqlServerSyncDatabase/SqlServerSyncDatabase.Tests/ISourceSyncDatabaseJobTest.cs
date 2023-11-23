using SqlServerSyncDatabase.Library.SyncDatabase;
using SqlServerSyncDatabase.Objects.SyncDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerSyncDatabase.Tests
{
    public class ISourceSyncDatabaseJobTest : ISourceSyncDatabaseJob
    {
        private ISourceSyncDatabaseJob _ISyncDatabaseJob;

        [SetUp]
        public void Init()
        {
            _ISyncDatabaseJob = new SourceSyncDatabaseJob();
        }

        [Test]
        public void Dispose()
        {
            _ISyncDatabaseJob.Dispose();
            Assert.Pass();
        }

        public Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject)
        {
            throw new NotImplementedException();
        }
    }
}
