namespace NPrismy
{
    internal class TableInfoProvider : ITableInfoProvider
    {
        public TableDefinition<T> GetTableDefinitionFor<T>()
        {
            return new TableDefinition<T>();
        }

        public string ValueToString(object value, bool quote)
        {
            return value.ToString();
        }
    }
}