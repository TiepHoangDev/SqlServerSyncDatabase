using Microsoft.Data.SqlClient;
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
            //Assert.That(cs.ConnectionString, Is.EqualTo(create.result));

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
            yield return new CreateConnectionString_InputTest(".", "A", "1", null) { isError = true };
            yield return new CreateConnectionString_InputTest("127.0.0.1a,1433", "Aqwe", null, null) { result = "Data Source=127.0.0.1a,1433;Initial Catalog=Aqwe;Encrypt=False;Trust Server Certificate=True", connectable = false };
            yield return new CreateConnectionString_InputTest("127.0.0.qwe1,1433", "Aqwe", null, null) { result = "Data Source=127.0.0.qwe1,1433;Initial Catalog=Aqwe;Encrypt=False;Trust Server Certificate=True", connectable = false };

            yield return new CreateConnectionString_InputTest(".", "A", null, null) { result = "Data Source=.;Initial Catalog=A;Encrypt=False;Trust Server Certificate=True", connectable = true };
            yield return new CreateConnectionString_InputTest(".", "A", " ", " ") { result = "Data Source=.;Initial Catalog=A;Encrypt=False;Trust Server Certificate=True", connectable = true };
            yield return new CreateConnectionString_InputTest(".", "A", null, null) { result = "Data Source=.;Initial Catalog=A;Encrypt=False;Trust Server Certificate=True", connectable = true };
            yield return new CreateConnectionString_InputTest(".", "A", "1", "1") { result = "Data Source=.;Initial Catalog=A;User ID=1;Password=1;Encrypt=False", connectable = true };
            yield return new CreateConnectionString_InputTest("127.0.0.1,1433", "A", "1", "1") { result = "Data Source=127.0.0.1,1433;Initial Catalog=A;User ID=1;Password=1;Encrypt=False", connectable = true };
            yield return new CreateConnectionString_InputTest("127.0.0.1,1433", "A", null, null) { result = "Data Source=127.0.0.1,1433;Initial Catalog=A;Encrypt=False;Trust Server Certificate=True", connectable = true };

        }


    }

    public record CreateConnectionString_InputTest(string server, string database = "master", string? username = null, string? pass = null)
    {
        public string? result { get; set; }
        public bool isError { get; set; }
        public bool connectable { get; set; }
    }
}
