using System;

namespace EirinDuran.Domain
{
    public class EmptyFieldException : DomainException
    {

        public EmptyFieldException(string field) : base($"Field {field} cannot be empty")
        {

        }
    }
}