using System;
using System.Runtime.Serialization;

namespace EirinDuran.Domain.Fixture
{
    public class InvalidTeamException : Exception
    {
        public InvalidTeamException()
        {
        }

        public InvalidTeamException(string message) : base(message)
        {
        }

        public InvalidTeamException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTeamException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}