using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPrismy
{
    public interface IConnection
    {
        Task Open();

        Task Close();
        
        Task<IEnumerable<T>> QueryAsync<T>(string query);

    }
}