using EirinDuran.Domain;
using EirinDuran.Domain.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.Test
{
    [TestClass]
    public class SportTest
    {
        [TestMethod]
        public void CreateTeamTest()
        {
            Sport sport = new Sport("Futbol");

            Assert.AreEqual("Futbol", sport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyFieldException))]
        public void CreateTeamEmptyNameTest()
        {
            Sport sport = new Sport("    ");
        }

        [TestMethod]
        public void AddTeamTest()
        {
            Sport sport = new Sport("Futbol");
            Team boca = new Team("Boca", null);
            Team river = new Team("River", null);
            sport.AddTeam(boca);
            sport.AddTeam(river);

            IEnumerable<Team> actual = sport.Teams;
            Assert.IsTrue(actual.Contains(boca) && actual.Contains(river));
        }
    }
}
