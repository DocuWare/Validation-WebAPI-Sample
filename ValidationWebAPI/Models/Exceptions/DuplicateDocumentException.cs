using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models
{
    [Serializable]
    internal class DuplicateDocumentException : Exception
    {
        public DuplicateDocumentException()
        {
        }

        public DuplicateDocumentException(string message) : base(message)
        {
        }

        public DuplicateDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateDocumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}