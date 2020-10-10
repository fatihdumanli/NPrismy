using System;

namespace NPrismy
{
    internal class ColumnDefinition
    {
        public bool IsIdentity { get; private set; }
        public Type EntityPropertyType { get; private set; }
        public string PropertyName { get; private set; }
        public string ColumnName { get; private set; }

        public ColumnDefinition(string propName, Type propertyType, string columnName, bool isIdentity = false)
        {
            this.IsIdentity = isIdentity;
            this.PropertyName = propName;
            this.EntityPropertyType = propertyType;
            this.ColumnName = columnName;
        }

    }
}