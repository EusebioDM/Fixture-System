using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class EncounterServicesTest
    {
        private LoginServices login;
        private UserRepository userRepository;
        private EncounterRepository encounterRepository;

        private Team felix;
        private Team liverpool;
        private Team river;
        private Team cerro;
        private Team torque;
        private Team danubio;
        private Team penhiarol;
        private Team atenas;
        private Team wanderers;

        [TestInitialize]
        public void TestInit()
        {
            userRepository = new UserRepository(GetContextFactory());
            encounterRepository = new EncounterRepository(GetContextFactory());
            userRepository.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            userRepository.Add(new User(Role.Follower, "martinFowler", "Mart�n", "Fowler", "user", "fowler@fowler.com"));
            login = new LoginServices(userRepository);
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            felix = new Team("Felix");
            liverpool = new Team("Liverpool");
            river = new Team("River Plate");
            cerro = new Team("Cerro");
            torque = new Team("Torque");
            danubio = new Team("Danubio");
            penhiarol = new Team("Pe�arol");
            atenas = new Team("Atenas");
            wanderers = new Team("Wanderes");
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging().UseLazyLoadingProxies().Options;
            return new InMemoryContextFactory(options);
        }

        [TestMethod]
        public void CreateSimpleEncounter()
        {
            login.CreateSession("sSanchez", "user");

            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(2018, 10, 07);

            Sport basketball = new Sport("Basketball");

            basketball.AddTeam(river);
            basketball.AddTeam(felix);

            Encounter expected = new Encounter(basketball, teams, date);
            encounterServices.CreateEncounter(expected);

            List<Encounter> recovered = (List<Encounter>)encounterRepository.GetAll();

            bool result = recovered.Contains(expected);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void CreateFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");

            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(2018, 10, 07);

            Sport basketball = new Sport("Basketball");

            basketball.AddTeam(river);
            basketball.AddTeam(felix);

            Encounter encounter = new Encounter(basketball, teams, date);
            encounterServices.CreateEncounter(encounter);
        }

        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");

            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teamsFirstEncounter = new List<Team> { felix, river };
            IEnumerable<Team> teamsSecondEncounter = new List<Team> { felix, penhiarol };

            DateTime date = new DateTime(2018, 10, 07);

            Sport football = new Sport("Football");

            football.AddTeam(river);
            football.AddTeam(felix);
            football.AddTeam(penhiarol);

            Encounter encounter = new Encounter(football, teamsFirstEncounter, date);
            Encounter encounterOverlappingDates = new Encounter(football, teamsSecondEncounter, date);
            encounterServices.CreateEncounter(encounter);
            encounterServices.CreateEncounter(encounterOverlappingDates);
        }

        [TestMethod]
        public void CreateAutoGeneratedFixture()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river, penhiarol, atenas, cerro, torque, danubio, wanderers };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            football.AddTeam(penhiarol);
            football.AddTeam(atenas);
            football.AddTeam(cerro);
            football.AddTeam(torque);
            football.AddTeam(danubio);
            football.AddTeam(wanderers);

            DateTime date = new DateTime(2018, 10, 10);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters);

            List<Encounter> recovered = (List<Encounter>)encounterRepository.GetAll();

            bool areAllPresent = recovered.All(i => encounters.ToList().Remove(i));

            Assert.IsTrue(areAllPresent);
        }

        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateAutoGeneratedFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river, penhiarol, atenas };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            football.AddTeam(penhiarol);
            football.AddTeam(atenas);

            DateTime dateFirstEncounters = new DateTime(2018, 10, 10);
            DateTime dateSecondEncounters = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);

            IEnumerable<Encounter> firstEncounters = fixture.GenerateFixture(teams, dateFirstEncounters);
            IEnumerable<Encounter> secondEncounters = fixture.GenerateFixture(teams, dateSecondEncounters);

            encounterServices.CreateEncounter(firstEncounters);
            encounterServices.CreateEncounter(secondEncounters);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void CreateAutoGeneratedFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);

            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);
            encounterServices.CreateEncounter(encounters);
        }

        [TestMethod]
        public void AddCommentToEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters);

            login.CreateSession("martinFowler", "user");
          
            IEnumerable<Encounter> allEncounters = encounterRepository.GetAll();

            Encounter firstEncounter = allEncounters.First();

            encounterServices.AddComment(firstEncounter, "I told you, Felix will win");
            Assert.AreEqual("I told you, Felix will win", encounterRepository.GetAll().First().Comments.First());
        }

        [TestMethod]
        public void GetAllEncounters()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters);

            IEnumerable<Encounter> result = encounterServices.GetAllEncounters();

            bool areAllPresent = encounters.All(i => result.ToList().Remove(i));

            Assert.IsTrue(areAllPresent);
        }

        [TestMethod]

        public void DeleteEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters);
            IEnumerable<Encounter> allEncounters = encounterServices.GetAllEncounters();

            encounterServices.DeleteEncounter(allEncounters.First());

            Assert.IsTrue(encounterServices.GetAllEncounters().ToList().Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void DeleteEncounterWithoutSufficientPermission()
        {
            login.CreateSession("martinFowler", "user");
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters);
            IEnumerable<Encounter> allEncounters = encounterServices.GetAllEncounters();

            encounterServices.DeleteEncounter(allEncounters.First());

            Assert.IsTrue(encounterServices.GetAllEncounters().ToList().Count == 0);
        }
    }
}