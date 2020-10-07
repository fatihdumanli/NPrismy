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
                    _container = _builder.Build();
                }

                return _container;
            }

        }

        private static ContainerBuilder _builder = new ContainerBuilder();
        
        public static void Register<TIntf>(Type impl)
        {
            _builder.RegisterType(impl).As<TIntf>().InstancePerLifetimeScope();
        }

        public static void Register<TIntf, TImpl>(RegistrationType type = RegistrationType.Scoped)
        {
            var b = _builder.RegisterType<TImpl>().As<TIntf>();

            switch(type)
            {
                case RegistrationType.Scoped:
                    b.InstancePerLifetimeScope();
                    break;

                case RegistrationType.Singleton:
                    b.SingleInstance();
                    break;

                case RegistrationType.Transient:
                    b.InstancePerDependency();
                    break;
            }
        }

        public static void Register<TConcrete>(RegistrationType type = RegistrationType.Scoped)
        {
             var b = _builder.RegisterType<TConcrete>();

            switch(type)
            {
                case RegistrationType.Scoped:
                    b.InstancePerLifetimeScope();
                    break;

                case RegistrationType.Singleton:
                    b.SingleInstance();
                    break;

                case RegistrationType.Transient:
                    b.InstancePerDependency();
                    break;
            }
        }
    }
}