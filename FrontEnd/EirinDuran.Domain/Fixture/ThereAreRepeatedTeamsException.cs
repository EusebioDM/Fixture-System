using System;
using System.Runtime.Serialization;

namespace EirinDuran.Domain.Fixture
{
    public class ThereAreRepeatedTeamsException : DomainException
    {
        public ThereAreRepeatedTeamsException()
        {
        }

        public ThereAreRepeatedTeamsException(string message) : base(message)
        {
        }

        public ThereAreRepeatedTeamsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThereAreRepeatedTeamsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}