namespace NPrismy
{
    internal interface ITableInfoProvider
    {
        TableDefinition<T> GetTableDefinitionFor<T>();
        string ValueToString(object value, bool quote);
    }
}