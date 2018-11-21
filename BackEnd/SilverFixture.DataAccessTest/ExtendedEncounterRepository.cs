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
    [TestClass]
    public class EncounterExtendedRepositoryTest
    {
        private ExtendedEncounterRepository repo;
        private Sport football;
        private Sport rugby;
        private Team boca;
        private Team river;
        private Team tomba;
        private Encounter bocaRiver;
        private Encounter tombaRiver;
        private User macri;

        [TestMethod]
        public void GetSingleEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam(GetTeamId(boca));

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.AreEqual(1, encounters.Count());
        }

        [TestMethod]
        public void GetMultipleEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam(GetTeamId(river));

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(2, encounters.Count());
        }

        [TestMethod]
        public void GetNoEncountersByTeamTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByTeam("Independiente_Futbol");

            Assert.AreEqual(0, encounters.Count());
        }

        private string GetTeamId(Team team)
        {
            return team.Name + "_" + team.Sport.Name;
        }

        [TestMethod]
        public void GetSingleEncountersBySportTest()
        {
            Sport rugby = new Sport("Rugby");
            Team allBlacks = new Team("AllBlacks", rugby);
            Team someTeam = new Team("SomeTeam", rugby);
            Encounter encounter = new Encounter(rugby, new List<Team>(){ allBlacks, someTeam }, new DateTime(3000, 1, 1));
            repo.Add(encounter);
            IEnumerable<Encounter> encounters = repo.GetBySport("Rugby");

            Assert.IsTrue(encounters.Contains(encounter));
            Assert.AreEqual(1, encounters.Count());
        }

        [TestMethod]
        public void GetMultipleEncountersBySportTest()
        {
            IEnumerable<Encounter> encounters = repo.GetBySport("Futbol");

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(2, encounters.Count());
        }

        [TestMethod]
        public void GetNoEncountersBySportTest()
        {
            IEnumerable<Encounter> encounters = repo.GetBySport("Rugby");

            Assert.AreEqual(0, encounters.Count());
        }

        [TestMethod]
        public void GetSingleEncountersByDateTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByDate(new DateTime(3000, 10, 1), new DateTime(3000, 10, 8));

            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(1, encounters.Count());
        }

        [TestMethod]
        public void GetMultipleEncountersByDateTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByDate(new DateTime(3000, 10, 1), new DateTime(3000, 10, 29));

            Assert.IsTrue(encounters.Contains(bocaRiver));
            Assert.IsTrue(encounters.Contains(tombaRiver));
            Assert.AreEqual(2, encounters.Count());
        }

        [TestMethod]
        public void GetNoEncountersByDateTest()
        {
            IEnumerable<Encounter> encounters = repo.GetByDate(new DateTime(3000, 10, 6), new DateTime(3000, 10, 9));

            Assert.AreEqual(0, encounters.Count());
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new ExtendedEncounterRepository(GetContextFactory());
            football = CreateFutbolTeam();
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
            return new Team(name, football, image);

        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            string path = GetResourcePath("River.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, football, image);
        }

        private Team CreateGodoyCruzTeam()
        {
            string name = "Godoy Cruz";
            string path = GetResourcePath("GodoyCruz.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, football, image);
        }

        private Encounter CreateBocaRiverEncounter()
        {
            Encounter encounter = new Encounter(football, new List<Team>() { boca, river }, new DateTime(3000, 10, 10));
            encounter.AddComment(macri, "Meow");
            return encounter;
        }

        private Encounter CreateTombaRiverEncounter()
        {
            Encounter encounter = new Encounter(football, new List<Team>() { tomba, river }, new DateTime(3000, 10, 5));
            encounter.AddComment(macri, "Meow");
            return encounter;
        }

        private User CreateMacriUser()
        {
            User user = new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
            user.AddFollowedTeam(new Team("River", football));
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

