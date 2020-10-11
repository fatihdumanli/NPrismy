using System;
using System.Collections.Generic;
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


        //uses transaction
        //TODO: Return the entity with database generated ID
        public T Add(T entity)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var insertQuery = _sqlCommandBuilder.BuildInsertQuery<T>(entity);     
            this.Database.Insert(insertQuery);         
            return entity;   
        }

        //uses transaction
        public void Update(T entity)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
        }

        //Uses a transaction
        public void Delete(Expression<Func<T, bool>> expression)
        {
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var deleteQuery = _sqlCommandBuilder.BuildDeleteQuery<T>(expression);
            this.Database.Delete(deleteQuery);
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
            
            var results = await this.Database.Query<T>(sqlQuery);
            return results;
        }
        
    }

}