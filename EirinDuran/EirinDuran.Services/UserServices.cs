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
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;

namespace EirinDuran.Services
{
    public class UserServices : IUserServices
    {
        private PermissionValidator adminValidator;
        private IRepository<User> userRepository;
        private IRepository<Team> teamRepository;
        private UserMapper userMapper;
        private TeamMapper teamMapper;

        public UserServices(IRepository<User> userRepository, IRepository<Team> teamRepository)
        {
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
            adminValidator = new PermissionValidator(Role.Administrator);
            userMapper = new UserMapper(teamRepository);
            teamMapper = new TeamMapper();
        }

        public ILoginServices Login { get; set; }

        public void CreateUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions(Login);
            User user = userMapper.Map(userDTO);
            userRepository.Add(user);
        }

        public void DeleteUser(string userName)
        {
            adminValidator.ValidatePermissions(Login);
            userRepository.Delete(userName);
        }

        public void Modify(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions(Login);
            User user = userMapper.Map(userDTO);
            userRepository.Update(user);
        }

        public void AddFollowedTeam(TeamDTO teamDTO)
        {
            Team team = teamMapper.Map(teamDTO);
            Login.LoggedUser.AddFollowedTeam(team);
            userRepository.Update(Login.LoggedUser);
        }

        public IEnumerable<TeamDTO> GetAllFollowedTeams()
        {
            User recovered = userRepository.Get(Login.LoggedUser.UserName);
            Func<Team, TeamDTO> mapDTOs = team => teamMapper.Map(team);
            return recovered.FollowedTeams.Select(mapDTOs);
        }
    }
}
