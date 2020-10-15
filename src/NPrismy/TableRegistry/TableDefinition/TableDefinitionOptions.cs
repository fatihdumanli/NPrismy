using System;

namespace NPrismy
{
    internal sealed class TableDefinitionOptions 
    {
        public Type EntityType { get; private set; }
        public string TableName { get; private set; }
        public string Schema { get; private set; }
        public bool EnableIdentityInsert { get; private set; }
        
        public TableDefinitionOptions(Type entityType, string tableName = null, string schema = null, bool enableIdentityInsert = false)
        {
            this.EntityType = entityType;
            this.TableName = tableName;
            this.Schema = schema;      
            this.EnableIdentityInsert = enableIdentityInsert;
        }
    }
}