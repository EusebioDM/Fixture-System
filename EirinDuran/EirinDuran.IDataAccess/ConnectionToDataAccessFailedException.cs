using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public class ConnectionToDataAccessFailedException : Exception
    {
        public ConnectionToDataAccessFailedException()
        {
        }

        public ConnectionToDataAccessFailedException(string message) : base(message)
        {
        }

        public ConnectionToDataAccessFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConnectionToDataAccessFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
