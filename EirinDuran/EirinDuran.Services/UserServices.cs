using System;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;

namespace EirinDuran.Services
{
    public class UserServices
    {
        private UserRepository userRepository;
        private PermissionValidator adminValidator;
        private ILoginServices login;

        public UserServices(UserRepository userRepository, ILoginServices loginServices)
        {
            this.userRepository = userRepository;
            this.login = loginServices;
            adminValidator = new PermissionValidator(Role.Administrator);
        }

        public void AddUser(User user)
        {
            adminValidator.ValidatePermissions(login.LoggedUser);
            userRepository.Add(user);

        }

        public void DeleteUser(string userName)
        {
            adminValidator.ValidatePermissions(login.LoggedUser);
            userRepository.Delete(new User(userName));
        }

        public void Modify(User userToModify)
        {
            adminValidator.ValidatePermissions(login.LoggedUser);
            userRepository.Update(userToModify);
        }
    }
}
