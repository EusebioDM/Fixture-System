using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;

namespace EirinDuran.Services
{
    public class PermissionValidator
    {
        private Role required;

        public PermissionValidator(Role required)
        {
            this.required = required;
        }

        public void ValidatePermissions(ILoginServices login)
        {
            if (login == null || login.LoggedUser == null || login.LoggedUser.Role != required)
            {
                throw new InsufficientPermissionException();
            }
        }
    }
}