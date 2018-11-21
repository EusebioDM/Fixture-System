using SilverFixture.Domain;
using SilverFixture.Domain.Fixture;
using SilverFixture.DomainTest.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SilverFixture.DomainTest
{
    [TestClass]
    public class TeamTest
    {
        private Team boca;
        private Team river;
        private Sport futbol;

        [TestMethod]
        public void CreateTeamTest()
        {
            Team boca = CreateBocaTeam();
            Assert.AreEqual("Boca Juniors", (string)boca.Name);
        }

        [TestMethod]
        public void TeamEqualsDifferentTeamsTest()
        {
            Team boca = CreateBocaTeam();
            Team river = CreateTeamThatBelongsInTheB();
            Assert.IsFalse(boca.Equals(river));
        }

        [TestMethod]
        public void TeamEqualsSameTeamsTest()
        {
            Team boca = CreateBocaTeam();
            string path = GetResourcePath("Boca.jpeg");
            Image image = Image.FromFile(path);
            Team anotherBoca = new Team("Boca Juniors", futbol, image);
            Assert.IsTrue(boca.Equals(anotherBoca));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void NullTeamNameTest()
        {
            Team team = new Team(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void EmptyTeamNameTest()
        {
            Team team = new Team("                  ", null);
        }

        [TestMethod]
        public void AddTeamTest()
        {
            Team tomba = new Team("GodoyCruz", futbol);

            Assert.AreEqual(futbol, tomba.Sport);
        }

        [TestInitialize]
        public void TestInit()
        {
            futbol = CreateFutbolSport();
            river = CreateTeamThatBelongsInTheB();
            boca = CreateBocaTeam();
        }

        private Sport CreateFutbolSport()
        {
            return new Sport("Futbol");
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
