using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class LoginServices : ILoginServices
    {
        private IRepository<User> userRepository;
        private readonly UserMapper mapper;
        private User loggedUser;


        public LoginServices(IRepository<User> userRepo, IRepository<Team> teamRepo)
        {
            userRepository = userRepo;
            mapper = new UserMapper(teamRepo);
        }

        public void CreateSession(string userName, string password)
        {
            try
            {
                User recovered = userRepository.Get(userName);

                if (recovered.Password == password)
                {
                    loggedUser = recovered;
                }
                else
                {
                    throw new IServices.Exceptions.InvalidaDataException(userName);
                }

            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to login", e);
            }
        }

        public UserDTO LoggedUser {
            get {
                if (loggedUser == null)
                {
                    throw new InsufficientPermissionException();
                }
                else
                {
                    return mapper.Map(loggedUser);
                }
            }
        }
    }
}
