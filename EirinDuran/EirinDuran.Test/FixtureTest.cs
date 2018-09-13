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

            List<Team> teams = new List<Team>();

            teams.Add(boca);
            teams.Add(cerro);

            DateTime date = new DateTime(2018, 09, 13, 16, 45, 0); 

            Encounter expected = new Encounter(football, teams, DateTime.Now);

            IFixtureGenerator fixtureLeague = new LeagueFixture();

            List<Encounter> result = fixtureLeague.GenerateFixture(teams, date);

            Assert.AreEqual(expected, result);
        }

        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }

    }
}
