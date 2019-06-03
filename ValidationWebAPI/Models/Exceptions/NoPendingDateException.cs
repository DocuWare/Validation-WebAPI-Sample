using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models.Exceptions
{
    [Serializable]
    internal class NoPendingDateException : Exception
    {
        public NoPendingDateException()
        {
        }

        public NoPendingDateException(string message) : base(message)
        {
        }

        public NoPendingDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoPendingDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}