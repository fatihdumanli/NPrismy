using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using NPrismy.CrossCuttingConcerns.Exceptions;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{

    /// <summary>
    /// A database abstraction.
    /// This class must be inherited for each database connection.
    /// </summary>
    public abstract class Database
    {
        private DatabaseOptions _options;

        private IConnection _connection;
        
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        
        //Property (EntityTable<>) instantiation must be performed here!
        public Database()
        {

             try
            {
                this._connection = AutofacModule.Container.Resolve<IConnection>();
                logger.LogInformation("Database object is instantiated SUCCESSFULLY: " + this.GetType().Name);
            }

            catch(Exception e)
            {
                logger.LogError("ERROR: " + e.Message);
                throw e;
            }


            this._options = AutofacModule.Container.Resolve<DatabaseOptions>();  
            if(_options == null)
            {
               throw new DatabaseNotConfiguredException(this.GetType());
            }


            try
            {
                /* BEGIN: Injecting this database object to EntityTable<T>'s 'Database' property */       
                var properties = this.GetType().GetProperties();
                logger.LogInformation(properties.ToString());
                foreach(var property in properties)
                {   
                    /* BEGIN: Intantiating EntityTable<T> property of Database object */
                    var propertyName = property.Name;
                    logger.LogInformation(propertyName);
                    var propertyType = property.PropertyType;
                    var propertyGenericArgument = propertyType.GetGenericArguments()[0];
                    var genericType = typeof(EntityTable<>).MakeGenericType(propertyGenericArgument);
                    this.GetType().GetProperty(propertyName).SetValue(this, Activator.CreateInstance(genericType));
                    /* BEGIN: Intantiating EntityTable<T> property of Database object */
                
                //Injecting EntityTable<T>'s Database property to this object
                propertyType.GetProperty("Database")
                    .SetValue(this.GetType().GetProperty(propertyName).GetValue(this), this);
            }

              /* END: Injecting this database object to EntityTable<T>'s 'Database' property */       
            }

            catch(Exception e)
            {
                logger.LogError("Database::ctor: " + e.Message);
            }
        }

        public virtual async void Commit()
        {
             await _connection.CommitTransactionAsync(); 
             await _connection.CloseConnection();
        }

        internal async Task<IEnumerable<T>> Query<T>(string query)
        {
           if(_connection == null)
           {
               throw new ActiveConnectionNotFoundException("An error has occured when trying to query database. The connection object is null.");
           }

           return await this._connection.QueryAsync<T>(query);
        }
        
        internal async Task<T> Insert<T>(T entity, string query)
        {
            //Query is executed, Id is determined. But object is not persited yet. Set database-generated id.
            try
            {
                var result = _connection.ExecuteScalar(query);

                /* BEGIN: Setting entity's PK property to database-generated id */
                var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
                var pkColumn = tableDefinition.GetPrimaryKeyColumnDefinition();
            
                //ExecuteScalar.Result's type might not compatible with entity's PK column, we need to convert it.
                var converted = Convert.ChangeType(await result, pkColumn.EntityPropertyType);
                typeof(T).GetProperty(pkColumn.PropertyName).SetValue(entity, converted);   

            } 
            catch(InvalidCastException invalidCastEx)
            {
                logger.LogWarning("Cannot convert DbNull. Details: " + invalidCastEx.Message);
            }
            
            catch(Exception e)
            {
                logger.LogError("Database.Insert():" + e.GetType().Name);
                //TODO: include inner exception.
                throw new CommandExecutionException(query);
            }
         
            /* END: Setting entity's PK property to database-generated id */
            return entity;
        }

        internal async Task Update(string command)
        {
            await _connection.ExecuteCommand(command);
        }
        

        internal async Task Delete(string command)
        {
            await _connection.ExecuteCommand(command);
        }

    }
    
}