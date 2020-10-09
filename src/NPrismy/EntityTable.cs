using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        //Singleton
        private ITableDefinitionBuilder _tableDefinitionBuilder = AutofacModule.Container.Resolve<ITableDefinitionBuilder>();

        public EntityTable()
        {
            logger.LogInformation(" Entitytable class is initialized.");            
        }

        public async Task<IEnumerable<T>> Query()
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>();

            var connection = AutofacModule.Container.Resolve<IConnection>();
            var results = await connection.QueryAsync<T>(sqlQuery);

            return results;
        }
        
        public async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> e)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>(e);
            
            var connection = AutofacModule.Container.Resolve<IConnection>();
            var results = await connection.QueryAsync<T>(sqlQuery);
            return results;
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