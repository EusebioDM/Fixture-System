using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IServices.Exceptions
{
    public class InvalidaDataException : Exception
    {
        private string message;

        public InvalidaDataException(object source, string invalidField, Exception innerException = null) : base("", innerException)
        {
            message = $"Object {source} had invalid data in the field {invalidField}";
            
        }

        public override string Message => message;
    }
}
