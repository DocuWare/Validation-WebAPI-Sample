using System;
using System.Runtime.Serialization;

namespace ValidationWebAPI.Models.Exceptions
{
    [Serializable]
    internal class ProjectNumberNotSpecifiedException : Exception
    {
        public ProjectNumberNotSpecifiedException()
        {
        }

        public ProjectNumberNotSpecifiedException(string message) : base(message)
        {
        }

        public ProjectNumberNotSpecifiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectNumberNotSpecifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}