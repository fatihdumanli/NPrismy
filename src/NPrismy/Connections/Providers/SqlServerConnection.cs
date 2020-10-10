using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Autofac;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class SqlServerConnection 
        : IConnection
    {
        private static SqlConnection connection;
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        private SqlTransaction _currentTransaction;

        public SqlServerConnection(string connStr)
        {
            //test conn
            connection = new SqlConnection(connStr);
            logger.LogInformation("SqlServerConnection: Created a new instance.");
        }

        public async Task BeginTransacionAsync()
        {
            logger.LogInformation(" SqlServerConnection: Transaction beginning...");

            if(_currentTransaction != null)
            {
                throw new TransactionAlreadyExistsException();
            }

            try
            {
                await connection.OpenAsync();
                _currentTransaction = connection.BeginTransaction();
                logger.LogInformation("SqlServerConnection.BeginTransactionAsync(): Transaction began. _currentTransaction is: " + _currentTransaction);
            }

            catch(Exception e)
            {
                logger.LogError("SqlServerConnection.BeginTransactionAsync(): Transaction begin failed: " + e.Message);
            }
          
        }

        public async Task Close()
        {
            await connection.CloseAsync();
        }

        public async Task CommitTransactionAsync()
        {
            logger.LogInformation(" SqlServerConnection.CommitTransactionAsync(): Committing current transaction...");

            try
            {
                if(_currentTransaction == null)
                {
                    throw new TransactionNotFoundException();
                }
                
                await _currentTransaction.CommitAsync();
                await connection.CloseAsync();
                logger.LogInformation("SqlServerConnection.CommitTransactionAsync(): Transaction committed successfully. Connection closed.");
            }

            catch(Exception e)
            {
                logger.LogError("CommitAsync Error: " + e.Message);
            }
        }

        public async Task ExecuteQuery(string query)
        {
            var sqlCommand = new SqlCommand(query, connection);
            sqlCommand.Transaction = _currentTransaction;
            
            logger.LogInformation(" SqlServerConnection: Executing query: " + query);
            try
            {
                var result = await sqlCommand.ExecuteNonQueryAsync();
                logger.LogInformation(" SqlServerConnection.ExecuteQuery(): Query executed, affected rows: " + result);
            }

            catch(Exception ex)
            {
                logger.LogError("SqlServerConnection.ExecuteQuery(): " + ex.Message);
            }

        }

        public bool IsOpen()
        {
            return connection.State == System.Data.ConnectionState.Open;
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

        public Task RollBackTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}