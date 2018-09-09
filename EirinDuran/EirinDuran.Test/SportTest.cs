using EirinDuran.Domain.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    }
}
