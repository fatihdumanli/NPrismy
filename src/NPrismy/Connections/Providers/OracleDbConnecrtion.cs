using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    internal class OracleDbConnection : IConnection
    {
        public Task BeginTransacionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task Close()
        {
            throw new System.NotImplementedException();
        }

        public Task CommitTransactionAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task ExecuteQuery(string query)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOpen()
        {
            throw new System.NotImplementedException();
        }

        public Task Open()
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