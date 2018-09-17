using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccessTest
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
        [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
        public void AddExistingSportTest()
        {
            repo.Add(rugby);
        }

        [TestMethod]
        public void RemoveSportTest()
        {
            repo.Delete(rugby);

            IEnumerable<Sport> actual = repo.GetAll();
            IEnumerable<Sport> expected = new List<Sport> { futbol };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void RemoveNonExistingSportTest()
        {
            repo.Delete(new Sport("hockey"));
        }

        [TestMethod]
        public void GetSportTest()
        {
            Sport fromRepo = repo.Get(rugby);
            Assert.AreEqual(rugby.Name, fromRepo.Name);
            Assert.IsTrue(HelperFunctions<Team>.CollectionsHaveSameElements(rugby.Teams, fromRepo.Teams));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void GetNonExistantSportTest()
        {
            repo.Get(new Sport("tennis"));
        }

        [TestMethod]
        public void UpdateSportTest()
        {
            rugby.AddTeam(boca);
            repo.Update(rugby);
            Sport fromRepo = repo.Get(rugby);

            Assert.IsTrue(fromRepo.Teams.Contains(boca));
        }

        [TestMethod]
        public void UpdateNonExistantSportTest()
        {
            Sport lasLeonas = new Sport("Hockey", new List<Team>() { new Team("Las Leonas") });
            repo.Update(lasLeonas);
            Sport fromRepo = repo.Get(new Sport("Hockey"));

            Assert.IsTrue(HelperFunctions<Team>.CollectionsHaveSameElements(lasLeonas.Teams, fromRepo.Teams));
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
            Sport futbol = new Sport("Futbol");
            futbol.AddTeam(boca);
            futbol.AddTeam(river);
            return futbol;
        }

        private Sport CreateRugbyTeam()
        {
            return new Sport("Rugby");
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging().UseLazyLoadingProxies().Options;
            return new InMemoryContextFactory(options);
        }

        private void CleanUpRepo()
        {
            foreach (Sport sport in repo.GetAll())
            {
                repo.Delete(sport);
            }
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = Image.FromFile("..\\..\\..\\Resources\\Boca.jpg");
            return new Team(name, image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = Image.FromFile("..\\..\\..\\Resources\\River.jpg");
            return new Team(name, image);
        }
    }
}
