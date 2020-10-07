using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NPrismy.Exceptions;
using NPrismy.IOC;

namespace NPrismy
{
    public static class ServiceProviderExtensions
    {
        public static void AddNPrismy<T>(this IServiceCollection services, DatabaseOptions options) where T: Database
        {
            services.AddScoped<T>();

            var connectionConcrete = PersistanceProviderFactory.GetProvider(options.Provider);
            AutofacModule.Register<IConnection>(connectionConcrete);

        }
    }
}