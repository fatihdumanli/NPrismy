using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
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
                return _instance ?? new TableRegistry();
            }
        }


        public void RegisterTableDefinition<T>(TableDefinition<T> tableDefinition)
        {
            _tableDefinitions.Add(new KeyValuePair<Type, TableDefinition>(typeof(T), tableDefinition));          
            logger.LogInformation("Table definition registered successfully for type: " + typeof(T).Name);
        }


        public TableDefinition<T> GetTableDefinition<T>()
        {
            var def = _tableDefinitions.Where(td => td.Key == typeof(T)).SingleOrDefault();
            
            if(def.Value == null)
            {
                //TODO: throw new TableDefinitionNotFoundException
            }

            return def.Value as TableDefinition<T>;            
        }

       
    }
}