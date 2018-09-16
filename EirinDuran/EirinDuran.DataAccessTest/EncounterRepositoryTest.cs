using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EirinDuran.DataAccessTest
{
    using Helper = HelperFunctions<Encounter>;
    [TestClass]
    public class EncounterRepositoryTest
    {
        private EncounterRepository repo;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;
        private Team tomba;
        private Encounter bocaRiver;
        private Encounter tombaRiver;

        [TestMethod]
        public void AddEncounterTest()
        {
            IEnumerable<Encounter> actual = repo.GetAll();


            Assert.IsTrue(actual.Any(e => e.Teams.Contains(boca) && e.Teams.Contains(river) && e.Sport.Equals(futbol)));
            Assert.IsTrue(actual.Any(e => e.Teams.Contains(tomba) && e.Teams.Contains(river) && e.Sport.Equals(futbol)));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
        public void AddExistingEncounterTest()
        {
            repo.Add(bocaRiver);
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new EncounterRepository(GetContextFactory());
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            tomba = CreateGodoyCruzTeam();
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            bocaRiver = CreateBocaRiverEncounter();
            tombaRiver = CreateTombaRiverEncounter();
            repo.Add(bocaRiver);
            repo.Add(tombaRiver);
        }

        private IContextFactory GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).EnableSensitiveDataLogging().UseLazyLoadingProxies().Options;
            return new ContextFactory(options);
        }

        private Sport CreateFutbolTeam()
        {
            Sport futbol = new Sport("Futbol");
            futbol.AddTeam(boca);
            futbol.AddTeam(river);
            futbol.AddTeam(tomba);
            return futbol;
        }

        private Sport CreateRugbyTeam()
        {
            return new Sport("Rugby");
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

        private Team CreateGodoyCruzTeam()
        {
            string name = "Godoy Cruz";
            Image image = Image.FromFile("..\\..\\..\\Resources\\GodoyCruz.jpg");
            return new Team(name, image);
        }

        private Encounter CreateBocaRiverEncounter()
        {
            return new Encounter(futbol, new List<Team>() { boca, river }, new DateTime(3001, 10, 10));
        }

        private Encounter CreateTombaRiverEncounter()
        {
            return new Encounter(futbol, new List<Team>() { tomba, river }, new DateTime(3001, 10, 11));
        }
    }
}
