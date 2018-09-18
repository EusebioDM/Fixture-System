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

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class EncounterServicesTest
    {
        private LoginServices login;
        private UserRepository userRepository;
        private EncounterRepository encounterRepository;

        private Image image;
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
            userRepository.Add(new User(Role.Follower, "martinFowler", "Martín", "Fowler", "user", "fowler@fowler.com"));
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
            penhiarol = new Team("Peñarol");
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
            encounterServices.CreateEncounter(expected);

            bool result = recovered.Contains(expected);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void CreateFeatureWithoutPermissions()
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
    }
}