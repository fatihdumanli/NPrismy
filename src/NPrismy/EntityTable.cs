using System;
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
        public Database Database { get; internal set; }

        public EntityTable()
        {
            logger.LogInformation(" Entitytable class is initialized.");    
        }

        public async Task<T> FindByPrimaryKey(object value)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var readQuery = _sqlCommandBuilder.BuildFindByPrimaryKeyQuery<T>(pkValue: value);    
            var queryResult = await this.Database.Query<T>(query: readQuery);  
            var result = queryResult.SingleOrDefault();
            return result;
        }

        //uses transaction
        public async Task<T> Add(T entity)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var insertQuery = _sqlCommandBuilder.BuildInsertQuery<T>(entity);     
            entity = await this.Database.Insert(entity, insertQuery);    
            return entity;   
        }

        //uses transaction
        public async Task Update(T entity)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var updateQuery = _sqlCommandBuilder.BuildUpdateQuery<T>(entity);
            await this.Database.Update(updateQuery);
        }

        //Uses a transaction
        public async Task Delete(Expression<Func<T, bool>> expression)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var deleteQuery = _sqlCommandBuilder.BuildDeleteQuery<T>(expression);
            await this.Database.Delete(deleteQuery);
        }

        public async Task Delete(object primaryKey)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var deleteQuery = _sqlCommandBuilder.BuildDeleteQuery<T>(primaryKey);
            await this.Database.Delete(deleteQuery);
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

            var results = await this.Database.Query<T>(sqlQuery);
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
            logger.LogInformation("QUERY BUILT: " + sqlQuery);
            var results = await this.Database.Query<T>(sqlQuery);
            return results;
        }
        
    }

}