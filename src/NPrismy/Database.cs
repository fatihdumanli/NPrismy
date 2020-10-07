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

            
            if(options.Provider == PersistanceProvider.SqlServer)
            {

            }             
        }

    }
    
}