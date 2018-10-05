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
        private readonly IRepository<Encounter> encounterRepository;
        private readonly IRepository<Sport> sportRepository;
        private readonly PermissionValidator validator;
        private TeamMapper teamMapper;
        private EncounterMapper encounterMapper;

        public TeamServices(ILoginServices loginServices, IRepository<Team> teamRepository, IRepository<Encounter> encounterRepository, IRepository<Sport> sportRepository)
        {
            this.loginServices = loginServices;
            this.sportRepository = sportRepository;
            this.teamRepository = teamRepository;
            this.encounterRepository = encounterRepository;
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginServices);
            encounterMapper = new EncounterMapper(sportRepository, teamRepository);
            teamMapper = new TeamMapper(sportRepository);
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

        public IEnumerable<EncounterDTO> GetAllEncounters(Team team)
        {
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            try
            {
                allEncounters = encounterRepository.GetAll();
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all teams of a team.", e);
            }

            IEnumerable<Encounter> encountersWhereTeamIs = allEncounters.Where(e => e.Teams.Contains(team));
            IEnumerable<EncounterDTO> encountersWhereTeamIsDTO = encountersWhereTeamIs.Select(e => encounterMapper.Map(e));
            return encountersWhereTeamIsDTO;
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