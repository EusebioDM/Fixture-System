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
            if (login.LoggedUser.Role == Role.Administrator)
            {
                userRepository.Add(user);
            }
            else
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }

        public void DeleteUser(string userName)
        {
            if (login.LoggedUser.Role == Role.Administrator)
            {
                userRepository.Delete(new User(userName));
            }
            else
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }

        public void Modify(User userToModify)
        {
            if (login.LoggedUser.Role == Role.Administrator)
            {
                userRepository.Update(userToModify);
            }
            else
            {
                throw new InsufficientPermissionToPerformThisActionException();
            }
        }
    }
}
