using System;

namespace NPrismy
{
    internal class ColumnDefinition
    {
        public bool IsIdentity { get; private set; }

        //As default, assume the column with name 'ID' is the PK for the table,
        //But allow consumer assembly to override this definition
        public bool IsPrimaryKey { get; private set; }
        public bool IsExcluded { get; private set; }
        public bool IsNavigationProperty { get; private set; }
        public Type EntityPropertyType { get; private set; }
        public string PropertyName { get; private set; }
        public string ColumnName { get; private set; }

        public ColumnDefinition(string propName, 
            Type propertyType, 
            string columnName, 
            bool isPrimaryKey = false,
            bool isIdentity = false, 
            bool IsNavigationProperty = false,
            bool isExcluded = false)
        {
            this.IsPrimaryKey = isPrimaryKey;
            this.IsIdentity = isIdentity;
            this.IsNavigationProperty = IsNavigationProperty;
            this.PropertyName = propName;
            this.EntityPropertyType = propertyType;
            this.ColumnName = columnName;
            this.IsExcluded = isExcluded;
        }

    }
}