using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class InsufficientPermissionToPerformThisActionException : Exception
    {
        public override string Message
        {
            get { return "Insufficient permission to perform this action."; }
        }
    }
}