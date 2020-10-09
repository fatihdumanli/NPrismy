using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{    
    internal class TableDefinition
    {  
        //PropertyName, ColumName mapping
        private List<ColumnDefinition> _columns;
        private string _tableName;
        private string _schemaName = "dbo";
        private Type _entityType;
        public TableDefinition(Type entityType)
        {   
            _entityType = entityType; 
            //Can be overrided by consumer.
            _tableName = entityType.Name.Pluralize();

            if(_columns == null)
                _columns = new List<ColumnDefinition>();
        }

      
        internal Type GetEntityType()
        {
            return _entityType;
        }
        internal string GetTableName()
        {
            return string.Format("{0}.{1}", _schemaName, _tableName);
        }
      
        internal List<ColumnDefinition> GetColumnDefinitions()
        {
            return _columns;
        }
        internal string GetColumnNameFor(string propertyName)
        {
            var column = _columns.Where(c => c.PropertyName == propertyName).SingleOrDefault();

            if(column.ColumnName == null)
            {
                throw new ColumnDefinitionNotFoundException(propertyName, _tableName);
            }

            return column.ColumnName;
        }

        internal void AddColumnDefinition(string propName, Type propertyType, string columnName)
        {
            _columns.Add(new ColumnDefinition(propName, propertyType, columnName));
        }

    }
}