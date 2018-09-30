using System;

namespace EirinDuran.IServices.Exceptions
{
    public class InvalidaDataException : Exception
    {
        private string message;

        public InvalidaDataException(object source, Exception innerException = null) : base("", innerException)
        {
            message = $"Object {source} had invalid data";
            
        }

        public override string Message => message;
    }
}
