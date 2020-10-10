using System;

namespace NPrismy
{
    public sealed class SchemaAttribute : Attribute
    {
        private string _schemaName;
        public string SchemaName
        {
            get
            {
                return _schemaName;
            }
        }

        public SchemaAttribute(string schemaName)
        {
            _schemaName = schemaName;            
        }

        
    }
}