using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public class ConnectionToDataAccessFailedException : Exception
    {

        public ConnectionToDataAccessFailedException(Exception innerException) : base("Conection to DataBase failed", innerException)
        {
        }

    }
}
