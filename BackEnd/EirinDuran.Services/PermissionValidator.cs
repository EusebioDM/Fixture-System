using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Services_Interfaces;
using EirinDuran.Services;

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
            if (login.LoggedUser == null || (required == Role.Administrator && !login.LoggedUser.IsAdmin))
            {
                throw new InsufficientPermissionException();
            }
        }
    }
}