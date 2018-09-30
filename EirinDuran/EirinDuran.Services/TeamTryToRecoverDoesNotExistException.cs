using EirinDuran.IServices.Exceptions;

namespace EirinDuran.Services
{
    public class TeamTryToRecoverDoesNotExistException : UserServicesException
    {
        public override string Message
        {
            get { return "There is no team with team name entered."; }
        }
    }
}