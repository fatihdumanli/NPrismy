using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class SqlServerConnection 
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

        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            logger.LogInformation("query to exexute: " + query);
            var sqlCommand = new SqlCommand(query, connection);
            
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            var columnDefinitions = tableDefinition.GetColumnDefinitions();

            IList<T> queryResults = new List<T>();

            try
            {
                await connection.OpenAsync();
                using(SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                {
                   while (reader.Read())
                   {
                       var record = Activator.CreateInstance<T>();

                       foreach(var column in columnDefinitions)
                       {
                            var entityPropertyType = column.EntityPropertyType;
                            var columnOrdinal = reader.GetOrdinal(column.ColumnName);
                            var columnValue = reader.GetTypeValueNonGeneric(entityPropertyType, columnOrdinal);
                            record.GetType().GetProperty(column.PropertyName).SetValue(record, columnValue);
                       }
                        
                        queryResults.Add(record);
                   }

                }
            }
            

            catch(Exception e)
            {
                logger.LogError(e.Message);
            }

           await connection.CloseAsync();

           logger.LogInformation("queryResults count: " + queryResults.Count);
           return queryResults;            
        }
    }
}