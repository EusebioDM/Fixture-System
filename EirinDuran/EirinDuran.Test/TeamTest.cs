using EirinDuran.Domain.Fixture;
using EirinDuran.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Image = System.Drawing.Image;

namespace EirinDuran.Test
{
    [TestClass]
    public class TeamTest
    {

        [TestMethod]
        public void CreateTeamTest()
        {
            Team boca = CreateBocaTeam();
            Assert.AreEqual("Boca Juniors", boca.Name);
        }

        [TestMethod]
        public void TeamEqualsDifferentTeamsTest()
        {
            Team boca = CreateBocaTeam();
            Team river = CreateTeamThatBelongsInTheB();
            Assert.IsFalse(boca.Equals(river));
        }

        private object GetImage(byte[] boca, object image)
        {
            throw new NotImplementedException();
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
