using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using System;

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

        public UserDTO GetUser(string id)
        {
            adminValidator.ValidatePermissions();
            try
            {
                return userMapper.Map(userRepository.Get(id));
            }
            catch (DataAccessException)
            {
                throw new FailureToTryToRecoverUserException();
            }
        }

        public virtual IEnumerable<UserDTO> GetAllUsers()
        {
            adminValidator.ValidatePermissions();
            return userRepository.GetAll().Select(u => userMapper.Map(u));
        }

        public void DeleteUser(string id)
        {
            adminValidator.ValidatePermissions();
            try
            {
                userRepository.Delete(id);
            }
            catch (DataAccessException)
            {
                throw new FailureToTryToDeleteUserException();
            }
            
        }

        public void ModifyUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User user = userMapper.Map(userDTO);
            try
            {
                userRepository.Update(user);
            }
            catch (DataAccessException)
            {
                throw new FailureToTryToModifyUserException();
            }
        }

        public void AddFollowedTeam(string id)
        {
            Team team = teamRepository.Get(id);
            User user = userRepository.Get(login.LoggedUser.UserName);
            user.AddFollowedTeam(team);
            try
            {
                userRepository.Update(user);
            }
            catch(DataAccessException)
            {
                throw new FailureToTryToModifyUserException();
            }
        }

        public IEnumerable<TeamDTO> GetAllFollowedTeams()
        {
            try
            {
                User recovered = userRepository.Get(login.LoggedUser.UserName);
                Func<Team, TeamDTO> mapDTOs = team => teamMapper.Map(team);
                return recovered.FollowedTeams.Select(mapDTOs);
            }
            catch(DataAccessException)
            {
                throw new FailureToTryToRecoverUserLoggedInFollowedTeamsException();
            }
        }
    }
}
