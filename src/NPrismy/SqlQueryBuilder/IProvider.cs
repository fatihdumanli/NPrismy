namespace NPrismy
{
    internal interface IProvider
    {
        TableDefinition GetTableDefinitionFor<T>();
        string ValueToString(object value, bool quote);
    }
}