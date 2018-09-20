using System;
using System.Collections.Generic;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public class ObjectDoesntExistsInDataBaseException : Exception
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

        public override string Message => $"Object {objectThatDoesntExist} doesnt exists in the DataBase";
    }
}
