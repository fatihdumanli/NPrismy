namespace NPrismy
{
    internal class DatabaseOptions
    {
        internal DatabaseOptions()
        {
            
        }
        internal DatabaseOptions(PersistanceProvider provider, string connString)
        {
            ConnectionString = connString;
            Provider = provider;
        }

        public PersistanceProvider Provider { get; internal set; }
        public string ConnectionString { get; internal set; }
        
    }
}