namespace NPrismy
{
    public class DatabaseOptions
    {
        private DatabaseOptions() {}

        public DatabaseOptions(PersistanceProvider provider, string connString)
        {
            ConnectionString = connString;
            Provider = provider;
        }

        public PersistanceProvider Provider { get; private set; }
        public string ConnectionString { get; private set; }
        
    }
}