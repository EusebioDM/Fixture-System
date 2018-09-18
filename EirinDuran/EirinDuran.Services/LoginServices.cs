using System;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;

namespace EirinDuran.Services
{
    public class LoginServices : ILoginServices
    {
        private UserRepository userRepository;

        public LoginServices(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void CreateSession(string userName, string password)
        {
            try
            {
                User recovered = userRepository.Get(new User(userName));

                if(recovered.Password == password)
                {
                    LoggedUser = recovered;
                }
                else
                {
                    throw new IncorrectPasswordException();
                }
            }
            catch (ObjectDoesntExistsInDataBaseException)
            {
                throw new UserTryToLoginDoesNotExistsException();
            }
        }

        public User LoggedUser { get; private set; }
    }
}
