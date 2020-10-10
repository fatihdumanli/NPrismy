using System;
using System.Linq.Expressions;

namespace NPrismy
{
    internal interface ISqlCommandBuilder
    {
        string BuildReadQuery<T>();
        string BuildReadQuery<T>(Expression<Func<T, bool>> expr);
        string BuildInsertQuery<T>(T obj);       
    }
}