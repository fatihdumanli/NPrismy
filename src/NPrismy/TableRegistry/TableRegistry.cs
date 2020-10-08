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


        public void RegisterTableDefinition<T>(TableDefinition<T> tableDefinition)
        {
            try
            {
                _tableDefinitions.Add(new KeyValuePair<Type, TableDefinition>(typeof(T), tableDefinition));         
                logger.LogInformation("Table definition registered successfully for type: " + typeof(T).Name + " table name: " + tableDefinition.GetTableName()); 
                logger.LogInformation(" TableDefinitions count: " + _tableDefinitions.Count);

            }

            catch(Exception e)
            {
                logger.LogError("cant add table definition to registry: " + e.Message);
            }


        }


        public TableDefinition<T> GetTableDefinition<T>()
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

            return def.Value as TableDefinition<T>;            
        }

       
    }
}