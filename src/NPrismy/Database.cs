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
        private ILogger _logger;

        public Database()
        {
        }

        public Database(DatabaseOptions options) : this()
        {
            this._options = options;      
            this._logger = AutofacModule.Container.ResolveOptional<ILogger>();

            var connection = AutofacModule.Container.ResolveOptional<IConnection>();
            
            connection.Open();
            _logger.LogInformation(" [x] CONNECTED!");
            connection.Close();

            var entityTableBuilder = AutofacModule.Container.ResolveOptional<EntityTableBuilder>();
            this.ConfigureTables(entityTableBuilder);
            entityTableBuilder.BuildTables();                
        }

        protected abstract void ConfigureTables(EntityTableBuilder entityTableBuilder);
    }
    
}