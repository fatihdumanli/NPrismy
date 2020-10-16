using System;

namespace NPrismy
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IgnorePropertyAttribute : Attribute
    {
        public string PropertyName { get; private set; }

        public IgnorePropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }
}