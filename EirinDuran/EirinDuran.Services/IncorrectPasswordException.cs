using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class IncorrectPasswordException : Exception
    {
        public override string Message
        {
            get { return "The password is incorrect."; }
        }
    }
}