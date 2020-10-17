using System;

namespace NPrismy
{
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public string PropertyName { get; private set; }

        public PrimaryKeyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;            
        }
    }
}