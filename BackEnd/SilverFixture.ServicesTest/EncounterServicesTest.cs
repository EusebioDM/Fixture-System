using SilverFixture.DataAccess;
using SilverFixture.DataAccessTest;
using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.IServices.DTOs;
using SilverFixture.IServices.Exceptions;
using SilverFixture.Services;
using SilverFixture.Services.DTO_Mappers;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using SilverFixture.FixtureGenerators.AllOnce;
using SilverFixture.FixtureGenerators.RoundRobin;
using SilverFixture.IServices.Infrastructure_Interfaces;
using SilverFixture.IServices.Services_Interfaces;

namespace SilverFixture.ServicesTest
{
    [TestClass]
    public class EncounterServicesTest
    {
        private LoginServices login;
        private IRepository<User> userRepo;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Comment> commentRepo;
        private ILogger logger;
        private IAssemblyLoader assemblyLoader;
        private ExtendedEncounterRepository encounterRepo;
        private EncounterMapper mapper;
        private TeamMapper teamMapper;

        private Sport futbol;
        private Sport basquetball;
        private Team felix;
        private Team liverpool;
        private Team river;
        private Team cerro;
        private Team torque;
        private Team danubio;
        private Team penhiarol;
        private Team atenas;
        private Team wanderers;
        private Team boca;

        [TestInitialize]
        public void TestInit()
        {
            userRepo = new UserRepository(GetContextFactory());
            sportRepo = new SportRepository(GetContextFactory());
            teamRepo = new TeamRepository(GetContextFactory());
            encounterRepo = new ExtendedEncounterRepository(GetContextFactory());
            commentRepo = new CommentRepository(GetContextFactory());
            mapper = new SilverFixture.Services.DTO_Mappers.EncounterMapper(sportRepo, teamRepo, commentRepo);
            teamMapper = new TeamMapper(sportRepo);
            userRepo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            userRepo.Add(new User(Role.Follower, "martinFowler", "Martin", "Fowler", "user", "fowler@fowler.com"));
            login = new LoginServices(userRepo, teamRepo);
            assemblyLoader = new SilverFixture.AssemblyLoader.AssemblyLoader();
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            futbol = new Sport("Futbol");
            basquetball = new Sport("Basquetball");
            sportRepo.Add(futbol);
            sportRepo.Add(basquetball);
            felix = new Team("Felix", futbol);
            teamRepo.Add(felix);
            liverpool = new Team("Liverpool", futbol);
            teamRepo.Add(liverpool);
            boca = new Team("Boca Juniors", futbol);
            teamRepo.Add(boca);
            river = new Team("River Plate", futbol);
            teamRepo.Add(river);
            cerro = new Team("Cerro", futbol);
            teamRepo.Add(cerro);
            torque = new Team("Torque", futbol);
            teamRepo.Add(torque);
            danubio = new Team("Danubio", futbol);
            teamRepo.Add(danubio);
            penhiarol = new Team("Peñarol", futbol);
            teamRepo.Add(penhiarol);
            atenas = new Team("Atenas", futbol);
            teamRepo.Add(atenas);
            wanderers = new Team("Wanderes", futbol);
            teamRepo.Add(wanderers);
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestMethod]
        public void CreateSimpleEncounter()
        {
            login.CreateSession("sSanchez", "user");

            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { atenas, wanderers };
            DateTime date = new DateTime(3018, 10, 07);

            Encounter expected = new Encounter(futbol, teams, date);
            encounterSimpleServices.CreateEncounter(mapper.Map(expected));

            List<Encounter> recovered = encounterRepo.GetAll().ToList();

            Assert.IsTrue(recovered.Contains(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void CreateFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");

            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { atenas, wanderers };
            DateTime date = new DateTime(3018, 10, 07);

            Encounter encounter = new Encounter(futbol, teams, date);
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter));
        }

        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");

            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);

            IEnumerable<Team> teamsFirstEncounter = new List<Team> { felix, river };
            IEnumerable<Team> teamsSecondEncounter = new List<Team> { felix, penhiarol };

            DateTime date = new DateTime(3018, 10, 07);

