using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using NPrismy.CrossCuttingConcerns.Exceptions;
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

        //Do not access this object from EntityTable<T> (remember aggregate pattern.)
        //One ChangeTracker instance per Database object
        private ChangeTracker _changeTracker;
        private IConnection _connection;
        
        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();

        public Database()
        {
            try
            {
                this._changeTracker = new ChangeTracker();
                this._connection = AutofacModule.Container.Resolve<IConnection>();
                logger.LogInformation("Database object is instantiated SUCCESSFULLY: " + this.GetType().Name);
            }

            catch(Exception e)
            {
                logger.LogError("ERROR: " + e.Message);
            }
           
        }

        
        //Property (EntityTable<>) instantiation must be performed here!
        public Database(DatabaseOptions options) : this()
        {
            this._options = options;  

            if(_options == null)
            {
               throw new DatabaseNotConfiguredException(this.GetType());
            }

            logger.LogInformation(options.ToString());

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

        public async void Commit()
        {
            await _connection.BeginTransacionAsync();
            
            var queries = _changeTracker.GetQueries();
            
            foreach(var query in queries)
            {
                await _connection.ExecuteQuery(query);
            }

            await _connection.CommitTransactionAsync();            

            //begin a transaction
            //read queries from changetracker
            //execute queries
            //commit transaction
            //dispose the changeTracker.
        }

        internal async Task<IEnumerable<T>> Query<T>(string query)
        {
           if(_connection == null)
           {
               throw new ActiveConnectionNotFoundException("An error has occured when trying to query database. The connection object is null.");
           }

           return await this._connection.QueryAsync<T>(query);
        }
        
        internal void Insert(string query)
        {
            this._changeTracker.AddQuery(query);
        }
        

        internal void Delete(string query)
        {
            logger.LogInformation("Delete query: " + query + " added to changeTracker.");
            this._changeTracker.AddQuery(query);
        }

        /// <summary>
        /// Configure table name, columns and schema.
        /// </summary>
        protected abstract void ConfigureTables();
    }
    
}