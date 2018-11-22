using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SilverFixture.Domain;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.Services.DTO_Mappers;

namespace SilverFixture.Services
{
    public class EncounterSimpleServices : IEncounterSimpleServices
    {
        private readonly ILoginServices loginServices;
        private readonly IRepository<Encounter> encounterRepository;
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Comment> commentRepo;
        private readonly PermissionValidator adminValidator;
        private readonly EncounterMapper mapper;

        public EncounterSimpleServices(ILoginServices loginServices, IRepository<Encounter> encounterRepo, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<User> userRepo, IRepository<Comment> commentRepo)
        {
            this.loginServices = loginServices;
            encounterRepository = encounterRepo;
            this.userRepo = userRepo;
            this.commentRepo = commentRepo;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepo, teamRepo, commentRepo);
        }

        public EncounterDTO CreateEncounter(EncounterDTO encounterDTO)
        {
            try
            {
                adminValidator.ValidatePermissions();
                Encounter encounter = mapper.Map(encounterDTO);
                ValidateNonOverlappingOfDates(encounter);
                encounterRepository.Add(encounter);
                return mapper.Map(encounter);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException(e.Message, e);
            }
            catch (DomainException e)
            {
                throw new ServicesException(e.Message, e);
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
                    throw new ServicesException(e.Message, e);
                }
                catch (DomainException e)
                {
                    throw new ServicesException(e.Message, e);
                }
            }
        }

        private void ValidateNonOverlappingOfDates(Encounter encounter)
        {
            Team firstTeamToAdd = encounter.Teams.ElementAt(0);
            Team secondTeamToAdd = encounter.Teams.ElementAt(1);
            DateTime encounterDateToAdd = encounter.DateTime;

            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            foreach (var aEncounter in allEncounters.Where(e => !e.Equals(encounter)).ToList())
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
                throw new ServicesException(e.Message, e);
            }
            catch (DomainException e)
            {
                throw new ServicesException(e.Message, e);
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

            try
            {
                Encounter encounter = mapper.Map(encounterModel);
                ValidateNonOverlappingOfDates(encounter);
                encounterRepository.Update(encounter);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to update encounter id = {encounterModel.Id}", e);
            }
            catch (DomainException e)
            {
                throw new ServicesException(e.Message, e);
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