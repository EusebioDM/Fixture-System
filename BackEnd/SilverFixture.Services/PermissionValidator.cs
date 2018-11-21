using EirinDuran.Domain.User;
using SilverFixture.IServices;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.Services;

namespace SilverFixture.Services
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