using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NPrismy.Exceptions;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    internal class TableRegistry
    {
        ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        
        private List<KeyValuePair<Type, TableDefinition>> _tableDefinitions;

        private TableRegistry()
        {
            if(_tableDefinitions == null)
                _tableDefinitions = new List<KeyValuePair<Type, TableDefinition>>();
        }

        private static TableRegistry _instance;

        public static TableRegistry Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TableRegistry();
                }

                return _instance;
            }
        }

        
        internal void RegisterTablesForDatabaseObject<T>()
        {
            logger.LogInformation("log from RegisterTablesForDatabaseObject");
            var databaseType = typeof(T);
            var databaseProperties = databaseType.GetProperties();

            foreach(var property in databaseProperties)
            {

                string tableName = null, schemaName = null;
                bool enableIdentityInsert = false;
                List<ColumnDefinition> _privatePropertyColumns = null;
                List<string> _ignoredProperties = null;

                //Check for '[TableName]' attribute of EntityTable<T>
                var tableNameAttribute = property.GetCustomAttributes(typeof(TableNameAttribute), false).FirstOrDefault();                
                if(tableNameAttribute != null)
                {
                    tableName = (tableNameAttribute as TableNameAttribute).TableName;
                }

                //Check for '[Schema]' attribute
                var schemaAttribute = property.GetCustomAttributes(typeof(SchemaAttribute), false).FirstOrDefault();

                if(schemaAttribute != null)
                {
                    schemaName = (schemaAttribute as SchemaAttribute).SchemaName;
                }

                //Check for '[EnableIdentityInsert]' attribute.
                var enableIdentityInsertAttribute = property.GetCustomAttributes(typeof(EnableIdentityInsertAttribute), false).FirstOrDefault();
                
                if(enableIdentityInsertAttribute != null)
                {
                    enableIdentityInsert = true;
                }


                //Check for '[IncudePrivateProperty]' attribute
                var includePrivatePropertyAttributes = property.GetCustomAttributes(typeof(IncludePrivatePropertyAttribute), false);

                if(includePrivatePropertyAttributes != null)
                {
                    _privatePropertyColumns = new List<ColumnDefinition>();

                    foreach(var attr in includePrivatePropertyAttributes)
                    {
                        IncludePrivatePropertyAttribute attribute = (IncludePrivatePropertyAttribute) attr;

                        _privatePropertyColumns
                            .Add(new ColumnDefinition(propName: attribute._propertyName, propertyType: attribute._propertyType, columnName: attribute._columnName));
                    }
                }


                //Check for '[IgnoreProperty]' attribute
                var ignorePropertyAttributes = property.GetCustomAttributes(typeof(IgnorePropertyAttribute), false);
                             
                if(ignorePropertyAttributes != null)
                {
                    _ignoredProperties = new List<string>();
                    foreach(var attr in ignorePropertyAttributes)
                    {
                        IgnorePropertyAttribute attribute = (IgnorePropertyAttribute) attr;

                        _ignoredProperties.Add(attribute.PropertyName);
                    }
                }

                                               

                var propertyType = property.PropertyType;
                
                //Check for EntityTable<>
                if(propertyType.IsGenericType && propertyType.GenericTypeArguments[0] == typeof(EntityTable<>))
                {
                    var entityType = propertyType.GetGenericArguments()[0]; //EntityTable<T>
                    var tableDefinition = AutofacModule.Container.Resolve<ITableDefinitionBuilder>()
                        .Build(entityType, tableName: tableName, schemaName: schemaName, enableIdentityInsert: enableIdentityInsert, 
                        privatePropertyColumns: _privatePropertyColumns.ToArray(),
                        ignoredProperties: _ignoredProperties.ToArray());
                        
                    TableRegistry.Instance.RegisterTableDefinition(tableDefinition);
                }                
            }             
          
        }

        private void RegisterTableDefinition(TableDefinition tableDefinition)
        {
            _tableDefinitions.Add(new KeyValuePair<Type, TableDefinition>(tableDefinition.GetEntityType(), tableDefinition));         
        }



        public TableDefinition GetTableDefinition(Type entityType)
        {
            var def = _tableDefinitions.Where(td => entityType.GetType() == td.Key.GetType()).SingleOrDefault();

            if(def.Value == null)
            {
                throw new TableDefinitionNotFoundException(entityType.Name);
            }

            return def.Value;         
        }


        public TableDefinition GetTableDefinition<T>()
        {
            logger.LogInformation(" GetTableDefinition called for type: " + typeof(T).Name);
            logger.LogInformation(" TableDefinitions count: " + _tableDefinitions.Count);

            var def = _tableDefinitions.Where(td => td.Key == typeof(T)).SingleOrDefault();
            
            if(def.Value == null)
            {
                throw new TableDefinitionNotFoundException(typeof(T).Name);
            }

            return def.Value;         
        }

       
    }
}