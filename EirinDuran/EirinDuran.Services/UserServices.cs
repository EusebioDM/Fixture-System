using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IServices;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using EirinDuran.IDataAccess;

namespace EirinDuran.Services
{
    public class UserServices : IUserServices
    {
        private IRepository<User> userRepository;
        private PermissionValidator adminValidator;
        private ILoginServices login;

        public UserServices(IRepository<User> userRepository, ILoginServices loginServices)
        {
            this.userRepository = userRepository;
            this.login = loginServices;
            adminValidator = new PermissionValidator(Role.Administrator, login);
        }

        public void AddUser(User user)
        {
            adminValidator.ValidatePermissions();
            userRepository.Add(user);

        }

        public User GetUser(User user)
        {
            adminValidator.ValidatePermissions();
            return userRepository.Get(user);
        }

        public virtual IEnumerable<User> GetAllUsers()
        {
            adminValidator.ValidatePermissions();
            return userRepository.GetAll();
        }

        public void DeleteUser(string userName)
        {
            adminValidator.ValidatePermissions();
            userRepository.Delete(new User(userName));
        }

        public void Modify(User userToModify)
        {
            adminValidator.ValidatePermissions();
            userRepository.Update(userToModify);
        }

        public void AddFollowedTeam(Team team)
        {
            login.LoggedUser.AddFollowedTeam(team);
            userRepository.Update(login.LoggedUser);
        }

        public IEnumerable<Team> GetAllFollowedTeams()
        {
            User recovered = userRepository.Get(login.LoggedUser);
            return recovered.FollowedTeams;
        }
    }
}
