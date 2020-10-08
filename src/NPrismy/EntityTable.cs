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

            //Her scoped instance yaratıldığında table registration mı yapacağız?
            //Başka bir deyişle, her session-+ için neden table definition yapalım?
            //Bu işlem uygulama başlarken yapılmalı.

            //Create table definition here.
            //Consumers should be able to override this definition later.
            //var tableDefinition = _tableDefinitionBuilder.Build<T>();
            //TableRegistry.Instance.RegisterTableDefinition<T>(tableDefinition);

            logger.LogInformation(" Entitytable class is initialized.");            
        }

        public async Task<IEnumerable<T>> Query()
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>();
            logger.LogInformation("Query generated: " + sqlQuery);

            var connection = AutofacModule.Container.Resolve<IConnection>();
            await connection.QueryAsync<T>(sqlQuery);


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