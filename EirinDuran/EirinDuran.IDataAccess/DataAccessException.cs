using System;

namespace EirinDuran.IDataAccess
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base()
        {
        }

        public DataAccessException(string message, Exception innerException) : base()
        {
        }
    }
}
