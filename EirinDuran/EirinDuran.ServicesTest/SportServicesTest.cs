using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
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
        private ILoginServices login;
        private SportRepository repo;
        private SportDTO futbol;
        private SportDTO rugby;
        private TeamDTO boca;
        private TeamDTO river;


        [TestMethod]
        public void CreatedSportTest()
        {
            SportServices service = new SportServices(login, repo);
            service.Create(rugby);

            Assert.IsTrue(repo.GetAll().Any(s => s.Name.Equals(rugby.Name)));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsException))]
        public void CreateAlreadyExistingSport()
        {
            SportServices service = new SportServices(login, repo);
            service.Create(futbol);
        }

        [TestMethod]
        public void ModifyExistingSportTest()
        {
            SportServices service = new SportServices(login, repo);
            futbol.Teams.Remove(boca);
            service.Modify(futbol);

            Sport fromRepo = repo.Get(futbol.Name);
            Assert.IsFalse(fromRepo.Teams.Contains(new Team("Boca")));
        }

        [TestMethod]
        public void ModifyNonExistingSportTest()
        {
            SportServices service = new SportServices(login, repo);
            rugby.Teams.Add(boca);
            service.Modify(rugby);

            Assert.IsTrue(repo.Get(rugby.Name).Teams.Contains(new Team(boca.Name)));
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new SportRepository(GetContextFactory());
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            repo.Add(new Sport(futbol.Name, futbol.Teams.Select(t => new Team(t.Name, t.Logo))));
            login = CreateLoginServices();
        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginSericesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));
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
                Teams = new List<TeamDTO> { boca, river }
            };
            return futbol;
        }

        private SportDTO CreateRugbyTeam()
        {
            return new SportDTO()
            {
                Name = "Rugby",
                Teams = new List<TeamDTO>()
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
