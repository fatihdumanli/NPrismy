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
    /// <summary>
    /// Representation of each table on persistance.
    /// Reads and commands are performed via this class.
    /// </summary>
    /// <typeparam name="T">Entity Type</typeparam>
    public class EntityTable<T> 
    {
        //Singleton
        private ISqlCommandBuilder _sqlCommandBuilder;
        ILogger logger = AutofacModule.Container.Resolve<ILogger>();

        public EntityTable()
        {
            logger.LogInformation(" Entitytable class is initialized.");            
        }


        /// <summary>
        /// Query the table without a where clause.
        /// Note that that method will return all of the records without a filter.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query()
        {   
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>();

            var connection = AutofacModule.Container.Resolve<IConnection>();
            var results = await connection.QueryAsync<T>(sqlQuery);

            return results;
        }
        
        /// <summary>
        /// Queries the table with a Where clause.
        /// </summary>
        /// <param name="e">LINQ predicate</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> e)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var sqlQuery = _sqlCommandBuilder.BuildReadQuery<T>(e);
            
            var connection = AutofacModule.Container.Resolve<IConnection>();
            var results = await connection.QueryAsync<T>(sqlQuery);
            return results;
        }
        
    }

}