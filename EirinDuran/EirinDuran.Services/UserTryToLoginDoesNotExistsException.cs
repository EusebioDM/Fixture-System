using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class UserTryToLoginDoesNotExistsException : Exception
    {
        public override string Message
        {
            get { return "There is no user with the combination username and password entered."; }
        }
    }
}