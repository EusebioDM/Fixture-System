using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.Services.DTO_Mappers;

namespace SilverFixture.Services
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
                    throw new InvalidaDataException(userName);
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
