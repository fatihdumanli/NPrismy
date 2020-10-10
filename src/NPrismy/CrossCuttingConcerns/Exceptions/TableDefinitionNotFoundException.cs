using System;

namespace NPrismy.Exceptions
{
    public sealed class TableDefinitionNotFoundException : Exception
    {
        internal TableDefinitionNotFoundException(string tableName) : base(string.Format("Table definition not found for Type: {0}", tableName))
        {   
        }
    }
}