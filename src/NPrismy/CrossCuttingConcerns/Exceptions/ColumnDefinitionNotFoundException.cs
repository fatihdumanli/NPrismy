using System;

namespace NPrismy.Exceptions
{
    internal class ColumnDefinitionNotFoundException : Exception
    {
        internal ColumnDefinitionNotFoundException(string columnName, string tableName) 
            : base(string.Format("Column definition is not found in table {0}. Column name: {1}", tableName, columnName))
        {
            
        }
    }
}