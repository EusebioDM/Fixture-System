using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Services;
using EirinDuran.IServices;
using EirinDuran.IServices.Interfaces;
using EirinDuran.IServices.DTOs;
using EirinDuran.Services.DTO_Mappers;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.Services
{
    public class LoginServices : ILoginServices
    {
        private IRepository<User> userRepository;
        private UserMapper mapper;
        private User loggedUser;
        

        public LoginServices(IRepository<User> userRepo, IRepository<Team> teamRepo)
        {
            this.userRepository = userRepo;
            mapper = new UserMapper(teamRepo);
        }

        public void CreateSession(string userName, string password)
        {
            try
            {
                User recovered = userRepository.Get(userName);

                if(recovered.Password == password)
                {
                    loggedUser = recovered;
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

        public UserDTO LoggedUser => mapper.Map(loggedUser);
    }
}
