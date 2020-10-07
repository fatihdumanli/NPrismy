using System;
using System.Collections.Generic;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    class EntityTableModel
    {
        public Type EntityType { get; set; }
        public string TableName { get; set; }

        public EntityTableModel(Type type, string tableName)
        {
            this.EntityType = type;
            this.TableName = tableName;
        }
    }
    public class EntityTableBuilder
    {     
        private List<EntityTableModel> _tables;

        private Type _entityType;
        private string _tableName;
        ILogger logger = AutofacModule.Container.ResolveOptional<ILogger>();

        public EntityTableBuilder()
        {
            _tables = new List<EntityTableModel>();            
        }
        
        public EntityTableBuilder DefineTableFor<T>()
        {
           
           if(_entityType != null)
           {
                _tables.Add(new EntityTableModel(_entityType, _tableName));
                logger.LogInformation(string.Format("Added table: {0} with name {1}", _entityType.ToString(), _tableName.ToString()));
           }   
            
            _entityType = typeof(T);
            return this;      
        }

        public EntityTableBuilder WithTableName(string name)
        {
            _tableName = name;
            return this;
        }

        internal void BuildTables()
        {
            List<object> list = new List<object>();

            foreach(var item in _tables)
            {
                //Determining entity type (i.e WeatherForecast)
                var type = item.EntityType;

                //Generating EntityTable generic type (i.e. EntityTable<WeatherForecast>)
                var concreteType = typeof(EntityTable<>).MakeGenericType(type);
                var entityTable = AutofacModule.Container.ResolveOptional(concreteType);
                list.Add(entityTable);
            }
        }
    }
}