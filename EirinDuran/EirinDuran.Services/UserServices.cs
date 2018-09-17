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

        }
    }
}
