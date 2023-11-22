using SqlServerSyncDatabase.Objects.SyncDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerSyncDatabase.Tests
{
    public class ISyncDatabaseJobTest : ISyncDatabaseJob
    {
        private ISyncDatabaseJob _ISyncDatabaseJob;

        [SetUp]
        public void Init()
        {
            _ISyncDatabaseJob = new SyncDatabaseJob();
        }

        [Test]
        public void Dispose()
        {
            _ISyncDatabaseJob.Dispose();
            Assert.Pass();
        }
    }
}
