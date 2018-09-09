using EirinDuran.Domain.Fixture;
using EirinDuran.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace EirinDuran.Test
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
