using EirinDuran.Domain.User;

namespace EirinDuran.Services
{
    public class PermissionValidator
    {
        private Role required;

        public PermissionValidator(Role required)
        {
            this.required = required;
        }

        public void ValidatePermissions(User user)
        {
            if (user.Role != required)
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }
    }
}