using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Data.SqlClient;
using NPrismy.Exceptions;
using NPrismy.Extensions;
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


        private SqlTransaction GetCurrentTransaction()
        {
            if(_currentTransaction == null)
            {
                _currentTransaction = GetPersistentConnection().BeginTransaction();
            }

            return _currentTransaction;
        }


        private SqlConnection GetPersistentConnection()
        {
            if(!IsOpen())
                connection.Open();

            return connection;
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
                _currentTransaction = GetPersistentConnection().BeginTransaction();
                logger.LogInformation("SqlServerConnection.BeginTransactionAsync(): Transaction began. _currentTransaction is: " + _currentTransaction);
            }

            catch(Exception e)
            {
                logger.LogError("SqlServerConnection.BeginTransactionAsync(): Transaction begin failed: " + e.Message);
                throw e;
            }
          
        }

        public async Task CommitTransactionAsync()
        {

                if(GetCurrentTransaction() == null)
                {
                    throw new TransactionNotFoundException();
                }
                
                try
                {
                    logger.LogInformation(" SqlServerConnection.CommitTransactionAsync(): Committing current transaction... Connection state: " + connection.State);
                    await GetCurrentTransaction().CommitAsync();
                }

                catch(Exception e)
                {
                    logger.LogError(e.Message);
                    RollBackTransactionAsync();
                    throw e;
                }

                await this.CloseConnection();

                logger.LogInformation("SqlServerConnection.CommitTransactionAsync(): Transaction committed successfully. Connection closing...");
        }

        public async Task ExecuteCommand(string query)
        {
            //get persistent connection
            var conn = GetPersistentConnection();
            
            var sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Transaction = this.GetCurrentTransaction();
            
            logger.LogInformation(" SqlServerConnection: Executing query: " + query);
            try
            {
                var result = await sqlCommand.ExecuteNonQueryAsync();
                logger.LogInformation(" SqlServerConnection.ExecuteQuery(): Query executed, affected rows: " + result);
            }

            catch(Exception ex)
            {
                logger.LogError("SqlServerConnection.ExecuteQuery(): " + ex.Message);
                throw ex;
            }

        }

        public async Task<object> ExecuteScalar(string query)
        {
           
            logger.LogInformation("ExecuteScalar query: " + query);
            var conn = GetPersistentConnection();
            var sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Transaction = this.GetCurrentTransaction();
     
            try
            {
                var result = await sqlCommand.ExecuteScalarAsync();
                return result;
            }

            catch(Exception ex)
            {
                logger.LogError("SqlServerConnection.ExecuteScalar(): " + ex.Message);
                throw ex;
            }
        }

        public bool IsOpen()
        {
            return connection.State == System.Data.ConnectionState.Open;
        }


        //TODO QUERY DOES NOT NEED A TRANSACTION
        //BUT EXECUTING A QUERY WITHOUT A TRANSACTION WHILE A TRANSACTION IS PROCEEDING,
        //CAUSES ERROR: BeginExecuteReader requires the command to have a transaction when the connection assigned to the command is in a pending local transaction.  The Transaction property of the command has not been initialized.
        //EITHER
        //Use a different connection when query is not need a transaction.
        //Or set query command's transaction property to current transaction
        public async Task<IEnumerable<T>> QueryAsync<T>(string query)
        {
            var conn = GetPersistentConnection();
            var sqlCommand = new SqlCommand(query, conn);
            sqlCommand.Transaction = this.GetCurrentTransaction();

            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            var columnDefinitions = tableDefinition.GetColumnDefinitions(includeIdentity: true);

            IList<T> queryResults = new List<T>();

            try
            {
                using(SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                {
                   while (reader.Read())
                   {
                       var record = Activator.CreateInstance<T>();

                       foreach(var column in columnDefinitions)
                       {
                            var entityPropertyType = column.EntityPropertyType;

                            int columnOrdinal = -1;

                            try
                            {
                                columnOrdinal = reader.GetOrdinal(column.ColumnName);
                            }

                            catch(IndexOutOfRangeException)
                            {
                                logger.LogWarning(string.Format("A property named {0} defined in the {1} entity class, but column is not found in the table. Skipping this property...", column.PropertyName, typeof(T).Name));
                                continue;
                                //throw new ColumnNotFoundException(typeof(T), entityPropertyName: column.PropertyName);
                            }
                            
                            var columnValue = reader.GetTypeValueNonGeneric(entityPropertyType, columnOrdinal);
                            record.GetType().GetProperty(column.PropertyName).SetValue(record, columnValue);
                       }
                        
                        queryResults.Add(record);
                   }

                }
            }
            

            catch(Exception e)
            {
                logger.LogError("Exception from SqlServerConnection.QueryAsync(): " + e.Message);
                throw e;
            }

    
           logger.LogInformation("queryResults count: " + queryResults.Count);
           return queryResults;            
        }

        public Task RollBackTransactionAsync()
        {
            return null;
        }

        public Task OpenConnection()
        {
            return connection.OpenAsync();
        }

        public Task CloseConnection()
        {
            return connection.CloseAsync();
        }
    }
}