using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class TableDefinitionBuilder : ITableDefinitionBuilder
    {
        //Builds table's column definitions.
        //Entity property name - database table column name mapping performed here.
        public TableDefinition<T> Build<T>()
        {
            var tableDefinition = AutofacModule.Container.ResolveOptional<TableDefinition<T>>();

            foreach(var prop in typeof(T).GetProperties())
            {
                //Adding KeyValuePair<string, string> to columns collection of TableDefinition.
                //Note that this is a default assignment. 
                //Consumer assembly can ovveride this definition.
                tableDefinition.AddColumnDefinition(prop.Name, prop.Name);
            }

            return tableDefinition;

        }
    }
}