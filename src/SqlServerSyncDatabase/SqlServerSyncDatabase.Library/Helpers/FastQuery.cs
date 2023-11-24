using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlServerSyncDatabase.Library
{
    public class FastQuery : IDisposable
    {
        private readonly SqlConnection _sqlConnection;
        private readonly SqlCommand _sqlCommand;

        public FastQuery(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlCommand = new SqlCommand();
        }

        public void Dispose()
        {
            _sqlCommand?.Dispose();
            if (_sqlConnection != null)
            {
                if (_sqlConnection.State != ConnectionState.Closed) _sqlConnection.Close();
            }
            GC.SuppressFinalize(this);
        }

        public FastQuery WithQuery(string commandText) => WithCustom(q => q.CommandText = commandText);
        public FastQuery WithParameters(Dictionary<string, object> parameters) => WithCustom(q => q.AddParameters(parameters));
        public FastQuery WithTransaction(SqlTransaction transaction) => WithCustom(q => q.Transaction = transaction);
        public FastQuery WithCommandType(CommandType commandType) => WithCustom(q => q.CommandType = commandType);
        public FastQuery WithTimeout(int commandTimeoutSecond) => WithCustom(q => q.CommandTimeout = commandTimeoutSecond);

        public FastQuery WithCustom(Action<SqlCommand> custom)
        {
            if (_sqlCommand == null) throw new Exception($"Call method {nameof(WithQuery)} to init {nameof(_sqlCommand)}");
            custom?.Invoke(_sqlCommand);
            return this;
        }

        public async Task<T> ExecuteAsync<T>(Func<SqlCommand, Task<T>> execute)
        {
            if (_sqlConnection.State != ConnectionState.Open) _sqlConnection.Open();
            return await execute.Invoke(_sqlCommand);
        }

        public async Task<FastQueryResult<List<T>>> ExecuteReadAsyncAs<T>() where T : class, new()
        {
            var reader = await ExecuteAsync(q => q.ExecuteReaderAsync());
            return new FastQueryResult<List<T>>(this, reader.ReadAs<T>().ToList());
        }

        public async Task<FastQueryResult<int>> ExecuteNumberOfRowsAsync() => new FastQueryResult<int>(this, await ExecuteAsync(q => q.ExecuteNonQueryAsync()));

    }
}
