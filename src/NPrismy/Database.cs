using Autofac;
using NPrismy.IOC;

namespace NPrismy
{
    public abstract class Database
    {
        private DatabaseOptions _options;

        public Database()
        {
        }

        public Database(DatabaseOptions options)
        {
            this._options = options;      
            var connection = AutofacModule.Container.ResolveOptional<IConnection>();
            
            connection.Open();
        }

    }
    
}