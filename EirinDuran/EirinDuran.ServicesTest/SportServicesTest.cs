using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.Services.DTO_Mappers;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class SportServicesTest
    {
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private ILoginServices login;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Encounter> encounterRepo;
        private SportDTO futbol;
        private SportDTO rugby;
        private TeamDTO boca;
        private TeamDTO river;


        [TestMethod]
        public void CreatedSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            service.Create(rugby);

            Assert.IsTrue(sportRepo.GetAll().Any(s => s.Name.Equals(rugby.Name)));
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void CreateAlreadyExistingSport()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            service.Create(futbol);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void CreateInvalidNullSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            SportDTO sport = new SportDTO();
            service.Create(sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void CreateInvalidNameSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            SportDTO sport = new SportDTO()
            {
                Name = "                  "
            };
            service.Create(sport);
        }

        [TestMethod]
        public void ModifyExistingSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            futbol.TeamsNames.Remove(boca.Name);
            service.Modify(futbol);

            Sport fromRepo = sportRepo.Get(futbol.Name);
            Assert.IsFalse(fromRepo.Teams.Contains(new Team("Boca")));
        }

        [TestMethod]
        public void ModifyNonExistingSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            rugby.TeamsNames.Add(boca.Name);
            service.Modify(rugby);

            Assert.IsTrue(sportRepo.Get(rugby.Name).Teams.Contains(new Team(boca.Name)));
        }

        [TestMethod]
        public void ListAllEncountersOfASpecificSport()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo, encounterRepo);
            IRepository<Encounter> encounterRepository = new EncounterRepository(contextFactory);
            EncounterMapper mapper = new EncounterMapper(sportRepo, teamRepo);

            TeamMapper teamMapper = new TeamMapper();
            SportMapper sportMapper = new SportMapper(teamRepo);

            Sport sport = sportMapper.Map(CreateFutbolTeam());

            Team boca = teamMapper.Map(CreateBocaTeam());
            Team river = teamMapper.Map(CreateTeamThatBelongsInTheB());

            List<Team> teams = new List<Team>() { boca, river };

            DateTime encounterDate = new DateTime(2020, 12, 12);

            Encounter encounter = new Encounter(sport, teams, encounterDate);

            encounterRepository.Add(encounter);

            IEnumerable<EncounterDTO> recovered = service.GetAllEncountersOfASpecificSport("Futbol");

            Assert.AreEqual(1, recovered.ToList().Count);
            Assert.AreEqual(boca.Name, recovered.ToList()[0].HomeTeamName);
            Assert.AreEqual(river.Name, recovered.ToList()[0].AwayTeamName);
        }

        [TestInitialize]
        public void TestInit()
        {
            contextFactory = GetContextFactory();
            sportRepo = new SportRepository(contextFactory);
            teamRepo = new TeamRepository(contextFactory);
            encounterRepo = new EncounterRepository(contextFactory);
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            sportRepo.Add(new Sport(futbol.Name, new List<Team>() { new Team(boca.Name, boca.Logo), new Team(river.Name, river.Logo) }));
            var a = sportRepo.GetAll();
            login = CreateLoginServices();
        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginServicesMock(new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            });
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }


        private TeamDTO CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = Image.FromFile(GetResourcePath("Boca.jpg"));
            return new TeamDTO()
            {
                Name = name,
                Logo = image
            };

        }

        private TeamDTO CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = Image.FromFile(GetResourcePath("River.jpg"));
            return new TeamDTO()
            {
                Name = name,
                Logo = image
            };
        }

        private SportDTO CreateFutbolTeam()
        {
            SportDTO futbol = new SportDTO()
            {
                Name = "Futbol",
                TeamsNames = new List<string> { boca.Name, river.Name }
            };
            return futbol;
        }

        private SportDTO CreateRugbyTeam()
        {
            return new SportDTO()
            {
                Name = "Rugby",
                TeamsNames = new List<string>()
            };
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}
