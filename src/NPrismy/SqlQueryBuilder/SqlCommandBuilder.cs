using System;
using System.Linq.Expressions;

namespace NPrismy
{
    internal class SqlCommandBuilder : ISqlCommandBuilder
    {
        public string BuildReadQuery<T>(Expression<Func<T, bool>> expr)
        {

            throw new NotImplementedException();
        }
    }
}