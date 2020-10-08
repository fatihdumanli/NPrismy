using System;

namespace NPrismy
{
    internal interface ITableDefinitionBuilder
    {
        TableDefinition Build<T>();
        TableDefinition Build(Type entityType);
    }
}