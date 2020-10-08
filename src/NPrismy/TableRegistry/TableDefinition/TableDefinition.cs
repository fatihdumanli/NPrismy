using System.Collections.Generic;

namespace NPrismy
{
    internal class TableDefinition
    {
    }
    
    internal class TableDefinition<T> : TableDefinition
    {  
        //PropertyName, ColumName mapping
        private List<KeyValuePair<string, string>> _columns;
        
        public TableDefinition()
        {   
            if(_columns == null)
                _columns = new List<KeyValuePair<string, string>>();
        }
        internal string GetColumnNameFor(string propertyName)
        {
            return "SAMPLECOLUMN";
        }

        internal void AddColumnDefinition(string propName, string columnName)
        {
            _columns.Add(new KeyValuePair<string, string>(propName, columnName));
        }

    }
}