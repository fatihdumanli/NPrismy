using System;

namespace NPrismy
{
    public sealed class IncludePrivatePropertyAttribute : Attribute
    {
        public string _propertyName { get; private set; }
        public string _columnName { get; private set;}

        public Type _propertyType { get; private set; }
        
        public IncludePrivatePropertyAttribute(Type propertyType, string propertyName, string columnName)
        {
            _propertyName = propertyName;
            _columnName = columnName;   
            _propertyType = propertyType;         
        }
        
    }
}