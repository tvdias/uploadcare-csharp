using System;
using System.Runtime.Serialization;

namespace Uploadcare.Exceptions
{
    [Serializable]
    public class UnknownException : UploadcareException
    {
        public UnknownException()
        {
        }

        public UnknownException(string message) : base(message)
        {
        }

        public UnknownException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}