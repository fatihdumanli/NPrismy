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
        public void Add(T entity)
        {
            
            _sqlCommandBuilder = AutofacModule.Container.Resolve<ISqlCommandBuilder>();
            var insertQuery = _sqlCommandBuilder.BuildInsertQuery<T>(entity);

            //add this insertquery to changeTracker

            
            logger.LogInformation("INSERT QUERY: " + insertQuery);
            //1. begin a transaction
            //2. log this add operation to somewhere

            //Detect parent object (Database) and access it's changeTracker.

        }

        //uses transaction
        public void AddRange(IEnumerable<T> entities)
        {
            
        }

        //uses transaction
        public void Update(T entity)
        {

        }

        //Uses a transaction
        public void Delete(T entity)
        {

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