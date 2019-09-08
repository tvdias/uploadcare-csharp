using System;
using System.Runtime.Serialization;

namespace Uploadcare.Exceptions
{
    [Serializable]
    public abstract class UploadcareException : Exception
    {
        protected UploadcareException()
        {
        }

        protected UploadcareException(string message) : base(message)
        {
        }

        protected UploadcareException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UploadcareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}