using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private User macri;

        [TestMethod]
        public void AddEncounterTest()
        {
            IEnumerable<Encounter> actual = repo.GetAll();

            Assert.IsTrue(actual.Any(e => e.Teams.Contains(boca) && e.Teams.Contains(river) && e.Sport.Equals(futbol)));
            Assert.IsTrue(actual.Any(e => e.Comments.Any(m => m.Message.Equals("Meow"))));
            Assert.IsTrue(actual.Any(e => e.Teams.Contains(tomba) && e.Teams.Contains(river) && e.Sport.Equals(futbol)));
            Assert.AreEqual(2, actual.Count());
        }

        [TestMethod]
        public void UpdateNonExistantEncounterTest()
        {
            Encounter tombaBoca = new Encounter(futbol, new List<Team> { boca, river }, new DateTime(3001, 10, 1));
            repo.Update(tombaBoca);

            Assert.AreEqual(3, repo.GetAll().Count());
        }

        [TestMethod]
        public void UpdateEncounterTest()
        {
            Encounter encounter = repo.GetAll().First(e => e.Teams.Contains(boca));
            encounter.AddComment(macri, "msj");
            repo.Update(encounter);

            Encounter updated = repo.GetAll().First(e => e.Teams.Contains(boca));
            Assert.IsTrue(updated.Comments.Any(c => c.Message.Equals("msj")));
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new EncounterRepository(GetContextFactory());
            macri = CreateMacriUser();
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

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
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
            string path = GetResourcePath("Boca.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            string path = GetResourcePath("River.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, image);
        }

        private Team CreateGodoyCruzTeam()
        {
            string name = "Godoy Cruz";
            string path = GetResourcePath("GodoyCruz.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, image);
        }

        private Encounter CreateBocaRiverEncounter()
        {
            Encounter encounter = new Encounter(futbol, new List<Team>() { boca, river }, new DateTime(3001, 10, 10));
            encounter.AddComment(macri, "Meow");
            return encounter;
        }

        private Encounter CreateTombaRiverEncounter()
        {
            Encounter encounter = new Encounter(futbol, new List<Team>() { tomba, river }, new DateTime(3001, 10, 11));
            encounter.AddComment(macri, "Meow");
            return encounter;
        }

        private User CreateMacriUser()
        {
            User user = new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
            user.AddFollowedTeam(new Team("River"));
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
