using System;
using EirinDuran.Domain.User;
using EirinDuran.DataAccess;
using EirinDuran.IDataAccess;
using EirinDuran.Services;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;
using EirinDuran.IServices;

namespace EirinDuran.Services
{
    public class UserServices : IUserServices
    {
        private PermissionValidator adminValidator;
        private ILoginServices login;
        private IRepository<User> userRepository;
        private IRepository<Team> teamRepository;
        private UserMapper userMapper;
        private TeamMapper teamMapper;

        public UserServices(ILoginServices loginServices, IRepository<User> userRepository, IRepository<Team> teamRepository)
        {
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
            this.login = loginServices;
            adminValidator = new PermissionValidator(Role.Administrator, login);
            userMapper = new UserMapper(teamRepository);
            teamMapper = new TeamMapper();
        }

        public void CreateUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User user = userMapper.Map(userDTO);
            userRepository.Add(user);
        }

        public void DeleteUser(string userName)
        {
            adminValidator.ValidatePermissions();
            userRepository.Delete(userName);
        }

        public void Modify(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User user = userMapper.Map(userDTO);
            userRepository.Update(user);
        }

        public void AddFollowedTeam(TeamDTO teamDTO)
        {
            Team team = teamMapper.Map(teamDTO);
            login.LoggedUser.AddFollowedTeam(team);
            userRepository.Update(login.LoggedUser);
        }

        public IEnumerable<TeamDTO> GetAllFollowedTeams()
        {
            User recovered = userRepository.Get(login.LoggedUser.UserName);
            Func<Team, TeamDTO> mapDTOs = team => teamMapper.Map(team);
            return recovered.FollowedTeams.Select(mapDTOs);
        }
    }
}
