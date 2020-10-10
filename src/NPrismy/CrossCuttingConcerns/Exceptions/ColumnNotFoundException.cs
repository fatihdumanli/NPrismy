using System;

namespace NPrismy
{
    public sealed class ColumnNotFoundException : Exception
    {
        internal ColumnNotFoundException(Type entity, string entityPropertyName)
            : base(string.Format("A property named {0} defined in the {1} entity class, but column is not found in the table. Check your definitions.", entityPropertyName, entity.Name))
        {
            
        }
    }
}