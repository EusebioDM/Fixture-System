using EirinDuran.IServices.Exceptions;

namespace EirinDuran.Services
{
    public class TeamTryToAddAlreadyExistsException : UserServicesException
    {
        public override string Message
        {
            get { return "The team trying to add already exists."; }
        }
    }
}