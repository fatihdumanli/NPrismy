using System;
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
        public TableDefinition Build<T>()
        {
            var tableDefinition = (TableDefinition) AutofacModule.Container.ResolveOptional<TableDefinition>(new NamedParameter("entityType", typeof(T)));

            foreach(var prop in typeof(T).GetProperties())
            {
                //Adding KeyValuePair<string, string> to columns collection of TableDefinition.
                //Note that this is a default assignment. 
                //Consumer assembly can ovveride this definition.
                tableDefinition.AddColumnDefinition(prop.Name, prop.PropertyType, prop.Name);
            }

            return tableDefinition;

        }

        public TableDefinition Build(Type entityType)
        {

            var tableDefinition = AutofacModule.Container.ResolveOptional<TableDefinition>(new NamedParameter("entityType", entityType));
    

            foreach(var prop in entityType.GetProperties())
            {
                //Adding KeyValuePair<string, string> to columns collection of TableDefinition.
                //Note that this is a default assignment. 
                //Consumer assembly can ovveride this definition.
                tableDefinition.AddColumnDefinition(prop.Name, prop.PropertyType, prop.Name);
            }
        
            return tableDefinition;
        }
    }
}