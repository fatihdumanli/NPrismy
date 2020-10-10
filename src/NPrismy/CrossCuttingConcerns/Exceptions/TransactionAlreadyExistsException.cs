using System;

namespace NPrismy.Exceptions
{
    public class TransactionAlreadyExistsException : Exception
    {
        internal TransactionAlreadyExistsException() : 
            base("Can not begin a transaction, because already exist one.")
        {
            
        }
    }
}