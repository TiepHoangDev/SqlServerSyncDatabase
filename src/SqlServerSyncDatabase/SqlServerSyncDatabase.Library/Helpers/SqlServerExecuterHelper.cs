using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlServerSyncDatabase.Library
{
    public class SqlServerExecuterHelper
    {
        public static IDbConnection CreateConnection(SqlConnectionStringBuilder sqlConnectionString)
        {
            var cs = sqlConnectionString.ConnectionString;
            return new SqlConnection(cs);
        }

        public static SqlConnectionStringBuilder CreateConnectionString(string server, string database = "master", string? username = null, string? pass = null)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = server ?? throw new ArgumentNullException(nameof(server));
            builder.InitialCatalog = database ?? throw new ArgumentNullException(nameof(database));
            //builder.Encrypt = false;

            //if (string.IsNullOrWhiteSpace(username))
            //{
            //    builder.TrustServerCertificate = string.IsNullOrWhiteSpace(username);
            //}

            if (!builder.TrustServerCertificate)
            {
                builder.UserID = username ?? throw new ArgumentNullException(nameof(username));
                builder.Password = pass ?? throw new ArgumentNullException(nameof(pass));
            }

            return builder;
        }
    }
}
