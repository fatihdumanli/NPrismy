using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NPrismy.Exceptions;
using NPrismy.IOC;

namespace NPrismy
{
    public static class ServiceProviderExtensions
    {
        public static void AddNPrismy<T>(this IServiceCollection services, DatabaseOptions options) where T: Database
        {
            
            services.AddSingleton<DatabaseOptions>(options);
            services.AddScoped<T>();

            var connectionConcrete = PersistanceProviderFactory.GetProvider(options.Provider);
            AutofacModule.ContainerBuilder.RegisterType(connectionConcrete).As<IConnection>()
                .WithParameter(new TypedParameter(typeof(string), options.ConnectionString));
        }
    }
}