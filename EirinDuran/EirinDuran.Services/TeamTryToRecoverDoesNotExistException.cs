using System;
namespace EirinDuran.Services
{
    public class TeamTryToRecoverDoesNotExistException : Exception
    {
        public override string Message
        {
            get { return "There is no team with team name entered."; }
        }
    }
}