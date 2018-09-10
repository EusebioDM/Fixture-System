using System;

namespace EirinDuran.Domain
{
    public class EmptyFieldException : Exception
    {
        public override string Message
        {
            get { return "This field can not be empty."; }
        }
    }
}