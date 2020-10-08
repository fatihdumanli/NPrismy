using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    public class SqlServerConnection 
        : IConnection
    {
        private static SqlConnection connection;
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
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

        public async Task QueryAsync<T>(string query)
        {
            var sqlCommand = new SqlCommand(query, connection);
            await connection.OpenAsync();

             using(SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
             {

                logger.LogInformation("Query executed in connection class.");
                while (reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("Id"));
                }
            }
            
            connection.CloseAsync();
        }
    }
}