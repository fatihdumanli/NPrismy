using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    internal interface IConnection
    {
        bool IsOpen();
        Task Open();

        Task Close();
        
        Task<IEnumerable<T>> QueryAsync<T>(string query);
        Task ExecuteQuery(string query);
        Task<object> ExecuteScalar(string query);
        Task BeginTransacionAsync();

        Task CommitTransactionAsync();

        Task RollBackTransactionAsync();

    }
}