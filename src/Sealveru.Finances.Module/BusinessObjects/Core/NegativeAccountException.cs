using System;
using System.Runtime.Serialization;

namespace Sealveru.Finances.Module
{ 
    public class NegativeAccountException : Exception
    {
        public NegativeAccountException() { }
        public NegativeAccountException(string message, Exception inner) : base(message, inner) { }
        public NegativeAccountException(string account) : base($"You do not have enough money in the account: {account}") { }
        public NegativeAccountException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
