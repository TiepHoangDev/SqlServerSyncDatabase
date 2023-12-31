﻿using Microsoft.Data.SqlClient;
using SqlServerSyncDatabase.Library;
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
    public class ISourceSyncDatabaseJobTest : TestBase, ISourceSyncDatabaseJob
    {
        private ISourceSyncDatabaseJob _ISyncDatabaseJob;

        [SetUp]
        public void Init()
        {
            _ISyncDatabaseJob = new SourceSyncDatabaseJob();
        }

        public Task<bool> CreateBackupFullAsync(InfoBackupObject infoBackupObject) => _ISyncDatabaseJob.CreateBackupFullAsync(infoBackupObject);
        public Task<bool> CreateBackupDiffAsync(InfoBackupObject infoBackupObject) => _ISyncDatabaseJob.CreateBackupDiffAsync(infoBackupObject);

        [TearDown]
        public void Dispose()
        {
            _ISyncDatabaseJob.Dispose();
        }

        [TestCaseSource(nameof(CreateBackupTest_Inputtest))]
        public async Task CreateBackupTest(CreateBackupFullAsync_TestCaseSource inputTest)
        {
            {
                var info = inputTest.InfoBackupObject;
                var data = await CreateBackupFullAsync(info);
                Assert.IsTrue(data);
                Assert.IsTrue(File.Exists(info.PathFile));
                File.Delete(info.PathFile!);
            }
            {
                var info = inputTest.InfoBackupObject;
                var data = await CreateBackupDiffAsync(info);
                Assert.IsTrue(data);
                Assert.IsTrue(File.Exists(info.PathFile));
                File.Delete(info.PathFile!);
            }
        }
        public static IEnumerable<CreateBackupFullAsync_TestCaseSource> CreateBackupTest_Inputtest() => CreateBackupFullAsync_TestCaseSource.Samples();

    }

    public record CreateBackupFullAsync_TestCaseSource(InfoBackupObject InfoBackupObject)
    {
        public static IEnumerable<CreateBackupFullAsync_TestCaseSource> Samples()
        {
            var cs = SqlServerExecuterHelper.CreateConnectionString(TestBase.Config.Source.ServerName, TestBase.Config.Source.Database);
            yield return new CreateBackupFullAsync_TestCaseSource(new InfoBackupObject(SqlServerExecuterHelper.CreateConnection(cs)));
        }
    }
}

