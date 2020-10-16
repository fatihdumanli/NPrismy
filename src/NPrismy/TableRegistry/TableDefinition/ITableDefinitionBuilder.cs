using System;

namespace NPrismy
{
    internal interface ITableDefinitionBuilder
    {
        TableDefinition Build(Type entityType, string tableName = null, string schemaName = null, 
            bool enableIdentityInsert = false, 
            ColumnDefinition[] privatePropertyColumns = null,
            string[] ignoredProperties = null);
    }
}