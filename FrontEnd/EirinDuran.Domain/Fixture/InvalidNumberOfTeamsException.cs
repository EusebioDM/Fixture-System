using System;
using System.Runtime.Serialization;

namespace EirinDuran.Domain.Fixture
{
    public class InvalidNumberOfTeamsException : DomainException
    {
        public InvalidNumberOfTeamsException()
        {
        }

        public InvalidNumberOfTeamsException(string message) : base(message)
        {
        }

        public InvalidNumberOfTeamsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidNumberOfTeamsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}