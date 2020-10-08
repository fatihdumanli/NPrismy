using System;
using System.Collections.Generic;
using System.Linq;
using NPrismy.Exceptions;

namespace NPrismy
{    
    internal class TableDefinition
    {  
        //PropertyName, ColumName mapping
        private List<KeyValuePair<string, string>> _columns;
        private string _tableName;
        private string _schemaName;
        private Type _entityType;
        public TableDefinition(Type entityType)
        {   
            _entityType = entityType; 
            //Can be overrided by consumer.
            _tableName = entityType.Name;

            if(_columns == null)
                _columns = new List<KeyValuePair<string, string>>();
        }

      
        internal Type GetEntityType()
        {
            return _entityType;
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