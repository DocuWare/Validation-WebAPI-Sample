using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models.Exceptions
{
    [Serializable]
    internal class NoAmountFoundException : Exception
    {
        public NoAmountFoundException()
        {
        }

        public NoAmountFoundException(string message) : base(message)
        {
        }

        public NoAmountFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAmountFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}