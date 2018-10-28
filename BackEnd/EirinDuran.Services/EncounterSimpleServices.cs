using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;

namespace EirinDuran.Services
{
    public class EncounterSimpleServices : IEncounterSimpleServices
    {
        private ILoginServices loginServices;
        private IExtendedEncounterRepository encounterRepository;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<User> userRepo;
        private ILogger logger;
        private PermissionValidator adminValidator;
        private EncounterMapper mapper;
        private CommentMapper commentMapper;

        public EncounterSimpleServices(ILoginServices loginServices, IExtendedEncounterRepository encounterRepo, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<User> userRepo)
        {
            this.loginServices = loginServices;
            this.userRepo = userRepo;
            encounterRepository = encounterRepo;
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepo, teamRepo);
            commentMapper = new CommentMapper(userRepo);
        }

        public EncounterDTO CreateEncounter(EncounterDTO encounterDTO)
        {
            adminValidator.ValidatePermissions();
            Encounter encounter = mapper.Map(encounterDTO);
            ValidateNonOverlappingOfDates(encounter);

            try
            {
                encounterRepository.Add(encounter);
                return mapper.Map(encounter);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to create encounter.", e);
            }
        }

        public void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs)
        {
            adminValidator.ValidatePermissions();
            foreach (EncounterDTO encounterDTO in encounterDTOs)
            {
                Encounter encounter = mapper.Map(encounterDTO);
                ValidateNonOverlappingOfDates(encounter);
                try
                {
                    encounterRepository.Add(encounter);
                }
                catch (DataAccessException e)
                {
                    throw new ServicesException("Failure to create encounter.", e);
                }
            }
        }

        private void ValidateNonOverlappingOfDates(Encounter encounter)
        {
            Team firstTeamToAdd = encounter.Teams.ElementAt(0);
            Team secondTeamToAdd = encounter.Teams.ElementAt(1);
            DateTime encounterDateToAdd = encounter.DateTime;

            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            foreach (var aEncounter in allEncounters.ToList())
            {
                Team firstTeamInDataBase = aEncounter.Teams.ElementAt(0);
                Team secondTeamInDataBase = aEncounter.Teams.ElementAt(1);
                DateTime encounterDateInDataBase = aEncounter.DateTime;

                if ((firstTeamInDataBase.Equals(firstTeamToAdd)
                     || firstTeamInDataBase.Equals(secondTeamToAdd)
                     || secondTeamInDataBase.Equals(firstTeamToAdd)
                     || secondTeamInDataBase.Equals(secondTeamToAdd))
                    && (encounterDateInDataBase == encounterDateToAdd))
                {
                    throw new EncounterWithOverlappingDatesException();
                }
            }
        }

        public void AddComment(string encounterId, string comment)
        {
            User user = userRepo.Get(loginServices.LoggedUser.UserName);
            Encounter encounterToComment = TryToGetEncounter(encounterId);
            encounterToComment.AddComment(user, comment);
            encounterRepository.Update(encounterToComment);
        }

        private Encounter TryToGetEncounter(string encounterId)
        {
            try
            {
                return encounterRepository.Get(encounterId);
            }
            catch (DataAccessException ex)
            {
                throw new ServicesException($"Encounter with id {encounterId} not found", ex);
            }
        }

        public IEnumerable<EncounterDTO> GetAllEncounters()
        {
            try
            {
                return encounterRepository.GetAll().Select(e => mapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to get all encounters.", e);
            }
        }

        public EncounterDTO GetEncounter(string encounterId)
        {
            try
            {
                return mapper.Map(encounterRepository.Get(encounterId));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to recover encounter with id = {encounterId}.", e);
            }
        }

        

        public void UpdateEncounter(EncounterDTO encounterModel)
        {
            adminValidator.ValidatePermissions();

            Encounter encounter = mapper.Map(encounterModel);
            try
            {
                encounterRepository.Update(encounter);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to update encounter id = {encounterModel.Id}", e);
            }
        }

        public void DeleteEncounter(string id)
        {
            adminValidator.ValidatePermissions();
            try
            {
                encounterRepository.Delete(id);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to delete encounter with id {id}.", e);
            }
        }
    }
}