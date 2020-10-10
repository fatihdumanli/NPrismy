using System;
using System.Linq;
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
        private ILogger _logger = AutofacModule.Container.ResolveOptional<ILogger>();

        public Database()
        {
        }

        public Database(DatabaseOptions options) : this()
        {
            this._options = options;      

            /*
            var connection = AutofacModule.Container.ResolveOptional<IConnection>();
       

            connection.Open();
            _logger.LogInformation(" [x] CONNECTED!");
            connection.Close();
            */

        }
        
        /// <summary>
        /// Define table columns, schemas by overriding this method.
        /// </summary>
        /// <param name="entityTableBuilder"></param>
        protected abstract void ConfigureTables(EntityTableBuilder entityTableBuilder);
    }
    
}