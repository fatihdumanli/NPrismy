using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

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

            IOLogger logger = new IOLogger();
            logger.LogError("testststsfsd");
            connection.Open();
            //testing connection
            connection.Close();
        }
    }
    
}