            Encounter encounter = new Encounter(futbol, teamsFirstEncounter, date);
            Encounter encounterOverlappingDates = new Encounter(futbol, teamsSecondEncounter, date);
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounterOverlappingDates));
        }

        [TestMethod]
        public void UpdateEncounterOk()
        {
            login.CreateSession("sSanchez", "user");

            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { atenas, wanderers };
            DateTime date = new DateTime(3018, 10, 07);

            Encounter expected = new Encounter(futbol, teams, date);
            encounterRepo.Add(expected);
            
            expected.DateTime = new DateTime(3021, 10, 07);

            encounterSimpleServices.UpdateEncounter(mapper.Map(expected));

            List<Encounter> recovered = encounterRepo.GetAll().ToList();
            Assert.AreEqual(expected.Sport, recovered[0].Sport);
            Assert.AreEqual(expected.DateTime, recovered[0].DateTime);
        }

        [TestMethod]
        public void GetEncounterByIdOk()
        {
            login.CreateSession("sSanchez", "user");

            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { atenas, wanderers };
            DateTime date = new DateTime(3018, 10, 07);

            Encounter expected = new Encounter(futbol, teams, date);
            EncounterDTO expectedDTO = mapper.Map(expected);
            expectedDTO.Id = IntToGuid(2);
            Encounter expectedWithId = mapper.Map(expectedDTO);

            encounterRepo.Add(expectedWithId);

            EncounterDTO recovered = encounterSimpleServices.GetEncounter(IntToGuid(2) + "");
            Assert.AreEqual(expectedWithId.Id, recovered.Id);
        }

        [TestMethod]
        public void CreateAutoGeneratedFixtureWithoutTeams()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("RoundRobin", "Basquetball", new DateTime(3000, 10, 10));

            IEnumerable<Encounter> encounters = encounterRepo.GetAll();
            Assert.AreEqual(0, encounters.Count());
        }

        [TestMethod]
        public void CreateAutoGeneratedFixtureImpairNumberOfTeams()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("RoundRobin", "Futbol", new DateTime(3000, 10, 10));

            IEnumerable<Encounter> expected = new RoundRobin().GenerateFixture(teamRepo.GetAll(), new DateTime(3000, 10, 10));
            IEnumerable<Encounter> actual = encounterRepo.GetAll();
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public void CreateAutoGeneratedFixtureRoundRobin()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("RoundRobin", "Futbol", new DateTime(3000, 10, 10));

            IEnumerable<Encounter> expected = new RoundRobin().GenerateFixture(teamRepo.GetAll(), new DateTime(3000, 10, 10));
            IEnumerable<Encounter> actual = encounterRepo.GetAll();
            Assert.AreEqual(expected.Count(), actual.Count());
        }



        [TestMethod]
        public void CreateAutoGeneratedFixtureAllOnce()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("AllOnce", "Futbol", new DateTime(3000, 10, 10));

            IEnumerable<Encounter> expected = new AllOnce().GenerateFixture(teamRepo.GetAll(), new DateTime(3000, 10, 10));
            IEnumerable<Encounter> actual = encounterRepo.GetAll();
            Assert.AreEqual(expected.Count(), actual.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateAutoGeneratedFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("AllOnce", "Futbol", new DateTime(3000, 10, 10));
            fixtureServices.CreateFixture("AllOnce", "Futbol", new DateTime(3000, 10, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void CreateAutoGeneratedFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);

            fixtureServices.CreateFixture("AllOnce", "Futbol", new DateTime(3000, 10, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateEncounterWithInvalidRelationshipTest()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterDTO encounter = new EncounterDTO()
            {
                SportName = "Football",
                TeamIds =  new List<string>() {"River Plate_Futbol", "Cerro"},
                DateTime = DateTime.UnixEpoch
            };
            encounterSimpleServices.CreateEncounter(encounter);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateEncounterWithInvalidDateTest()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterDTO encounter = new EncounterDTO()
            {
                SportName = "Football",
                TeamIds = new List<string>() {"River Plate_Futbol", ""}
            };
            encounterSimpleServices.CreateEncounter(encounter);
        }

        [TestMethod]
        public void AddCommentToEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterQueryServices encounterQueryServices = new EncounterQueryServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            
            DateTime date = new DateTime(3018, 10, 12);
            Guid guid = Guid.NewGuid();
            encounterSimpleServices.CreateEncounter(new EncounterDTO()
            {
                SportName = "Futbol",
                TeamIds =  new List<string>() {"River Plate_Futbol", "Cerro_Futbol"},
                DateTime = date,
                Id = guid
            });
            encounterSimpleServices.AddComment(guid.ToString(), "Aguante River vieja, no me importa nada");
            CommentDTO comment = encounterQueryServices.GetAllCommentsToOneEncounter(guid.ToString()).Single();
            Assert.AreEqual("sSanchez", comment.UserName);
            Assert.AreEqual("Aguante River vieja, no me importa nada", comment.Message);
        }

        [TestMethod]
        public void GetAllEncounters()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterQueryServices encounterQueryServices = new EncounterQueryServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(3018, 10, 12);
            IFixtureGenerator fixture = new RoundRobin();
            SilverFixture.Services.DTO_Mappers.EncounterMapper mapper = new SilverFixture.Services.DTO_Mappers.EncounterMapper(sportRepo, teamRepo, commentRepo);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);
            encounterSimpleServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));

            IEnumerable<Encounter> result = encounterSimpleServices.GetAllEncounters().Select(e => mapper.Map(e));
            bool areAllPresent = encounters.All(i => result.ToList().Remove(i));
            Assert.IsTrue(areAllPresent);
        }

        [TestMethod]
        public void DeleteEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(3018, 10, 12);

            IFixtureGenerator fixture = new RoundRobin();
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterSimpleServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));
            IEnumerable<EncounterDTO> allEncounters = encounterSimpleServices.GetAllEncounters();

            encounterSimpleServices.DeleteEncounter(allEncounters.First().Id.ToString());

            Assert.IsTrue(encounterSimpleServices.GetAllEncounters().ToList().Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void DeleteEncounterWithoutSufficientPermission()
        {
            login.CreateSession("martinFowler", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            DateTime date = new DateTime(3018, 10, 12);

            IFixtureGenerator fixture = new RoundRobin();
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterSimpleServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));
            IEnumerable<EncounterDTO> allEncounters = encounterSimpleServices.GetAllEncounters();

            encounterSimpleServices.DeleteEncounter(allEncounters.First().Id.ToString());

            Assert.IsTrue(encounterSimpleServices.GetAllEncounters().ToList().Count == 0);
        }

        [TestMethod]
        public void ListEncountersByTeam()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterQueryServices encounterQueryServices = new EncounterQueryServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            Encounter encounter1 = new Encounter(futbol, new List<Team> { felix, river }, new DateTime(3018, 10, 05));
            Encounter encounter2 = new Encounter(futbol, new List<Team> { atenas, wanderers }, new DateTime(3018, 10, 07));
            Encounter encounter3 = new Encounter(futbol, new List<Team> { penhiarol, torque }, new DateTime(3018, 10, 09));
            Encounter encounter4 = new Encounter(futbol, new List<Team> { river, liverpool }, new DateTime(3018, 10, 11));

            encounterSimpleServices.CreateEncounter(mapper.Map(encounter1));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter2));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter3));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter4));

            IEnumerable<EncounterDTO> encountersRiver = encounterQueryServices.GetEncountersByTeam(river.Name + "_" + futbol.Name);
            Assert.IsTrue(encountersRiver.ToList().Count == 2);
        }

        [TestMethod]
        public void ListEncountersBySport()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterQueryServices encounterQueryServices = new EncounterQueryServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            
            Encounter encounter1 = new Encounter(futbol, new List<Team> { felix, river }, new DateTime(3018, 10, 05));
            Encounter encounter2 = new Encounter(futbol, new List<Team> { atenas, wanderers }, new DateTime(3018, 10, 07));
            Encounter encounter3 = new Encounter(futbol, new List<Team> { penhiarol, torque }, new DateTime(3018, 10, 09));
            Encounter encounter4 = new Encounter(futbol, new List<Team> { river, liverpool }, new DateTime(3018, 10, 11));

            encounterSimpleServices.CreateEncounter(mapper.Map(encounter1));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter2));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter3));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter4));

            IEnumerable<EncounterDTO> encountersRiver = encounterQueryServices.GetEncountersBySport(futbol.Name);
            Assert.IsTrue(encountersRiver.ToList().Count == 4);
        }

        [TestMethod]
        public void ListEncountersByDate()
        {
            login.CreateSession("sSanchez", "user");
            EncounterSimpleServices encounterSimpleServices = new EncounterSimpleServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            EncounterQueryServices encounterQueryServices = new EncounterQueryServices(login, encounterRepo, sportRepo, teamRepo, userRepo, commentRepo);
            
            Encounter encounter1 = new Encounter(futbol, new List<Team> { felix, river }, new DateTime(3018, 10, 05));
            Encounter encounter2 = new Encounter(futbol, new List<Team> { atenas, wanderers }, new DateTime(3018, 10, 07));
            Encounter encounter3 = new Encounter(futbol, new List<Team> { penhiarol, torque }, new DateTime(3018, 10, 09));
            Encounter encounter4 = new Encounter(futbol, new List<Team> { river, liverpool }, new DateTime(3018, 10, 11));

            encounterSimpleServices.CreateEncounter(mapper.Map(encounter1));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter2));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter3));
            encounterSimpleServices.CreateEncounter(mapper.Map(encounter4));

            IEnumerable<EncounterDTO> encountersRiver = encounterQueryServices.GetEncountersByDate(new DateTime(3018, 10, 06), new DateTime(3018, 10, 09));
            Assert.IsTrue(encountersRiver.ToList().Count == 2);
        }

        [TestMethod]
        public void GetAvailableFixtureGenerators()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);

            IEnumerable<string> actual = fixtureServices.GetAvailableFixtureGenerators();
            Assert.IsTrue(actual.Contains("AllOnce"));
            Assert.IsTrue(actual.Contains("RoundRobin"));
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateFixtureWithInvalidGeneratorName()
        {
            login.CreateSession("sSanchez", "user");
            FixtureServices fixtureServices = new FixtureServices(login, encounterRepo, sportRepo, teamRepo, assemblyLoader, commentRepo);
            fixtureServices.CreateFixture("A non existant fixture generator", "Futbol", new DateTime(3000, 10, 10));
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }

        public Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
