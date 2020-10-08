using System;
using System.Linq.Expressions;

namespace NPrismy
{
    internal interface ISqlCommandBuilder
    {
        string BuildReadQuery<T>(Expression<Func<T, bool>> expr);
       
    }
}