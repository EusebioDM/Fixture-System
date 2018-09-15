using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
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
        SportRepository repo;
        IContext context;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;

        [TestMethod]
        public void AddSportTest()
        {
            repo.Add(rugby);

            IEnumerable<Sport> actual = repo.GetAll();
            IEnumerable<Sport> expected = new List<Sport> { rugby };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
        public void AddExistingSportTest()
        {
            repo.Add(rugby);
            repo.Add(rugby);
        }

        [TestMethod]
        public void RemoveSportTest()
        {
            repo.Add(rugby);
            repo.Add(futbol);
            repo.Delete(rugby);

            IEnumerable<Sport> actual = repo.GetAll();
            IEnumerable<Sport> expected = new List<Sport> { futbol };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void RemoveNonExistingSportTest()
        {
            repo.Delete(rugby);
        }

        [TestMethod]
        public void GetSportTest()
        {
            repo.Add(rugby);
            Sport fromRepo = repo.Get(rugby.Name);
            Assert.AreEqual(rugby.Name, fromRepo.Name);
            Assert.IsTrue(HelperFunctions<Team>.CollectionsHaveSameElements(rugby.Teams, fromRepo.Teams));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void GetNonExistantSportTest()
        {
            repo.Get("hockey");
        }

        [TestMethod]
        public void UpdateSportTest()
        {
            repo.Add(rugby);
            rugby.AddTeam(boca);

            repo.Update(rugby);
            Sport fromRepo = repo.Get(rugby.Name);

            Assert.IsTrue(fromRepo.Teams.Contains(boca));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void UpdateNonExistantSportTest()
        {
            repo.Update(rugby);
        }

        [TestInitialize]
        public void TestInit()
        {
            context = GetTestContext();
            repo = new SportRepository(context);
            CleanUpRepo();
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            futbol = GetFutbolSport();
            rugby = GetSportRugby();
        }

        private Sport GetFutbolSport()
        {
            Sport futbol =  new Sport("Futbol");
            futbol.AddTeam(boca);
            futbol.AddTeam(river);
            return futbol;
        }

        private Sport GetSportRugby()
        {
            return new Sport("Rugby");
        }

        private IContext GetTestContext()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("In Memory Test DB").Options;
            return new Context(options);
        }

        private void CleanUpRepo()
        {
            foreach (Sport sport in repo.GetAll())
                repo.Delete(sport);
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
