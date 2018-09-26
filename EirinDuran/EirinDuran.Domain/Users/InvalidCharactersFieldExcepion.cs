using System;

namespace EirinDuran.Domain.User {

    public class InvalidCharactersFieldExcepion : Exception 
    {
        public InvalidCharactersFieldExcepion(string field) : base($"The field {field} can not contain special characters")
        {
        }

        public override string Message 
        {
            get { return "This field can not contain special characters."; }
        }
    }
}