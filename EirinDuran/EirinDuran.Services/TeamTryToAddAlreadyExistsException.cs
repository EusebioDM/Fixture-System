using System;

namespace EirinDuran.Services
{
    public class TeamTryToAddAlreadyExistsException : Exception
    {
        public override string Message
        {
            get { return "The team trying to add already exists."; }
        }
    }
}