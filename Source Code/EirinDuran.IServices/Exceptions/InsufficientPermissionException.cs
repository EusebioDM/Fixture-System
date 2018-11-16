using System;

namespace EirinDuran.IServices.Exceptions
{
    public class InsufficientPermissionException : Exception
    {
        public override string Message
        {
            get { return "Insufficient permission to perform this action."; }
        }
    }
}