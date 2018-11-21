using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Infrastructure_Interfaces;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.Services.DTO_Mappers;
using EncounterPlayerCount = SilverFixture.Domain.Fixture.EncounterPlayerCount;

namespace SilverFixture.Services
{
    public class FixtureServices : IFixtureServices
    {
        private const string FixtureGeneratorsFolderName = "FixtureGenerators";
        private readonly PermissionValidator adminValidator;
        private readonly EncounterMapper mapper;
        private readonly IRepository<Encounter> encounterRepository;
        private readonly IRepository<Team> teamRepo;
        private readonly IAssemblyLoader assemblyLoader;
        private readonly IRepository<Comment> commentRepo;
        private readonly IRepository<Sport> sportRepo;

        public FixtureServices(ILoginServices loginServices,IRepository<Encounter> encounterRepository, IRepository<Sport> sportRepo,IRepository<Team> teamRepo, IAssemblyLoader assemblyLoader, IRepository<Comment> commentRepo)
        {
            this.adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepo,teamRepo, commentRepo);
            this.encounterRepository = encounterRepository;
            this.teamRepo = teamRepo;
            this.assemblyLoader = assemblyLoader;
            this.commentRepo = commentRepo;
            SetupAssemblyLoader();
            this.sportRepo = sportRepo;
        }

        public IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate)
        {
            adminValidator.ValidatePermissions();
            ValidateSportIsTwoPlayerSport(sportName);
            IFixtureGenerator generatorServices = GetFixtureGenerator(fixtureGeneratorName, sportName);
            IEnumerable<Team> teamsInSport = teamRepo.GetAll().Where(t => t.Sport.Name == sportName);
            ICollection<Encounter> encounters = generatorServices.GenerateFixture(teamsInSport, startDate);
            ValidateFixture(encounters);
            SaveEncounters(encounters);
            return encounters.Select(e => mapper.Map(e));
        }

        private void ValidateSportIsTwoPlayerSport(string sportName)
        {
            Sport sport = GetSport(sportName);
            if(sport.EncounterPlayerCount != EncounterPlayerCount.TwoPlayers)
                throw new ServicesException("Fixture generators are only for two player sports");
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
        
        private IFixtureGenerator GetFixtureGenerator(string fixtureGeneratorName, string sportName)
        {
            Func<IFixtureGenerator, bool> isNeededGenerator = g => g.GetType().FullName.EndsWith(fixtureGeneratorName);
            SetupAssemblyLoader();
            IEnumerable<IFixtureGenerator> generators = assemblyLoader.GetImplementations<IFixtureGenerator>().Where(isNeededGenerator);
            if (generators.IsNullOrEmpty())
            {
                throw new ServicesException($"{fixtureGeneratorName} was not a valid fixture generator");
            }

            return generators.First();
        }

        public IEnumerable<string> GetAvailableFixtureGenerators()
        {
            Func<IFixtureGenerator, string> getGeneratorTypeName = t => t.GetType().FullName.Split('.').Last();
            return assemblyLoader.GetImplementations<IFixtureGenerator>().Select(getGeneratorTypeName);
        }

        private void SetupAssemblyLoader()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Directory.EnumerateDirectories(currentDir).First(d => d.EndsWith(FixtureGeneratorsFolderName));
            assemblyLoader.AssembliesPath = path;
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