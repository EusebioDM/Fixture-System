using EirinDuran.Domain.Fixture;
using EirinDuran.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EirinDuran.Test
{
    [TestClass]
    public class FixtureTest
    {
        
        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void SampleAutoFixtureLeagueTest()
        {   
            Image image = GetImage(Resources.River);
            
            Team boca = new Team("boca", image);
            Team cerro = new Team("cerro", image);

            Sport football = new Sport("football");

            football.AddTeam(boca);
            football.AddTeam(cerro);

            List<Team> teams = new List<Team>();

            teams.Add(boca);
            teams.Add(cerro);

            DateTime date = new DateTime(2018, 10, 07, 18, 30, 00); 

            Encounter expected = new Encounter(football, teams, date);

            IFixtureGenerator leagueFixture = new LeagueFixture();

            List<Encounter> result = leagueFixture.GenerateFixture(teams, date).ToList();

            Assert.AreEqual(expected, result);
        }

        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }

    }
}
