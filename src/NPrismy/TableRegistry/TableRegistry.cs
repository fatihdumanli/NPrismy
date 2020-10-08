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
                var propertyType = property.PropertyType;
                
                if(propertyType.IsGenericType)
                {
                    var entityType = propertyType.GetGenericArguments()[0];
                    var tableDefinition = AutofacModule.Container.Resolve<ITableDefinitionBuilder>().Build(entityType);
                    TableRegistry.Instance.RegisterTableDefinition(tableDefinition);
                }                
            }             
          
        }

        private void RegisterTableDefinition(TableDefinition tableDefinition)
        {
            try
            {
                _tableDefinitions.Add(new KeyValuePair<Type, TableDefinition>(tableDefinition.GetEntityType(), tableDefinition));         
                logger.LogInformation("Table definition registered successfully for type: " + tableDefinition.GetEntityType() + " table name: " + tableDefinition.GetTableName()); 
                logger.LogInformation(" TableDefinitions count: " + _tableDefinitions.Count);

            }

            catch(Exception e)
            {
                logger.LogError("cant add table definition to registry: " + e.Message);
            }


        }


        public TableDefinition GetTableDefinition<T>()
        {
            logger.LogInformation(" GetTableDefinition called for type: " + typeof(T).Name);
            logger.LogInformation(" TableDefinitions count: " + _tableDefinitions.Count);

            foreach(var t in _tableDefinitions)
            {
                 logger.LogInformation("Existing table definition: " + t.Key);
            }
            var def = _tableDefinitions.Where(td => td.Key == typeof(T)).SingleOrDefault();
            
            if(def.Value == null)
            {
                throw new TableDefinitionNotFoundException(nameof(T));
            }

            return def.Value;         
        }

       
    }
}