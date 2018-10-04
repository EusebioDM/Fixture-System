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
        private readonly PermissionValidator validator;
        private TeamMapper teamMapper;

        public TeamServices(ILoginServices loginServices, IRepository<Team> teamRepository, IRepository<Sport> sportRepo)
        {
            this.teamRepository = teamRepository;
            this.loginServices = loginServices;
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginServices);
            teamMapper = new TeamMapper(sportRepo
            );
        }

        public void AddTeam(TeamDTO team)
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

        public TeamDTO GetTeam(string teamName)
        {
            try
            {
                return teamMapper.Map(teamRepository.Get(teamName));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover team.", e);
            }
        }

        public IEnumerable<TeamDTO> GetAll()
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

        public void DeleteTeam(string id)
        {
            validator.ValidatePermissions();
            try
            {
                teamRepository.Delete(id);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to delete team.", e);
            }
        }
    }
}