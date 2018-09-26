using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
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
        private IRepository<User> userRepo;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Encounter> encounterRepo;
        private Services.DTO_Mappers.EncounterMapper mapper;

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

            userRepo = new UserRepository(GetContextFactory());
            sportRepo = new SportRepository(GetContextFactory());
            teamRepo = new TeamRepository(GetContextFactory());
            encounterRepo = new EncounterRepository(GetContextFactory());
            mapper = new Services.DTO_Mappers.EncounterMapper(sportRepo, teamRepo);
            userRepo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            userRepo.Add(new User(Role.Follower, "martinFowler", "Martin", "Fowler", "user", "fowler@fowler.com"));
            login = new LoginServices(userRepo);
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            felix = new Team("Felix");
            teamRepo.Add(felix);
            liverpool = new Team("Liverpool");
            teamRepo.Add(liverpool);
            river = new Team("River Plate");
            teamRepo.Add(river);
            cerro = new Team("Cerro");
            teamRepo.Add(cerro);
            torque = new Team("Torque");
            teamRepo.Add(torque);
            danubio = new Team("Danubio");
            teamRepo.Add(danubio);
            penhiarol = new Team("Peñarol");
            teamRepo.Add(penhiarol);
            atenas = new Team("Atenas");
            teamRepo.Add(atenas);
            wanderers = new Team("Wanderes");
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

            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(2018, 10, 07);

            Sport basketball = new Sport("Basketball");

            basketball.AddTeam(river);
            basketball.AddTeam(felix);
            sportRepo.Add(basketball);

            Encounter expected = new Encounter(basketball, teams, date);
            encounterServices.CreateEncounter(mapper.Map(expected));

            List<Encounter> recovered = (List<Encounter>)encounterRepo.GetAll();

            bool result = recovered.Contains(expected);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void CreateFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");

            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);
            IEnumerable<Team> teams = new List<Team> { felix, river };
            DateTime date = new DateTime(2018, 10, 07);

            Sport basketball = new Sport("Basketball");

            basketball.AddTeam(river);
            basketball.AddTeam(felix);

            Encounter encounter = new Encounter(basketball, teams, date);
            encounterServices.CreateEncounter(mapper.Map(encounter));
        }

        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");

            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teamsFirstEncounter = new List<Team> { felix, river };
            IEnumerable<Team> teamsSecondEncounter = new List<Team> { felix, penhiarol };

            DateTime date = new DateTime(2018, 10, 07);

            Sport football = new Sport("Football");

            football.AddTeam(river);
            football.AddTeam(felix);
            football.AddTeam(penhiarol);
            sportRepo.Add(football);

            Encounter encounter = new Encounter(football, teamsFirstEncounter, date);
            Encounter encounterOverlappingDates = new Encounter(football, teamsSecondEncounter, date);
            encounterServices.CreateEncounter(mapper.Map(encounter));
            encounterServices.CreateEncounter(mapper.Map(encounterOverlappingDates));
        }

        [TestMethod]
        public void CreateAutoGeneratedFixture()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

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
            sportRepo.Add(football);

            DateTime date = new DateTime(2018, 10, 10);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));

            List<Encounter> recovered = (List<Encounter>)encounterRepo.GetAll();

            bool areAllPresent = recovered.All(i => encounters.ToList().Remove(i));

            Assert.IsTrue(areAllPresent);
        }

        [TestMethod]
        [ExpectedException(typeof(EncounterWithOverlappingDatesException))]
        public void CreateAutoGeneratedFixtureWithOverlappingDates()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river, penhiarol, atenas };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            football.AddTeam(penhiarol);
            football.AddTeam(atenas);
            sportRepo.Add(football);

            DateTime dateFirstEncounters = new DateTime(2018, 10, 10);
            DateTime dateSecondEncounters = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);

            IEnumerable<Encounter> firstEncounters = fixture.GenerateFixture(teams, dateFirstEncounters);
            IEnumerable<Encounter> secondEncounters = fixture.GenerateFixture(teams, dateSecondEncounters);

            encounterServices.CreateEncounter(firstEncounters.Select(e => mapper.Map(e)));
            encounterServices.CreateEncounter(secondEncounters.Select(e => mapper.Map(e)));
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void CreateAutoGeneratedFixtureWithoutPermissions()
        {
            login.CreateSession("martinFowler", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);

            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);
            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));
        }

        [TestMethod]
        public void AddCommentToEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            sportRepo.Add(football);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));

            login.CreateSession("martinFowler", "user");

            IEnumerable<Encounter> allEncounters = encounterRepo.GetAll();

            Encounter firstEncounter = allEncounters.First();

            encounterServices.AddComment(firstEncounter, "I told you, Felix will win");
            Assert.AreEqual("I told you, Felix will win", encounterRepo.GetAll().First().Comments.First().ToString());
        }

        [TestMethod]
        public void GetAllEncounters()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            sportRepo.Add(football);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));

            IEnumerable<Encounter> result = encounterServices.GetAllEncounters();

            bool areAllPresent = encounters.All(i => result.ToList().Remove(i));

            Assert.IsTrue(areAllPresent);
        }

        [TestMethod]

        public void DeleteEncounter()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            sportRepo.Add(football);
            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));
            IEnumerable<Encounter> allEncounters = encounterServices.GetAllEncounters();

            encounterServices.DeleteEncounter(allEncounters.First().Id.ToString());

            Assert.IsTrue(encounterServices.GetAllEncounters().ToList().Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void DeleteEncounterWithoutSufficientPermission()
        {
            login.CreateSession("martinFowler", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            IEnumerable<Team> teams = new List<Team> { felix, river };

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);

            DateTime date = new DateTime(2018, 10, 12);

            IFixtureGenerator fixture = new LeagueFixture(football);
            IEnumerable<Encounter> encounters = fixture.GenerateFixture(teams, date);

            encounterServices.CreateEncounter(encounters.Select(e => mapper.Map(e)));
            IEnumerable<Encounter> allEncounters = encounterServices.GetAllEncounters();

            encounterServices.DeleteEncounter(allEncounters.First().Id.ToString());

            Assert.IsTrue(encounterServices.GetAllEncounters().ToList().Count == 0);
        }

        [TestMethod]
        public void ListEncountersForATeam()
        {
            login.CreateSession("sSanchez", "user");
            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            Sport football = new Sport("Football");
            football.AddTeam(felix);
            football.AddTeam(river);
            football.AddTeam(penhiarol);
            football.AddTeam(atenas);
            football.AddTeam(torque);
            football.AddTeam(wanderers);
            football.AddTeam(liverpool);
            sportRepo.Add(football);

            Encounter encounter1 = new Encounter(football, new List<Team> { felix, river }, new DateTime(2018, 10, 05));
            Encounter encounter2 = new Encounter(football, new List<Team> { atenas, wanderers }, new DateTime(2018, 10, 07));
            Encounter encounter3 = new Encounter(football, new List<Team> { penhiarol, torque }, new DateTime(2018, 10, 09));
            Encounter encounter4 = new Encounter(football, new List<Team> { river, liverpool }, new DateTime(2018, 10, 11));

            encounterServices.CreateEncounter(mapper.Map(encounter1));
            encounterServices.CreateEncounter(mapper.Map(encounter2));
            encounterServices.CreateEncounter(mapper.Map(encounter3));
            encounterServices.CreateEncounter(mapper.Map(encounter4));

            IEnumerable<Encounter> encountersRiver = encounterServices.GetAllEncounters(river);
            Assert.IsTrue(encountersRiver.ToList().Count == 2);
        }

        [TestMethod]
        public void ListCommentsWithFollowedTeams()
        {
            Sport basketball = new Sport("Baskteball");

            login.CreateSession("sSanchez", "user");

            Team cavaliers = new Team("Cavaliers");
            teamRepo.Add(cavaliers);
            TeamDTO cavaliersDTO = new TeamDTO() { Name = "Cavaliers", Logo = Image.FromFile(GetResourcePath("Cavaliers.jpg")) };
            Team celtics = new Team("Celtics");
            teamRepo.Add(celtics);
            Team pistons = new Team("Pistons");
            teamRepo.Add(pistons);
            Team raptors = new Team("Raptors");
            teamRepo.Add(raptors);

            List<Team> teamList1 = new List<Team>();
            List<Team> teamList2 = new List<Team>();

            basketball.AddTeam(cavaliers);
            basketball.AddTeam(celtics);
            basketball.AddTeam(pistons);
            basketball.AddTeam(raptors);
            sportRepo.Add(basketball);

            teamList1.Add(cavaliers);
            teamList1.Add(celtics);

            teamList2.Add(pistons);
            teamList2.Add(raptors);

            Encounter encounter1 = new Encounter(basketball, teamList1, new DateTime(2018, 10, 09));
            Encounter encounter2 = new Encounter(basketball, teamList1, new DateTime(2018, 10, 10));

            EncounterServices encounterServices = new EncounterServices(login, encounterRepo, sportRepo, teamRepo);

            encounterServices.CreateEncounter(mapper.Map(encounter1));
            encounterServices.CreateEncounter(mapper.Map(encounter2));

            login.CreateSession("martinFowler", "user");
            encounter1.AddComment(login.LoggedUser, "Yes, we can!");
            encounterServices.AddComment(encounter1, "Yes, we can!");

            UserServices userServices = new UserServices(login, userRepo, teamRepo);

            userServices.AddFollowedTeam(cavaliersDTO);
            IEnumerable<Encounter> followedTeamsInEncounter = encounterServices.GetAllEncountersWithFollowedTeams();

            Assert.IsTrue(followedTeamsInEncounter.ToList().Count == 1);
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}