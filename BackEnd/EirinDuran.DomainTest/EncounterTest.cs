using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.DomainTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using EirinDuran.Domain;

namespace EirinDuran.DomainTest
{
    [TestClass]
    public class EncounterTest
    {
        private DateTime fechaMenor;
        private DateTime fechaMayor;
        private Team boca;
        private Team river;
        private List<Team> teams;
        private Sport futbol;
        private Sport sailing;
        private Team redBull;
        private Team azzurra;
        private Team aleph;
        private Team artemis;

        [TestMethod]
        public void CreateEncounterTest()
        {
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);

            Assert.IsTrue(encounter.DateTime.Equals(fechaMenor) && encounter.Sport.Equals(futbol));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void HigherNumberOfTeamsTest()
        {
            teams.Add(new Team("Godoy Cruz Antonio Tomba", null));
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void LowerNumberOfTeamsTest()
        {
            teams.Remove(river);
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamException))]
        public void InvalidTeamSportTest()
        {
            Encounter encounter = new Encounter(new Sport("Rugby"), teams, fechaMenor);

        }

        [TestMethod]
        public void AddMessageTest()
        {
            User user = new User("User");
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);
            encounter.AddComment(user, "msj");

            Comment comment = encounter.Comments.Single();
            Assert.AreEqual(user, comment.User);
            Assert.AreEqual("msj", comment.Message);
            Assert.AreEqual(DateTime.Now.Hour, comment.TimeStamp.Hour);
        }

        [TestMethod]
        public void MoreThan2PlayersEncounterTest()
        {
            Encounter encounter = new Encounter(sailing, new List<Team>(){azzurra, redBull, artemis, aleph}, new DateTime(3000,10,10));
            
            Assert.IsTrue(encounter.Teams.Contains(redBull));
            Assert.IsTrue(encounter.Teams.Contains(azzurra));
            Assert.IsTrue(encounter.Teams.Contains(artemis));
            Assert.IsTrue(encounter.Teams.Contains(aleph));
        }
        
        [TestInitialize]
        public void TestInit()
        {
            fechaMenor = new DateTime(3000, 1, 20, 1, 1, 1);
            fechaMayor = new DateTime(3000, 1, 21, 1, 1, 1);
            futbol = new Sport("Futbol");
            sailing = new Sport("Sailing", EncounterPlayerCount.MoreThanTwoPlayers);
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            teams = new List<Team>() { boca, river };
            redBull = new Team("RedBull", sailing);
            azzurra = new Team("Azzurra", sailing);
            aleph = new Team("Aleph", sailing);
            artemis = new Team("Artemis", sailing);
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            string path = GetResourcePath("Boca.jpeg");
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

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}
