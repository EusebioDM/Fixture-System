using System.Collections.Generic;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;
using System;

namespace EirinDuran.Services
{
    public class SportServices : ISportServices
    {
        private readonly ILoginServices loginService;
        private readonly IRepository<Sport> sportRepo;
        private readonly IRepository<Team> teamRepo;
        private readonly PermissionValidator validator;
        private readonly SportMapper mapper;

        public SportServices(ILoginServices loginService, IRepository<Sport> sportRepo, IRepository<Team> teamRepo)
        {
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginService);
            this.loginService = loginService;
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            mapper = new SportMapper(teamRepo);
        }

        public void Create(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = mapper.Map(sportDTO);
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
            Sport sport = mapper.Map(sportDTO);

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
                return sportRepo.GetAll().Select(s => mapper.Map(s));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all sports.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetAllEncountersWithSpecificSport(string sportName)
        {
            throw new NotImplementedException();
        }

        public void DeleteSport(string id)
        {
            validator.ValidatePermissions();
            try
            {
                sportRepo.Delete(id);
            }
            catch(DataAccessException e)
            {
                throw new ServicesException("Failure to try to delete sport.", e);
            }
        }
    }
}
