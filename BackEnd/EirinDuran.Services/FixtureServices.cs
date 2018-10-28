using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services.DTO_Mappers;

namespace EirinDuran.Services
{
    public class FixtureServices : IFixtureServices
    {
        private readonly PermissionValidator adminValidator;
        private readonly EncounterMapper mapper;
        private readonly IRepository<Encounter> encounterRepository;
        private readonly IRepository<Team> teamRepo;
        private readonly IRepository<Sport> sportRepo;

        public FixtureServices(ILoginServices loginServices,IRepository<Encounter> encounterRepository, IRepository<Sport> sportRepo,IRepository<Team> teamRepo)
        {
            this.adminValidator = new PermissionValidator(Role.Administrator, loginServices);
            mapper = new EncounterMapper(sportRepo,teamRepo);
            this.encounterRepository = encounterRepository;
            this.teamRepo = teamRepo;
            this.sportRepo = sportRepo;
        }

        public IEnumerable<EncounterDTO> CreateFixture(string fixtureGeneratorName, string sportName, DateTime startDate)
        {
            adminValidator.ValidatePermissions();
            Domain.Fixture.IFixtureGenerator generatorServices = GetFixtureGenerator(fixtureGeneratorName, sportName);
            IEnumerable<Team> teamsInSport = teamRepo.GetAll().Where(t => t.Sport.Name == sportName);
            ICollection<Encounter> encounters = generatorServices.GenerateFixture(teamsInSport, startDate);
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
        
        private Domain.Fixture.IFixtureGenerator GetFixtureGenerator(string fixtureGeneratorName, string sportName)
        {
            Func<Domain.Fixture.IFixtureGenerator, bool> isNeededGenerator = g => g.GetType().FullName.EndsWith(fixtureGeneratorName);
            AssemblyLoader.AssemblyLoader loader = GetAssemblyLoader();
            IEnumerable<Domain.Fixture.IFixtureGenerator> generators = loader.GetImplementations<Domain.Fixture.IFixtureGenerator>().Where(isNeededGenerator);
            if (generators.IsNullOrEmpty())
            {
                throw new ServicesException($"{fixtureGeneratorName} was not a valid fixture generator");
            }

            return generators.First();
        }

        public IEnumerable<string> GetAvailableFixtureGenerators()
        {
            Func<Domain.Fixture.IFixtureGenerator, string> getGeneratorTypeName = t => t.GetType().FullName.Split('.').Last();
            AssemblyLoader.AssemblyLoader loader = GetAssemblyLoader();
            return loader.GetImplementations<Domain.Fixture.IFixtureGenerator>().Select(getGeneratorTypeName);
        }

        private AssemblyLoader.AssemblyLoader GetAssemblyLoader()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Directory.EnumerateDirectories(currentDir).First(d => d.EndsWith("FixtureGenerators"));
            return new AssemblyLoader.AssemblyLoader(path);
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