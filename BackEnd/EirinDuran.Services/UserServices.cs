using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.Services.DTO_Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.Services
{
    public class UserServices : IUserServices
    {
        private PermissionValidator adminValidator;
        private ILoginServices loginServices;
        private IRepository<User> userRepository;
        private IRepository<Team> teamRepository;
        private UserMapper userMapper;
        private TeamMapper teamMapper;

        public UserServices(ILoginServices loginServices, IRepository<User> userRepository, IRepository<Team> teamRepository, IRepository<Sport> sportRepository)
        {
            this.userRepository = userRepository;
            this.teamRepository = teamRepository;
            this.loginServices = loginServices;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            userMapper = new UserMapper(teamRepository);
            teamMapper = new TeamMapper(sportRepository);
        }

        public UserDTO CreateUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User user = userMapper.Map(userDTO);

            try
            {
                userRepository.Add(user);
                return userMapper.Map(user);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"User with name {user.Name} already exists.", e);
            }

        }

        public UserDTO GetUser(string userId)
        {
            try
            {
                return userMapper.Map(userRepository.Get(userId));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"User with name {userId} doesnt exists.", e);
            }
        }

        public virtual IEnumerable<UserDTO> GetAllUsers()
        {
            try
            {
                return userRepository.GetAll().Select(u => userMapper.Map(u));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to get all users.", e);
            }
        }

        public void DeleteUser(string id)
        {
            adminValidator.ValidatePermissions();
            try
            {
                userRepository.Delete(id);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"User with name {id} doesnt exists.", e);
            }

        }

        public void ModifyUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User fromDB = userRepository.Get(userDTO.UserName);
            userDTO.Password = fromDB.Password;
            userDTO.FollowedTeamsNames = fromDB.FollowedTeams.Select(t => t.Name + "_" + t.Sport).ToList();
            User user = userMapper.Map(userDTO);
            try
            {
                userRepository.Update(user);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"User with name {user.Name} doesnt exists.", e);
            }
        }

        public IEnumerable<TeamDTO> GetFollowedTeams()
        {
            try
            {
                User recovered = userRepository.Get(loginServices.LoggedUser.UserName);
                Func<Team, TeamDTO> mapDTOs = team => teamMapper.Map(team);
                return recovered.FollowedTeams.Select(mapDTOs);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failed to get {loginServices.LoggedUser} followed teams.", e);
            }
        }
    }
}
