using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class UserTryToRecoverDoesNotExistsException : Exception
    {
        public override string Message
        {
            get { return "There is no user with user name entered."; }
        }
    }
}