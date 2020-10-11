using Autofac;
using NPrismy.IOC;

namespace NPrismy
{
    public class DatabaseOptionsBuilder<T>
    {
        private DatabaseOptions _databaseOptions;

        public DatabaseOptionsBuilder()
        {
            _databaseOptions = new DatabaseOptions();            
        }

        public DatabaseOptionsBuilder<T> UseProvider(PersistanceProvider provider)
        {
            _databaseOptions.Provider = provider;
            AutofacModule.ContainerBuilder.RegisterInstance<DatabaseOptions>(_databaseOptions);
            return this;
        }

        public DatabaseOptionsBuilder<T> ConnectionString(string connectionString)
        {
            _databaseOptions.ConnectionString = connectionString;
            AutofacModule.ContainerBuilder.RegisterInstance<DatabaseOptions>(_databaseOptions);

              
            //Decide the connection object depends on Persistance Provider (SQL Server, Oracle DB or MySql)
            var connectionConcrete = PersistanceProviderFactory.GetProvider(_databaseOptions.Provider);

            //Register connection object to autofac container.                        
            //Remember that Connection object constructors requires a connection string!!!
            AutofacModule.ContainerBuilder.RegisterType(connectionConcrete).As<IConnection>()
                .WithParameter(new TypedParameter(typeof(string), _databaseOptions.ConnectionString));

            this.Build();
            
            return this;
        }

        internal DatabaseOptions Build()
        {
         
            AutofacModule.Initialize();

            //Registering table definitions
            TableRegistry.Instance.RegisterTablesForDatabaseObject<T>();

            return _databaseOptions;
        }
    }
}