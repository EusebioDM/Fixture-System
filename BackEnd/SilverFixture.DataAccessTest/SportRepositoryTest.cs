using SilverFixture.DataAccess;
using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SilverFixture.DataAccessTest
{
    using Helper = HelperFunctions<Sport>;
    [TestClass]
    public class SportRepositoryTest
    {
        private SportRepository repo;
        private IDesignTimeDbContextFactory<Context> contextFactory;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;

        [TestMethod]
        public void AddSportTest()
        {
            IEnumerable<Sport> actual = repo.GetAll();
            IEnumerable<Sport> expected = new List<Sport> { rugby, futbol };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void AddExistingSportTest()
        {
            repo.Add(rugby);
        }

        [TestMethod]
        public void RemoveSportTest()
        {
            repo.Delete(rugby.Name);

            IEnumerable<Sport> actual = repo.GetAll();
            IEnumerable<Sport> expected = new List<Sport> { futbol };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveNonExistingSportTest()
        {
            repo.Delete("hockey");
        }

        [TestMethod]
        public void GetSportTest()
        {
            Sport fromRepo = repo.Get(rugby.Name);
            Assert.AreEqual(rugby.Name, fromRepo.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void GetNonExistantSportTest()
        {
            repo.Get("tennis");
        }

        [TestInitialize]
        public void TestInit()
        {
            contextFactory = GetContextFactory();
            repo = new SportRepository(contextFactory);
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            repo.Add(futbol);
            repo.Add(rugby);
        }

        private Sport CreateFutbolTeam()
        {
            Sport futbol = new Sport("Futbol");;
            return futbol;
        }

        private Sport CreateRugbyTeam()
        {
            return new Sport("Rugby");
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        private void CleanUpRepo()
        {
            foreach (Sport sport in repo.GetAll())
            {
                repo.Delete(sport.Name);
            }
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = Image.FromFile(GetResourcePath("Boca.jpg"));
            return new Team(name, futbol,image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = Image.FromFile(GetResourcePath("River.jpg"));
            return new Team(name, futbol,image);
        }

        private User CreateUserMacri()
        {
            User user = new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
            user.AddFollowedTeam(new Team("River", futbol));
            return user;
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));

        }
    }
}
