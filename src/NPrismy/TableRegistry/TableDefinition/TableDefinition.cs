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
        public TableDefinition(TableDefinitionOptions options)
        {   
            _entityType = options.EntityType;

            if(!string.IsNullOrEmpty(options.TableName))
            {
                _tableName = options.TableName;
            } 

            else
            {
                _tableName = _entityType.Name.Pluralize();
            }
            
            if(!string.IsNullOrEmpty(options.Schema))
            {
                _schemaName = options.Schema;
            }

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
      

        internal ColumnDefinition GetPrimaryKeyColumnDefinition()
        {
            var pkColumn = _columns.Where(c => c.IsPrimaryKey).SingleOrDefault();
            return pkColumn;
        }

        internal IEnumerable<ColumnDefinition> GetColumnDefinitions(bool includeIdentity = false, bool includeNavigationProperties = false)
        {

            IEnumerable<ColumnDefinition> results = _columns;

            if(!includeIdentity)
            {
                results = _columns.Where(c => !c.IsIdentity);
            }

            if(!includeNavigationProperties)
            {
                results =  results.Where(c => !c.IsNavigationProperty);
            }
            
            return results;
            
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

        internal void AddColumnDefinition(string propName, 
            Type propertyType, 
            string columnName, 
            bool isPk = false,
            bool isIdentity = false,
            bool IsNavigationProperty = false)
        {
            _columns.Add(new ColumnDefinition(propName, propertyType, columnName,
                isPrimaryKey: isPk,
                isIdentity: isIdentity, 
                IsNavigationProperty: IsNavigationProperty));
        }

    }
}