using System;
using System.Linq.Expressions;
using System.Text;

namespace NPrismy
{
    internal class SqlCommandBuilder : ISqlCommandBuilder
    {
        private WhereBuilder _whereBuilder;
        public string BuildReadQuery<T>(Expression<Func<T, bool>> expr)
        {
            StringBuilder sb = new StringBuilder();
            var whereClause = _whereBuilder.ToSql<T>(expr);
            sb.Append("SELECT * FROM [] ");
            sb.Append(whereClause);
            return sb.ToString();          
            
        }
    }
}