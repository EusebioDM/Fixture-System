using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EirinDuran.Services
{
    public class EncounterServices : IEncounterServices
    {
        private ILoginServices loginServices;
        private IExtendedEncounterRepository encounterRepository;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<User> userRepo;
        private PermissionValidator adminValidator;
        private EncounterMapper mapper;
        private CommentMapper commentMapper;
        private const string FixtureGeneratorsAssembly = "EirinDuran.Domain";

        public EncounterServices(ILoginServices loginServices, IExtendedEncounterRepository encounterRepo, IRepository<Sport> sportRepo, IRepository<Team> teamRepo, IRepository<User> userRepo)
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
            catch(DataAccessException ex)
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
            catch(DataAccessException e)
            {
                throw new ServicesException($"Failure to recover encounter with id = {encounterId}.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetEncountersBySport(string sportName)
        {
            try
            {
                return encounterRepository.GetBySport(sportName).Select(e => mapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to recover encounter with {sportName} sport name.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetEncountersByTeam(string teamId)
        {
            try
            {
                return encounterRepository.GetByTeam(teamId).Select(e => mapper.Map(e));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException($"Failure to get all encounters of a team with id {teamId}.", e);
            }
        }

        public IEnumerable<EncounterDTO> GetEncountersByDate(DateTime start, DateTime end)
        {
            IEnumerable<Encounter> encounters = encounterRepository.GetByDate(start, end);
            return encounters.Select(e => mapper.Map(e));
        }

        public IEnumerable<CommentDTO> GetAllCommentsToOneEncounter(string encounterId)
        {
            try
            {
                return encounterRepository.Get(encounterId).Comments.Select(c => commentMapper.Map(c));
            }
            catch (DataAccessException e)
            {
                throw new ServicesException("Failure to recover all commentaries from encounter with id " + encounterId, e);
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
            catch(DataAccessException e)
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

        public IEnumerable<EncounterDTO> GetAllEncountersWithFollowedTeams()
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
            return encountersWithComment.Select(e => mapper.Map(e));
        }

        public IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate)
        {
            adminValidator.ValidatePermissions();
            IFixtureGenerator generator = GetFixtureGenerator(fixtureGeneratorName, sportName);
            IEnumerable<Team> teamsInSport = teamRepo.GetAll().Where(t => t.Sport.Name == sportName);
            ICollection<Encounter> encounters = generator.GenerateFixture(teamsInSport, startDate);
            ValidateFixture(encounters);
            SaveEncounters(encounters);
            return encounters.Select(e => mapper.Map(e));
        }

        private void ValidateFixture(ICollection<Encounter> encounters)
        {
            foreach (var encounter in encounters)
            {
                ValidateNonOverlappingOfDates(encounter);
            }
        }

        private void SaveEncounters(ICollection<Encounter> encounters)
        {
            foreach (var encounter in encounters)
            {
                encounterRepository.Add(encounter);
            }
        }

        private IFixtureGenerator GetFixtureGenerator(string fixtureGeneratorName, string sportName)
        {
            Sport sport = GetSport(sportName);
            Assembly domainAssembly = Assembly.Load(FixtureGeneratorsAssembly);
            return GetFixtureGeneratorFromAssembly(fixtureGeneratorName, sport, domainAssembly);
        }

        private static IFixtureGenerator GetFixtureGeneratorFromAssembly(string fixtureGeneratorName, Sport sport, Assembly domainAssembly)
        {
            Type generatorType = domainAssembly.GetTypes().FirstOrDefault(t => t.FullName.EndsWith(fixtureGeneratorName));
            if (generatorType == null)
            {
                throw new ServicesException($"{fixtureGeneratorName} was not a valid fixture generator");
            }
            return Activator.CreateInstance(generatorType, sport) as IFixtureGenerator;
        }

        public IEnumerable<string> GetAvailableFixtureGenerators()
        {
            Func<Type, bool> typeIsFixtureGenerator = t => typeof(IFixtureGenerator).IsAssignableFrom(t) && !t.IsInterface;
            Func<Type, string> getGeneratorTypeName = t => t.FullName.Split('.').Last();

            Assembly domainAssembly = Assembly.Load(FixtureGeneratorsAssembly);
            IEnumerable<Type> generatorTypes = domainAssembly.GetTypes().Where(typeIsFixtureGenerator);
            
            return generatorTypes.Select(getGeneratorTypeName);
        }

        private Sport GetSport(string sportName)
        {
            try
            {
                return sportRepo.Get(sportName);
            }
            catch (DataAccessException ex)
            {
                throw new ServicesException($"Sport with name {sportName} doesnt exists", ex);
            }
        }
    }
}