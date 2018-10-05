using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
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
    [TestClass]
    public class EncounterExtendedRepositoryTest
    {
        private ExtendedEncounterRepository repo;
        private Sport futbol;
        private Sport rugby;
        private Team boca;
        private Team river;
        private Team tomba;
        private Encounter bocaRiver;
        private Encounter tombaRiver;
        private User macri;

        [TestMethod]
        private void GetSingleEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam(boca);

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.AreEqual(1, encounters.Count());
        }

        [TestMethod]
        private void GetMultipleEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam(river);

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(2, encounters.Count());
        }

        [TestMethod]
        private void GetNoEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam(river);

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(2, encounters.Count());
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new ExtendedEncounterRepository(GetContextFactory());
            futbol = CreateFutbolTeam();
            rugby = CreateRugbyTeam();
            macri = CreateMacriUser();
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            tomba = CreateGodoyCruzTeam();
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
            return new Team(name, futbol, image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            string path = GetResourcePath("River.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, futbol, image);
        }

        private Team CreateGodoyCruzTeam()
        {
            string name = "Godoy Cruz";
            string path = GetResourcePath("GodoyCruz.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, futbol, image);
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
