using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class TableDefinitionBuilder : ITableDefinitionBuilder
    {
      
        //Builds table's column definitions.
        //Table name
        //Entity property name - database table column name mapping performed here.
        public TableDefinition Build(Type entityType, string tableName = null, string schemaName = null, bool enableIdentityInsert = false)
        {
            //Todo: configure for tableName and schemaName.
            var tableDefinitionOptions = AutofacModule.Container.Resolve<TableDefinitionOptions>
            (new NamedParameter("entityType", entityType), 
            new NamedParameter("tableName", tableName),
            new NamedParameter("schema", schemaName),
            new NamedParameter("enableIdentityInsert", enableIdentityInsert));

            var tableDefinition = AutofacModule.Container.ResolveOptional<TableDefinition>(new NamedParameter("options", tableDefinitionOptions));
    
            PropertyInfo[] entityTypeProperties = entityType.GetProperties();

            foreach(var prop in entityTypeProperties)
            {
                //Adding KeyValuePair<string, string> to columns collection of TableDefinition.
                //Note that this is a default assignment. 
                //Consumer assembly can ovveride this definition.
                tableDefinition.AddColumnDefinition(prop.Name, prop.PropertyType, prop.Name, 
                    isPk: prop.Name.ToUpper().Equals("ID"), //default
                    isIdentity: prop.Name.ToUpper().Equals("ID"), //default
                    IsNavigationProperty: !(prop.PropertyType.IsPrimitive  || prop.PropertyType.IsValueType || prop.PropertyType == typeof(string)));
            }
        
            return tableDefinition;
        }
    }
}