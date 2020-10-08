using System.Collections.Generic;
using System.Linq;
using NPrismy.Exceptions;

namespace NPrismy
{
    internal class TableDefinition
    {
    }
    
    internal class TableDefinition<T> : TableDefinition
    {  
        //PropertyName, ColumName mapping
        private List<KeyValuePair<string, string>> _columns;
        private string _tableName;
        private string _schemaName;

        public TableDefinition()
        {   
            _tableName = typeof(T).Name;

            if(_columns == null)
                _columns = new List<KeyValuePair<string, string>>();
        }

        internal string GetTableName()
        {
            return _tableName;
        }
      
        internal string GetColumnNameFor(string propertyName)
        {
            var column = _columns.Where(c => c.Key == propertyName).SingleOrDefault();

            if(column.Value == null)
            {
                throw new ColumnDefinitionNotFoundException(propertyName, _tableName);
            }

            return column.Value;
        }

        internal void AddColumnDefinition(string propName, string columnName)
        {
            _columns.Add(new KeyValuePair<string, string>(propName, columnName));
        }

    }
}