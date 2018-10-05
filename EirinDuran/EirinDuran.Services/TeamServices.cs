using EirinDuran.Domain.Fixture;
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
        private readonly PermissionValidator validator;
        private TeamMapper teamMapper;

        public TeamServices(ILoginServices loginServices, IRepository<Team> teamRepository, IRepository<Sport> sportRepository)
        {
            this.loginServices = loginServices;
            this.sportRepository = sportRepository;
            this.teamRepository = teamRepository;
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginServices);
            teamMapper = new TeamMapper(sportRepository);
        }

        public void CreateTeam(TeamDTO team)
        {
            validator.ValidatePermissions();
            try
            {
                Team domianTeam = teamMapper.Map(team);
                teamRepository.Add(domianTeam);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to add team.", e);
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
                throw new ServicesException("Failure to try to recover team.", e);
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
                throw new ServicesException("Failure to try to get all teams.", e);
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
                throw new ServicesException("Failure to try to delete team.", e);
            }
        }
    }
}