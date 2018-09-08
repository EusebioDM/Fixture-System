using System;

namespace EirinDuran.Domain.User 
{

    public class InvalidMailFormatException : Exception 
    {

        public override string Message 
        {
            get { return "The mail format is incorrect."; }
        }
    }
}