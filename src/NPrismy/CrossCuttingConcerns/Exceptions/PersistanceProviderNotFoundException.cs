using System;

namespace NPrismy.Exceptions
{
    public sealed class PersistanceProviderNotFoundException : Exception
    {
        private const string MESSAGE = "Please make sure that you've supplied a valid Persistance Provider to NPrismy (SqlServer, MySql or Oracle Db)";
        public PersistanceProviderNotFoundException()
            : base(MESSAGE)
        {
        }
    }
}