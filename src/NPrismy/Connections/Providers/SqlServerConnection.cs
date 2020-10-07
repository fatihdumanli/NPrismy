using System.Data.SqlClient;
using System.Threading.Tasks;

namespace NPrismy
{
    public class SqlServerConnection 
        : IConnection
    {
        private static SqlConnection connection;

        public SqlServerConnection(string connStr)
        {
            //test conn
            connection = new SqlConnection(connStr);
        }

        public async Task Close()
        {
            await connection.CloseAsync();
        }

        public async Task Open()
        {
            await connection.OpenAsync();
        }
    }
}