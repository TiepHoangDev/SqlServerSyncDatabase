using Microsoft.Data.SqlClient;

namespace SqlServerSyncDatabase.Library
{
    public static class FastQueryExtention
    {
        public static FastQuery CreateFastQuery(this SqlConnection dbConnection)
        {
            return new FastQuery(dbConnection);
        }

    }
}
