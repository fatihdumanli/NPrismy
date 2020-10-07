using System.Threading.Tasks;

namespace NPrismy
{
    public interface IConnection
    {
        Task Open();

        Task Close();

    }
}