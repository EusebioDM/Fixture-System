using EirinDuran.Domain;
using EirinDuran.Domain.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EirinDuran.DomainTest
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
        public void EqualsTrueTest()
        {
            Sport futboll = new Sport("Futbol");
            Sport anotherFutboll = new Sport("Futbol");

            Assert.AreEqual(futboll, anotherFutboll);
        }

        [TestMethod]
        public void EqualsFalseTest()
        {
            Sport futbol = new Sport("Futbol");
            Sport basquet = new Sport("Basquet");

            Assert.AreNotEqual(futbol, basquet);
        }

        [TestMethod]
        public void EncounterSizeTwoPlayersTest()
        {
            Sport futbol = new Sport("Futbol", EncounterPlayerCount.TwoPlayers);

            Assert.Equals(EncounterPlayerCount.TwoPlayers, futbol.EncounterPlayerCount);
        }
        
        [TestMethod]
        public void EncounterSizeMoreThanTwoPlayersTest()
        {
            Sport futbol = new Sport("Futbol", EncounterPlayerCount.TwoPlayers);

            Assert.Equals(EncounterPlayerCount.TwoPlayers, futbol.EncounterPlayerCount);
        }
        
        
    }
}