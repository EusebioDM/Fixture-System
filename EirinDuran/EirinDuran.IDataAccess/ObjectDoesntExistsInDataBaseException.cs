using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public class ObjectDoesntExistsInDataBaseException : DataAccessException
    {
        private object objectThatDoesntExist;

        public ObjectDoesntExistsInDataBaseException(object objectThatDoesntExist)
        {
            this.objectThatDoesntExist = objectThatDoesntExist;
        }

        public ObjectDoesntExistsInDataBaseException(object objectThatDoesntExist, Exception innerException) : base("", innerException)
        {
            this.objectThatDoesntExist = objectThatDoesntExist;
        }

        public override string Message => $"Object {objectThatDoesntExist} does not exists in the DataBase";
    }
}
