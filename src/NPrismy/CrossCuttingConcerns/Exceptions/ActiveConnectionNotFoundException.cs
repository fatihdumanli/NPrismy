using System;

namespace NPrismy
{
    public sealed class ActiveConnectionNotFoundException : Exception
    {
        internal ActiveConnectionNotFoundException(string message) : base(message)
        {   
        }
    }
}