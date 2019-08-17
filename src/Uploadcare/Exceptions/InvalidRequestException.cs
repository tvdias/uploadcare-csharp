using System;
using System.Runtime.Serialization;

namespace Uploadcare.Exceptions
{
    [Serializable]
    internal class InvalidRequestException : UploadcareException
    {
        public InvalidRequestException()
        {
        }

        public InvalidRequestException(string message) : base(message)
        {
        }

        public InvalidRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}