using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    internal class MySqlDbConnection : IConnection
    {
        public Task BeginTransacionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task CloseConnection()
        {
            throw new System.NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task ExecuteCommand(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> ExecuteScalar(string query)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOpen()
        {
            throw new System.NotImplementedException();
        }

        public Task OpenConnection()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task RollBackTransactionAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}