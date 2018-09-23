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
        private UserRepository userRepository;
        private PermissionValidator adminValidator;
        private ILoginServices login;
        private UserMapper userMapper;
        private TeamMapper teamMapper;

        public UserServices(UserRepository userRepository, ILoginServices loginServices)
        {
            this.userRepository = userRepository;
            this.login = loginServices;
            adminValidator = new PermissionValidator(Role.Administrator, login);
            userMapper = new UserMapper();
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
            userRepository.Delete(new User(userName));
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
            User recovered = userRepository.Get(login.LoggedUser);
            Func<Team, TeamDTO> mapDTOs = team => teamMapper.Map(team);
            return recovered.FollowedTeams.Select(mapDTOs);
        }
    }
}
