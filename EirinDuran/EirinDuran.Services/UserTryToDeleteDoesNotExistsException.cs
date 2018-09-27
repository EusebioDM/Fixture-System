using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class UserTryToDeleteDoesNotExistsException : Exception
    {
        public override string Message
        {
            get { return "There is no user with user name entered."; }
        }
    }
}