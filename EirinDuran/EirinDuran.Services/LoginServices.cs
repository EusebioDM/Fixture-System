using System;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IDataAccess;
using EirinDuran.Services;
using EirinDuran.IServices;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.Services
{
    public class LoginServices : ILoginServices
    {
        private IRepository<User> userRepository;

        public LoginServices(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void CreateSession(string userName, string password)
        {
            try
            {
                User recovered = userRepository.Get(userName);

                if(recovered.Password == password)
                {
                    LoggedUser = recovered;
                }
                else
                {
                    throw new IServices.Exceptions.InvalidaDataException(userName);
                }
            }
            catch (ObjectDoesntExistsInDataBaseException)
            {
                throw new IServices.Exceptions.InvalidaDataException(userName);
            }
        }

        public User LoggedUser { get; private set; }
    }
}
