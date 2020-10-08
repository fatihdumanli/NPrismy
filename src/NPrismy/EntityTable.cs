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
        private ISqlCommandBuilder _sqlCommandBuilder;
        ILogger logger = AutofacModule.Container.Resolve<ILogger>();
        private IConnection connection = AutofacModule.Container.Resolve<IConnection>();

        T[] objects;

        public EntityTable()
        {
            logger.LogInformation(" Entitytable class is initialized.");            
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> e)
        {
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery(e);
            return objects.AsEnumerable<T>();
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