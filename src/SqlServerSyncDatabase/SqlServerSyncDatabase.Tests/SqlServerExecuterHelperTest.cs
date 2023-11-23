﻿using Microsoft.Data.SqlClient;
using SqlServerSyncDatabase.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerSyncDatabase.Tests
{
    public class SqlServerExecuterHelperTest
    {
#if true
        const string DATABASE = "dbA";
        const string SQLVERVER = ".\\SQLEXPRESS";
        const int SQLPORT = 5566;
#else
        const string DATABASE = "A";
        const string SQLVERVER = ".";
        const int SQLPORT = 1433;
#endif

        [TestCaseSource(nameof(CreateConnectionStringData))]
        public void CreateConnection(CreateConnectionString_InputTest create)
        {
            if (create.isError)
            {
                Assert.Throws<ArgumentNullException>(() => SqlServerExecuterHelper.CreateConnectionString(create.server, create.database, create.username, create.pass));
                return;
            }

            var cs = SqlServerExecuterHelper.CreateConnectionString(create.server, create.database, create.username, create.pass);
            Assert.IsNotNull(cs);
            Assert.IsNotNull(cs.ConnectionString);
            Assert.IsNotEmpty(cs.ConnectionString);
            Console.WriteLine(cs.ConnectionString);

            using var ct = SqlServerExecuterHelper.CreateConnection(cs);

            if (create.connectable)
            {
                ct.Open();
                Assert.That(ct.State, Is.EqualTo(ConnectionState.Open));
                ct.Close();
                Assert.That(ct.State, Is.EqualTo(ConnectionState.Closed));
                return;
            }

            Assert.Throws<SqlException>(() => ct.Open());
        }


        public static IEnumerable<CreateConnectionString_InputTest> CreateConnectionStringData()
        {
            yield return new CreateConnectionString_InputTest(null) { isError = true };
            yield return new CreateConnectionString_InputTest(null, null) { isError = true };
            yield return new CreateConnectionString_InputTest(null, null, null, "1") { isError = true };
            yield return new CreateConnectionString_InputTest(null, null, "1", null) { isError = true };
            yield return new CreateConnectionString_InputTest(SQLVERVER, DATABASE, "1", null) { isError = true };
            yield return new CreateConnectionString_InputTest($"127.0.0.1a,{SQLPORT}", "Aqwe", null, null) { connectable = false };
            yield return new CreateConnectionString_InputTest($"127.0.0.qwe1,{SQLPORT}", "Aqwe", null, null) { connectable = false };

            yield return new CreateConnectionString_InputTest(SQLVERVER, DATABASE, null, null) { connectable = true };
            yield return new CreateConnectionString_InputTest(SQLVERVER, DATABASE, " ", " ") { connectable = true };
            yield return new CreateConnectionString_InputTest(SQLVERVER, DATABASE, null, null) { connectable = true };
            yield return new CreateConnectionString_InputTest(SQLVERVER, DATABASE, "1", "1") { connectable = true };
            yield return new CreateConnectionString_InputTest($"127.0.0.1,{SQLPORT}", DATABASE, "1", "1") { connectable = true };
            yield return new CreateConnectionString_InputTest($"127.0.0.1,{SQLPORT}", DATABASE, null, null) { connectable = true };

        }


    }

    public record CreateConnectionString_InputTest(string server, string database = "master", string? username = null, string? pass = null)
    {
        public bool isError { get; set; }
        public bool connectable { get; set; }
    }
}
