using System;

namespace EirinDuran.Domain.User 
{

    public class InvalidMailFormatException : Exception 
    {
        public InvalidMailFormatException(string field) : base($"The mail format is incorrect in the field {field}")
        {
        }

    }
}