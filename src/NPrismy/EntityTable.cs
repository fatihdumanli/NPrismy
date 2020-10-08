using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using NPrismy.IOC;
using NPrismy.Logging;

namespace NPrismy
{
    public class EntityTable<T> 
    {
        //Singleton
        private ISqlCommandBuilder _sqlCommandBuilder;
        ILogger logger = AutofacModule.Container.Resolve<ILogger>();

        //Transient / Scoped
        private IConnection connection = AutofacModule.Container.Resolve<IConnection>();

        //Singleton
        private ITableDefinitionBuilder _tableDefinitionBuilder = AutofacModule.Container.Resolve<ITableDefinitionBuilder>();

        public EntityTable()
        {
            //Create table definition here.
            //Consumers should be able to override this definition later.
            var tableDefinition = _tableDefinitionBuilder.Build<T>();
            TableRegistry.Instance.RegisterTableDefinition<T>(tableDefinition);

            logger.LogInformation(" Entitytable class is initialized.");            
        }

        public IEnumerable<T> Query()
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>();
            logger.LogInformation("Query generated: " + sqlQuery);


            return null;
        }
        
        public IEnumerable<T> Query(Expression<Func<T, bool>> e)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>(e);
            logger.LogInformation("Query generated: " + sqlQuery);


            return null;
        }

        private string _tableName 
        {
            get 
            {
                return typeof(T).Name;
            }
        }

        
    }

}