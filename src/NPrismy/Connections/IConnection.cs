using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    internal interface IConnection
    {
        Task Open();

        Task Close();
        
        Task<IEnumerable<T>> QueryAsync<T>(string query);

    }
}