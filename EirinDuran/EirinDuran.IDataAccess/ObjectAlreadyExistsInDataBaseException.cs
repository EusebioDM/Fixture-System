using System;

namespace EirinDuran.IDataAccess
{
    public class ObjectAlreadyExistsInDataBaseException : DataAccessException
    {
        private object objectThatAlreadyExists;
        public ObjectAlreadyExistsInDataBaseException(object objectThatAlreadyExists)
        {
            this.objectThatAlreadyExists = objectThatAlreadyExists;
        }

        public ObjectAlreadyExistsInDataBaseException(object objectThatAlreadyExists, Exception innerExcepion) : base("", innerExcepion)
        {
            this.objectThatAlreadyExists = objectThatAlreadyExists;
        }

        public override string Message => $"Object {objectThatAlreadyExists} already exists in the DataBase";


    }
}
