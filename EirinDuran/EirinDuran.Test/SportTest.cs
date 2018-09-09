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
    }
}
