using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Services
{
    public class TeamServices : ITeamServices
    {
        private readonly ILoginServices loginServices;
        private readonly IRepository<Team> teamRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly IRepository<User> userRepository;
        private readonly PermissionValidator validator;
        private TeamMapper teamMapper;

        public TeamServices(ILoginServices loginServices, IRepository<Team> teamRepository, IRepository<Sport> sportRepository, IRepository<User> userRepository)
        {
            this.loginServices = loginServices;
            this.sportRepository = sportRepository;
            this.teamRepository = teamRepository;
            this.userRepository = userRepository;
            validator = new PermissionValidator(Role.Administrator, loginServices);
            teamMapper = new TeamMapper(sportRepository);
        }

        public TeamDTO CreateTeam(TeamDTO team)
        {
            validator.ValidatePermissions();
            try
            {
                Team domainTeam = teamMapper.Map(team);
                teamRepository.Add(domainTeam);
                return teamMapper.Map(domainTeam);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to add team with name {team.Name}.", e);
            }
        }

        public TeamDTO GetTeam(string teamId)
        {
            try
            {
                return teamMapper.Map(teamRepository.Get(teamId));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to recover team. with id {teamId}", e);
            }
        }

        public IEnumerable<TeamDTO> GetAllTeams()
        {
            try
            {
                return teamRepository.GetAll().Select(t => teamMapper.Map(t));
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to get all teams.", e);
            }
        }

        public void UpdateTeam(TeamDTO teamToUpdate)
        {
            try
            {
                Team team = teamMapper.Map(teamToUpdate);
                teamRepository.Update(team);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to update team with name {teamToUpdate.Name}.", e);
            }
        }

        public void DeleteTeam(string teamId)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Delete(teamId);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to delete team with id {teamId}.", e);
            }
        }

        public void AddFollowedTeam(string teamId)
        {
            Team team = teamRepository.Get(teamId);
            User user = userRepository.Get(loginServices.LoggedUser.UserName);
            user.AddFollowedTeam(team);
            try
            {
                userRepository.Update(user);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to add followed sport to user.", e);
            }
        }
    }
}