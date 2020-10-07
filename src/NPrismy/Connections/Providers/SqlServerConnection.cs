using System.Data.SqlClient;

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

        public void Open()
        {
            connection.Open();
        }
    }
}