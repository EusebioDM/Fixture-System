
using System;
using System.Runtime.Serialization;

namespace EirinDuran.Services
{
    public class TeamTryToDeleteDoesNotExistsException : Exception
    {
        public override string Message
        {
            get { return "There is no team with team name entered."; }
        }
    }
}