using System;


namespace NPrismy.CrossCuttingConcerns.Exceptions
{
    public sealed class DatabaseNotConfiguredException : Exception
    {
        internal DatabaseNotConfiguredException(Type databaseType)
            : base(string.Format("Database object is not configured. Please make sure that you've added a constructor to {0} that acceps a DatabaseOptions, and you have passed it to the base class.", databaseType.Name))
        {
        }
    }
}
