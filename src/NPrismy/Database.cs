using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
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
        private IConnection _connection
        {
            get
            {
                //Needs instance per call.
                //Connection per command.
                return AutofacModule.Container.Resolve<IConnection>();
            }
        }

        private ILogger logger = AutofacModule.Container.Resolve<ILogger>();

        public Database()
        {
            this._changeTracker = new ChangeTracker();
            logger.LogInformation("Database object is instantiated: " + this.GetType().Name);
        }

        
        //Property (EntityTable<>) instantiation must be performed here!
        public Database(DatabaseOptions options) : this()
        {
            this._options = options;  

            /* BEGIN: Injecting this database object to EntityTable<T>'s 'Database' property */       
            var properties = this.GetType().GetProperties();
            foreach(var property in properties)
            {   
                /* BEGIN: Intantiating EntityTable<T> property of Database object */
                var propertyName = property.Name;
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

        public void Commit()
        {
        }

        internal async Task<IEnumerable<T>> Query<T>(string query)
        {
           return await this._connection.QueryAsync<T>(query);
        }
        
        /// <summary>
        /// Define table columns, schemas by overriding this method.
        /// </summary>
        /// <param name="entityTableBuilder"></param>
        protected abstract void ConfigureTables(EntityTableBuilder entityTableBuilder);
    }
    
}