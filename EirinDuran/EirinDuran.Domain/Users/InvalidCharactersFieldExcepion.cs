using System;

namespace EirinDuran.Domain.User {

    public class InvalidCharactersFieldExcepion : Exception 
    {

        public override string Message 
        {
            get { return "This field can not contain special characters."; }
        }
    }
}