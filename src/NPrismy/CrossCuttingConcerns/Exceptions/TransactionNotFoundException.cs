using System;

namespace NPrismy
{
    public class TransactionNotFoundException : Exception
    {
        internal TransactionNotFoundException(string message) : base(message)
        {
            
        }
    }
}