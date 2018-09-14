using EirinDuran.Domain.Fixture;
using EirinDuran.Test.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EirinDuran.Test
{
    [TestClass]
    public class FixtureTest
    {
        private Image image;
        private Team boca;
        private Team cerro;
        private Team river;

        [TestInitialize]
        public void SetUp()
        {
            image = GetImage(Resources.River);
            boca = new Team("boca", image);
            cerro = new Team("cerro", image);
            river = new Team("River Plate", image);
        }

        [TestMethod]
        public void SampleAutoFixtureLeagueTest()
        {
            Sport football = new Sport("football");

            football.AddTeam(boca);
            football.AddTeam(cerro);

            List<Team> teams = new List<Team>();

            teams.Add(boca);
            teams.Add(cerro);

            DateTime date = new DateTime(2018, 10, 07, 18, 30, 00);
            /*
            Encounter expected = new Encounter(football, teams, date);

            List<Encounter> expectResult = new List<Encounter>();
            expectResult.Add(expected); */

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, date).ToList();

            Assert.AreEqual(1, result.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void OddNumberOfTeamsInFixture()
        {
            Sport football = new Sport("football");

            football.AddTeam(boca);
            football.AddTeam(cerro);
            football.AddTeam(river);

            List<Team> teams = new List<Team>();

            teams.Add(boca);
            teams.Add(cerro);
            teams.Add(river);

            DateTime date = new DateTime(2018, 10, 07, 18, 30, 00);

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, date).ToList();
        }




        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }

    }
}
