using System;

namespace NPrismy.Exceptions
{
    public sealed class CommandExecutionException : Exception
    {
        public CommandExecutionException(string query, Exception innerException) : 
            base(string.Format("An error has occured when executing command. ({0})", query), innerException)
        {
        }
    }
}