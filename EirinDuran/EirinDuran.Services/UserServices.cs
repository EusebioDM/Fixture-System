using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using System;
using EirinDuran.IServices.Exceptions;

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

        public void CreateUser(UserDTO userDTO)
        {
            adminValidator.ValidatePermissions();
            User user = userMapper.Map(userDTO);

            try
            {
                userRepository.Add(user);
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to create user.", e);
            }
            
        }

        public UserDTO GetUser(string userId)
        {
            adminValidator.ValidatePermissions();
            try
            {
                return userMapper.Map(userRepository.Get(userId));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover user.", e);
            }
        }

        public virtual IEnumerable<UserDTO> GetAllUsers()
        {
            adminValidator.ValidatePermissions();
            try
            {
                return userRepository.GetAll().Select(u => userMapper.Map(u));
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all users.", e);
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
                throw new ServicesException("Failure to try to delete user.", e);
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
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to modify user.", e);
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
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover user logged in followed teams.", e);
            }
        }
    }
}
