using System;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IDataAccess;

namespace EirinDuran.Services
{
    public class UserServices
    {
        private UserRepository userRepository;
        private LoginServices login;

        public UserServices(UserRepository userRepository, LoginServices loginServices)
        {
            this.userRepository = userRepository;
            this.login = loginServices;
        }

        public void AddUser(User user)
        {
            RoleValidator(Role.Administrator);
            userRepository.Add(user);
        }

        public void DeleteUser(string userName)
        {
            RoleValidator(Role.Administrator);
            userRepository.Delete(new User(userName));
        }

        public void Modify(User userToModify)
        {
            RoleValidator(Role.Administrator);
            userRepository.Update(userToModify);
        }

        private void RoleValidator(Role required)
        {
            if(login.LoggedUser.Role != required)
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }
    }
}
