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

        public Task Close()
        {
            throw new System.NotImplementedException();
        }

        public Task CommitTransactionAsync()
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