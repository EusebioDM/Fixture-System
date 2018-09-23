﻿using EirinDuran.Domain;
using EirinDuran.Domain.Fixture;
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

        [TestMethod]
        public void TeamEqualsSameTeamsTest()
        {
            Team boca = CreateBocaTeam();
            string path = GetResourcePath("Boca.jpeg");
            Image image = Image.FromFile(path);
            Team anotherBoca = new Team("Boca Juniors", image);
            Assert.IsTrue(boca.Equals(anotherBoca));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyFieldException))]
        public void NullTeamNameTest()
        {
            Team team = new Team(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyFieldException))]
        public void EmptyTeamNameTest()
        {
            Team team = new Team("                  ", null);
        }

        private Team CreateBocaTeam()
        {
            string name = "Boca Juniors";
            string path = GetResourcePath("Boca.jpeg");
            Image image = Image.FromFile(path);
            return new Team(name, image);
        }

        private Team CreateTeamThatBelongsInTheB()
        {
            string name = "River Plate";
            string path = GetResourcePath("River.jpg");
            Image image = Image.FromFile(path);
            return new Team(name, image);
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}
