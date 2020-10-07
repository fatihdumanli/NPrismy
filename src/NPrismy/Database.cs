using System;
using System.Linq;
using Autofac;
using Newtonsoft.Json;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
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
        
        //Consumer assembly must specify their tables by overriding this method.
        protected abstract void ConfigureTables(EntityTableBuilder entityTableBuilder);
    }
    
}