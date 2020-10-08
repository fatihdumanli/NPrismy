namespace NPrismy
{
    internal interface ITableDefinitionBuilder
    {
        TableDefinition<T> Build<T>();
    }
}