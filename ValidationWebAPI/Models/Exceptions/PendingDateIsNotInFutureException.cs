using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models.Exceptions
{
    [Serializable]
    internal class PendingDateIsNotInFutureException : Exception
    {
        public PendingDateIsNotInFutureException()
        {
        }

        public PendingDateIsNotInFutureException(string message) : base(message)
        {
        }

        public PendingDateIsNotInFutureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PendingDateIsNotInFutureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}