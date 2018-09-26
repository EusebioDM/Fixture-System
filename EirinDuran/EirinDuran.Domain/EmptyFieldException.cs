using System;

namespace EirinDuran.Domain
{
    public class EmptyFieldException : Exception
    {

        public EmptyFieldException(string field) : base($"Field {field} cannot be empty")
        {

        }
    }
}