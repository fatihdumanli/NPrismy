using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    internal interface IConnection
    {
        Task OpenConnection();
        Task CloseConnection();
        bool IsOpen();        
        Task<IEnumerable<T>> QueryAsync<T>(string query);
        Task ExecuteCommand(string query);
        Task<object> ExecuteScalar(string query);
        Task BeginTransacionAsync();

        Task CommitTransactionAsync();

        Task RollBackTransactionAsync();

    }
}