using System;

namespace NPrismy
{
    public sealed class TableNameAttribute : Attribute
    {
        private string _tableName;

        public string TableName
        {
            get
            {
                return _tableName;
            }
        }


        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }
}