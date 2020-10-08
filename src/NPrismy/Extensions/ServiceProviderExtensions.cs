using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{

    //Entry point for package. Acts like Program.cs
    public static class ServiceProviderExtensions
    {
        public static void AddNPrismy<T>(this IServiceCollection services, DatabaseOptions options) where T: Database
        {
            
            //Register database options as singleton (PersistanceProvider and connection string)
            services.AddSingleton<DatabaseOptions>(options);

            //Register consumer's Database object.
            services.AddScoped<T>();

            //Decide the connection object depends on Persistance Provider (SQL Server, Oracle DB or MySql)
            var connectionConcrete = PersistanceProviderFactory.GetProvider(options.Provider);

            //Register connection object to autofac container.
            //Remember that Connection object constructors requires a connection string!!!
            AutofacModule.ContainerBuilder.RegisterType(connectionConcrete).As<IConnection>()
                .WithParameter(new TypedParameter(typeof(string), options.ConnectionString));

            AutofacModule.ContainerBuilder.RegisterType<IOLogger>().As<ILogger>();

            AutofacModule.ContainerBuilder.RegisterType<EntityTableBuilder>().InstancePerDependency();

            //Registering generic EntityTable<>
            AutofacModule.ContainerBuilder.RegisterGeneric(typeof(EntityTable<>));

            AutofacModule.ContainerBuilder.RegisterGeneric(typeof(TableDefinition<>));

            AutofacModule.ContainerBuilder.RegisterType<TableDefinitionBuilder>().As<ITableDefinitionBuilder>();
        }


    }
}