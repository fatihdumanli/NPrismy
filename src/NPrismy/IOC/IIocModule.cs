namespace NPrismy.IOC
{
    public interface IIocModule
    {
        void Register<TIntf, TImpl>(RegistrationType type = RegistrationType.Scoped);
        void Register<TConcrete>(RegistrationType type = RegistrationType.Scoped);
    }
}