using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models.Exceptions
{
    [Serializable]
    internal class InvalidProjectNumberException : Exception
    {
        public InvalidProjectNumberException()
        {
        }

        public InvalidProjectNumberException(string message) : base(message)
        {
        }

        public InvalidProjectNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidProjectNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}