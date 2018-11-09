using System;
using System.Collections.Generic;
using System.IO;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.Services.DTO_Mappers;
using System.Linq;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.Services
{
    public class SportServices : ISportServices
    {
        private readonly ILoginServices loginServices;
        private readonly IRepository<Sport> sportRepository;
        private readonly PermissionValidator validator;
        private readonly SportMapper sportMapper;

        public SportServices(ILoginServices loginServices, IRepository<Sport> sportRepository)
        {
            this.loginServices = loginServices;
            this.sportRepository = sportRepository;
            validator = new PermissionValidator(Domain.User.Role.Administrator, loginServices);
            sportMapper = new SportMapper();
        }

        public SportDTO CreateSport(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = sportMapper.Map(sportDTO);

            try
            {
                sportRepository.Add(sport);
                return sportMapper.Map(sport);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to create sport with name {sportDTO.Name}.", e);
            }
        }

        public void ModifySport(SportDTO sportDTO)
        {
            validator.ValidatePermissions();
            Sport sport = sportMapper.Map(sportDTO);

            try
            {
                sportRepository.Update(sport);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to modify sport with name {sportDTO.Name}.", e);
            }

        }

        public SportDTO GetSport(string sportId)
        {
            try
            {
                return sportMapper.Map(sportRepository.Get(sportId));
            }
            catch(DataAccessException e)
            {
                throw new ServicesException($"Failure to recover sport with id {sportId}.", e);
            }
        }

        public IEnumerable<SportDTO> GetAllSports()
        {
            try
            {
                return sportRepository.GetAll().Select(s => sportMapper.Map(s));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to get all sports.", e);
            }
        }

        public void DeleteSport(string sportId)
        {
            validator.ValidatePermissions();
            try
            {
                sportRepository.Delete(sportId);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to delete sport.", e);
            }
        }
    }
}
