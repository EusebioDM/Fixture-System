using System.Collections.Generic;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;

namespace EirinDuran.Services
{
    public class SportServices : ISportServices
    {
        private readonly ILoginServices loginService;
        private readonly IRepository<Sport> sportRepo;
        private readonly IRepository<Team> teamRepo;
        private readonly IRepository<Encounter> encounterRepo;
        private readonly PermissionValidator validator;
        private readonly SportMapper sportMapper;
        private readonly EncounterMapper encounterMapper;

        public SportServices(ILoginServices loginService, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<Encounter> encounterRepo)
        {
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginService);
            this.loginService = loginService;
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            this.encounterRepo = encounterRepo;
            encounterMapper = new EncounterMapper(sportRepo, teamRepo);
            sportMapper = new SportMapper();
        }

        public void Create(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = sportMapper.Map(sportDTO);
            try
            {
                sportRepo.Add(sport);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to create sport.", e);
            }
        }

        public void Modify(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = sportMapper.Map(sportDTO);

            try
            {
                sportRepo.Update(sport);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to modify sport.", e);
            }

        }

        public IEnumerable<SportDTO> GetAllSports()
        {
            try
            {
                return sportRepo.GetAll().Select(s => sportMapper.Map(s));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all sports.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetAllEncountersOfASpecificSport(string sportName)
        {
            IEnumerable<Encounter> allEncounters;
            try
            {
                allEncounters = encounterRepo.GetAll();
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover encounter with specific sport.", e);
            }

            IEnumerable<Encounter> filteredEncounters = allEncounters.Where(e => e.Sport.Name.Equals(sportName));
            IEnumerable<EncounterDTO> filteredEncountersDTO = filteredEncounters.Select(e => encounterMapper.Map(e));

            return filteredEncountersDTO;
        }

        public void DeleteSport(string id)
        {
            validator.ValidatePermissions();
            try
            {
                sportRepo.Delete(id);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to delete sport.", e);
            }
        }
    }
}
