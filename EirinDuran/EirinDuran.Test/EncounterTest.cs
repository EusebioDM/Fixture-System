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

        [TestMethod]
        public void CreateEncounterTest()
        {
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);

            Assert.IsTrue(encounter.DateTime.Equals(fechaMenor) && encounter.Sport.Equals(futbol));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void HigherNumberOfTeamsTest()
        {
            teams.Add(new Team("Godoy Cruz Antonio Tomba",null));
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void LowerNumberOfTeamsTest()
        {
            teams.Remove(river);
            Encounter encounter = new Encounter(futbol, teams, fechaMenor);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDateException))]
        public void InvalidDateException()
        {
            Encounter encounter = new Encounter(futbol, teams, new DateTime(1900,1,1));
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

        [TestInitialize]
        public void TestInit()
        {
            fechaMenor = new DateTime(3000, 1, 20, 1, 1, 1);
            fechaMayor = new DateTime(3000, 1, 21, 1, 1, 1);
            boca = CreateBocaTeam();
            river = CreateTeamThatBelongsInTheB();
            teams = new List<Team>()
            {
                boca, river
            };
            futbol = new Sport("Futbol");
            futbol.AddTeam(boca);
            futbol.AddTeam(river);
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = GetImage(Resources.Boca);
            return new Team(name, image);
        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            Image image = GetImage(Resources.River);
            return new Team(name, image);
        }

        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }
    }
}
