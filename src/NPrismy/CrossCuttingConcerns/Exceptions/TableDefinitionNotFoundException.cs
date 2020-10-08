using System;

namespace NPrismy.Exceptions
{
    internal sealed class TableDefinitionNotFoundException : Exception
    {
        public TableDefinitionNotFoundException(string tableName) : base(string.Format("Table definition not found for Type: {0}", tableName))
        {   
        }
    }
}