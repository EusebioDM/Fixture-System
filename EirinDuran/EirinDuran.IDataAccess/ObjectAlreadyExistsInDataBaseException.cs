using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EirinDuran.IDataAccess
{
    public class ObjectAlreadyExistsInDataBaseException : Exception
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
