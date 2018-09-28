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
        private SportDTO futbol;
        private SportDTO rugby;
        private TeamDTO boca;
        private TeamDTO river;


        [TestMethod]
        public void CreatedSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            service.Create(rugby);

            Assert.IsTrue(sportRepo.GetAll().Any(s => s.Name.Equals(rugby.Name)));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsException))]
        public void CreateAlreadyExistingSport()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            service.Create(futbol);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void CreateInvalidNullSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            SportDTO sport = new SportDTO();
            service.Create(sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void CreateInvalidNameSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            SportDTO sport = new SportDTO()
            {
                Name = "                  "
            };
            service.Create(sport);
        }

        [TestMethod]
        public void ModifyExistingSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            futbol.TeamsNames.Remove(boca.Name);
            service.Modify(futbol);

            Sport fromRepo = sportRepo.Get(futbol.Name);
            Assert.IsFalse(fromRepo.Teams.Contains(new Team("Boca")));
        }

        [TestMethod]
        public void ModifyNonExistingSportTest()
        {
            SportServices service = new SportServices(login, sportRepo, teamRepo);
            rugby.TeamsNames.Add(boca.Name);
            service.Modify(rugby);

            Assert.IsTrue(sportRepo.Get(rugby.Name).Teams.Contains(new Team(boca.Name)));
        }

        [TestInitialize]
        public void TestInit()
        {
            contextFactory = GetContextFactory();
            sportRepo = new SportRepository(contextFactory);
            teamRepo = new TeamRepository(contextFactory);
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
