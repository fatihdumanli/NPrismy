using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Newtonsoft.Json;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class TableDefinitionBuilder : ITableDefinitionBuilder
    {
      
        //Builds table's column definitions.
        //Table name
        //Entity property name - database table column name mapping performed here.
        public TableDefinition Build(Type entityType, 
            string tableName = null,
            string schemaName = null, 
            bool enableIdentityInsert = false,
            ColumnDefinition[] privatePropertyColumns = null,
            string[] ignoredProperties = null,
            string pkPropertyName = null)
        {
            var tableDefinitionOptions = AutofacModule.Container.Resolve<TableDefinitionOptions>
            (new NamedParameter("entityType", entityType), 
            new NamedParameter("tableName", tableName),
            new NamedParameter("schema", schemaName),
            new NamedParameter("enableIdentityInsert", enableIdentityInsert));

            var tableDefinition = AutofacModule.Container.ResolveOptional<TableDefinition>(new NamedParameter("options", tableDefinitionOptions));
    
            PropertyInfo[] entityTypeProperties = entityType.GetProperties();

            /* BEGIN: Adding public properties to TableDefinition */
            foreach(var prop in entityTypeProperties)
            {
                
                //Adding KeyValuePair<string, string> to columns collection of TableDefinition.
                //Note that this is a default assignment. 
                //Consumer assembly can ovveride this definition.
                tableDefinition.AddColumnDefinition(prop.Name, prop.PropertyType, prop.Name, 
                    isPk: (pkPropertyName != null && pkPropertyName.Equals(prop.Name)) || prop.Name.ToUpper().Equals("ID"), //default
                    isIdentity: (pkPropertyName != null && pkPropertyName.Equals(prop.Name)) || prop.Name.ToUpper().Equals("ID"), //default
                    IsNavigationProperty: !(prop.PropertyType.IsPrimitive  || prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)),
                    isExcluded: ignoredProperties.Contains(prop.Name));
            }
            /* END: Adding public properties to TableDefinition */
            
            /* BEGIN: Adding private properties to TableDefinition */
            tableDefinition.AddColumnDefinition(privatePropertyColumns);
                        
            /* BEGIN: Adding private properties to TableDefinition */
        
            return tableDefinition;
        }
    }
}