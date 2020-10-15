using System;
using Autofac;
using NPrismy.Logging;

namespace NPrismy.IOC
{
    internal static class AutofacModule
    {
        private static ILifetimeScope _container;
        public static ILifetimeScope Container 
        { 
            get
            {
                return _container;
            }
        }

        public static ContainerBuilder ContainerBuilder { get; set; } = new ContainerBuilder();

        public static void RegisterArtifacts()
        {
            AutofacModule.ContainerBuilder.RegisterType<IOLogger>().As<ILogger>();

            AutofacModule.ContainerBuilder.RegisterType<SqlCommandBuilder>().As<ISqlCommandBuilder>().SingleInstance();

            AutofacModule.ContainerBuilder.RegisterType<WhereBuilder>().SingleInstance();

            AutofacModule.ContainerBuilder.RegisterType<TableDefinitionOptions>().InstancePerDependency();

            AutofacModule.ContainerBuilder.RegisterType<TableDefinition>();

            AutofacModule.ContainerBuilder.RegisterType<TableDefinitionBuilder>().As<ITableDefinitionBuilder>().SingleInstance();
            
            //Registering generic EntityTable<>
            AutofacModule.ContainerBuilder.RegisterGeneric(typeof(EntityTable<>));

        }

        internal static void Initialize()
        {
            _container = ContainerBuilder.Build();
        }
    }
}