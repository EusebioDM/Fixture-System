using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.Services
{
    public class EncounterServices : IEncounterServices
    {
        private ILoginServices loginServices;
        private IRepository<Encounter> encounterRepository;
        private PermissionValidator adminValidator;

        public EncounterServices(IRepository<Encounter> encounterRepository, ILoginServices loginServices)
        {
            this.loginServices = loginServices;
            this.encounterRepository = encounterRepository;
            adminValidator = new PermissionValidator(Role.Administrator, loginServices);
        }

        public void CreateEncounter(Encounter encounter)
        {
            adminValidator.ValidatePermissions();
            ValidateNonOverlappingOfDates(encounter);
            encounterRepository.Add(encounter);
        }

        public void CreateEncounter(IEnumerable<Encounter> encounters)
        {
            adminValidator.ValidatePermissions();
            encounters.ToList().ForEach(e => ValidateNonOverlappingOfDates(e));
            encounters.ToList().ForEach(e => encounterRepository.Add(e));
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

        public void AddComment(Encounter encounterToComment, string comment)
        {
            encounterToComment.AddComment(loginServices.LoggedUser, comment);
            encounterRepository.Update(encounterToComment);
        }

        public IEnumerable<Encounter> GetAllEncounters()
        {
            return encounterRepository.GetAll();
        }

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

        public void DeleteEncounter(Encounter encounter)
        {
            adminValidator.ValidatePermissions();
            encounterRepository.Delete(encounter);
        }

        public IEnumerable<Encounter> GetAllEncountersWithFollowedTeams()
        {
            List<Encounter> encountersWithComment = new List<Encounter>();
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();
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