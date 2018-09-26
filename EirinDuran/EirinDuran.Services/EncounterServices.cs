using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.Services.DTO_Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Services
{
    public class EncounterServices : IEncounterServices
    {
        private LoginServices loginServices;
        private IRepository<Encounter> encounterRepo;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private PermissionValidator adminValidator;
        private EncounterMapper mapper;

        public EncounterServices(LoginServices loginServices, IRepository<Encounter> encounterRepository, IRepository<Sport> sportRepository, IRepository<Team> teamRepository)
        {
            this.loginServices = loginServices;
            this.encounterRepo = encounterRepository;
            this.sportRepo = sportRepository;
            this.teamRepo = teamRepository;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepository, teamRepository);
        }

        public void CreateEncounter(EncounterDTO encounterDTO)
        {
            adminValidator.ValidatePermissions();
            Encounter encounter = mapper.Map(encounterDTO);
            ValidateNonOverlappingOfDates(encounter);
            encounterRepo.Add(encounter);
        }

        public void CreateEncounter(IEnumerable<EncounterDTO> encounterDTOs)
        {
            adminValidator.ValidatePermissions();
            foreach(EncounterDTO encounterDTO in encounterDTOs)
            {
                Encounter encounter = mapper.Map(encounterDTO);
                ValidateNonOverlappingOfDates(encounter);
                encounterRepo.Add(encounter);
            }
        }

        private void ValidateNonOverlappingOfDates(Encounter encounter)
        {

            Team firstTeamToAdd = encounter.Teams.ElementAt(0);
            Team secondTeamToAdd = encounter.Teams.ElementAt(1);
            DateTime encounterDateToAdd = encounter.DateTime;

            IEnumerable<Encounter> allEncounters = encounterRepo.GetAll();
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

        public void AddComment(Encounter encounterToComment, string comment)
        {
            encounterToComment.AddComment(loginServices.LoggedUser, comment);
            encounterRepo.Update(encounterToComment);
        }

        public IEnumerable<Encounter> GetAllEncounters()
        {
            return encounterRepo.GetAll();
        }

        public IEnumerable<Encounter> GetAllEncounters(Team team)
        {
            IEnumerable<Encounter> allEncounters = encounterRepo.GetAll();
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

        public void DeleteEncounter(string id)
        {
            adminValidator.ValidatePermissions();
            encounterRepo.Delete(id);
        }

        public IEnumerable<Encounter> GetAllEncountersWithFollowedTeams()
        {
            List<Encounter> encountersWithComment = new List<Encounter>();
            IEnumerable<Encounter> allEncounters = encounterRepo.GetAll();
            foreach (var encounter in allEncounters)
            {
                bool intersect = encounter.Teams.Intersect(loginServices.LoggedUser.FollowedTeams).Any();
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