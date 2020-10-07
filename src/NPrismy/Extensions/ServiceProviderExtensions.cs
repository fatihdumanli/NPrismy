using System;
using Microsoft.Extensions.DependencyInjection;
using NPrismy.Exceptions;

namespace NPrismy
{
    public static class ServiceProviderExtensions
    {
        public static void AddNPrismy<T>(this IServiceCollection services, DatabaseOptions options) where T: Database
        {
            services.AddScoped<T>();


            switch(options.Provider)
            {
                case PersistanceProvider.SqlServer:
                    //register SqlServerConnection to Autofac container;
                break;

                case PersistanceProvider.MySql:
                    break;

                case PersistanceProvider.Oracle:
                    break;

                default:
                    throw new PersistanceProviderNotFoundException();
            }

        }
    }
}