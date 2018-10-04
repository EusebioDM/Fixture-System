using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Services
{
    public class EncounterServices : IEncounterServices
    {

        private ILoginServices loginServices;
        private IRepository<Encounter> encounterRepository;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<User> userRepo;
        private PermissionValidator adminValidator;
        private EncounterMapper mapper;

        public EncounterServices(ILoginServices loginServices, IRepository<Encounter> encounterRepo, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<User> userRepo)
        {
            this.loginServices = loginServices;
            this.userRepo = userRepo;
            this.encounterRepository = encounterRepo;
            this.sportRepo = sportRepo;
            this.teamRepo = teamRepo;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepo, teamRepo);
        }

        public void CreateEncounter(EncounterDTO encounterDTO)
        {
            adminValidator.ValidatePermissions();
            Encounter encounter = mapper.Map(encounterDTO);
            ValidateNonOverlappingOfDates(encounter);

            try
            {
                encounterRepository.Add(encounter);
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to create encounter.", e);
            }
        }

        public void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs)
        {
            adminValidator.ValidatePermissions();
            foreach(EncounterDTO encounterDTO in encounterDTOs)
            {
                Encounter encounter = mapper.Map(encounterDTO);
                ValidateNonOverlappingOfDates(encounter);
                try
                {
                    encounterRepository.Add(encounter);
                }
                catch (DataAccessException e)
                {
                    throw new ServicesException("Failure to try to create encounter.", e);
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
            Encounter encounterToComment = encounterRepository.Get(encounterId);
            encounterToComment.AddComment(user, comment);
            encounterRepository.Update(encounterToComment);
        }

        public IEnumerable<EncounterDTO> GetAllEncounters()
        {
            try
            {
                return encounterRepository.GetAll().Select(e => mapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to get all encounters.", e);
            }
        }

        /*Este método debería cambiarse para el sport services*/
        public IEnumerable<Encounter> GetAllEncounters(Team team)
        {
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            List<Encounter> encountersWhereTeamIs = new List<Encounter>();

            foreach (var encounter in allEncounters)
            {
                if (encounter.Teams.Contains(team))
                {
                    encountersWhereTeamIs.Add(encounter);
                }
            }

            return encountersWhereTeamIs;
        }

        public IEnumerable<Comment> GetAllCommentsToOneEncounter(string encounterId)
        {
            try
            {
                return encounterRepository.Get(encounterId).Comments;
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to try to recover all commentaries to encounter " + encounterId, e);
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
                throw new ServicesException("Failure to try to delete encounter.", e);
            }
        }

        public IEnumerable<Encounter> GetAllEncountersWithFollowedTeams()
        {
            List<Encounter> encountersWithComment = new List<Encounter>();
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
            foreach (var encounter in allEncounters)
            {
                bool intersect = encounter.Teams.Select(t => t.Name).Intersect(loginServices.LoggedUser.FollowedTeamsNames).Any();
                bool hasComments = (encounter.Comments.Count() > 0);

                if (intersect && hasComments)
                {
                    encountersWithComment.Add(encounter);
                }    
            }
            return encountersWithComment;
        }
    }
}