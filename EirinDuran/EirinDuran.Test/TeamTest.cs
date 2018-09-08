using EirinDuran.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace EirinDuran.Test
{
    [TestClass]
    public class TeamTest
    {

        [TestMethod]
        public void CreateTeamNameTest()
        {
            Team boca = CreateBocaTeam();
            Assert.AreEqual("Boca Juniors", boca.Name);
        }

        [TestMethod]
        public void CreateTeamImageTest()
        {
            Team boca = CreateBocaTeam();
            Assert.AreEqual(GetImage(Resources.Boca, boca.Image);
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            Image image = GetImage(Resources.Boca);
            Team boca = new Team(name, image);
        }

        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }
    }
}
