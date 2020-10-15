using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy.Extensions
{

    //Entry point for package. Acts like Program.cs
    public static class ServiceProviderExtensions
    {
        public static DatabaseOptionsBuilder<T> AddNPrismy<T>(this IServiceCollection services) where T: Database
        {
            //Register consumer's Database object.
            services.AddScoped<T>();
           
            AutofacModule.RegisterArtifacts();

            return new DatabaseOptionsBuilder<T>();
        }


    }
}