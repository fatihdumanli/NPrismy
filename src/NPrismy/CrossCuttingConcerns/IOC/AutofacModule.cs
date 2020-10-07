using System;
using Autofac;

namespace NPrismy.IOC
{
    public static class AutofacModule
    {
        private static ILifetimeScope _container;
        public static ILifetimeScope Container 
        { 
            get
            {
                if(_container == null)
                {
                    _container = ContainerBuilder.Build();
                }

                return _container;
            }

        }

        public static ContainerBuilder ContainerBuilder { get; set; } = new ContainerBuilder();

               
        
    }
}