using System;

namespace NPrismy
{
    public class TransactionNotFoundException : Exception
    {
        internal TransactionNotFoundException() : base("The transaction which is intended to commit was null.")
        {
            
        }
    }
}