using EirinDuran.Domain.User;
using EirinDuran.IServices;

namespace EirinDuran.Services
{
    public class PermissionValidator
    {
        private Role required;
        private ILoginServices login;

        public PermissionValidator(Role required, ILoginServices login)
        {
            this.required = required;
            this.login = login;
        }

        public void ValidatePermissions()
        {
            if (login.LoggedUser == null || login.LoggedUser.Role != required)
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }
    }
}