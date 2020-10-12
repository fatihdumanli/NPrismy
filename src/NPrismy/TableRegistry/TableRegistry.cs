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
                                

                var propertyType = property.PropertyType;
                
                if(propertyType.IsGenericType)
                {
                    var entityType = propertyType.GetGenericArguments()[0]; //EntityTable<T>
                    var tableDefinition = AutofacModule.Container.Resolve<ITableDefinitionBuilder>()
                        .Build(entityType, tableName: tableName, schemaName: schemaName);
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
                throw new TableDefinitionNotFoundException(nameof(T));
            }

            return def.Value;         
        }

       
    }
}