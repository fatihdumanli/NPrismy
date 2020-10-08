using System;
using System.Linq.Expressions;
using System.Text;
using Autofac;
using NPrismy.IOC;

namespace NPrismy
{
    internal class SqlCommandBuilder : ISqlCommandBuilder
    {
        private WhereBuilder _whereBuilder = AutofacModule.Container.Resolve<WhereBuilder>();

        public string BuildReadQuery<T>()
        {   
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("SELECT * FROM {0} ", tableDefinition.GetTableName()));
            return sb.ToString();    
        }

        public string BuildReadQuery<T>(Expression<Func<T, bool>> expr)
        {
            var tableDefinition = TableRegistry.Instance.GetTableDefinition<T>();

            StringBuilder sb = new StringBuilder();
            var whereClause = _whereBuilder.ToSql<T>(expr);
            sb.Append(string.Format("SELECT * FROM {0} ", tableDefinition.GetTableName()));
            sb.Append(whereClause);
            return sb.ToString();          
        }

     
    }
}