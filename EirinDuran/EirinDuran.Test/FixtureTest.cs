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
        private Team felix;
        private Team liverpool;
        private Team river;
        private Team cerro;

        [TestInitialize]
        public void SetUp()
        {
            image = GetImage(Resources.River);
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            felix = new Team("Félix", image);
            liverpool = new Team("Liverpool", image);
            river = new Team("River Plate", image);
            cerro = new Team("Cerro", image);

        }

        [TestMethod]
        public void SampleAutoFixtureLeagueTest()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);

            List<Team> teams = new List<Team>(){ felix, liverpool };

            DateTime start = new DateTime(2018, 10, 07);
            DateTime end = new DateTime(2018, 10, 17);
      
            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start, end).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void OddNumberOfTeamsInFixture()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);
            football.AddTeam(river);

            List<Team> teams = new List<Team>() { felix, liverpool, river };

            DateTime start = new DateTime(2018, 10, 07);
            DateTime end = new DateTime(2018, 10, 17);

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start, end).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(OutdatedDatesException))]
        public void CreateFeatureWithOutdatedDates()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);

            List<Team> teams = new List<Team>() { felix, liverpool };

            DateTime start = new DateTime(2018, 10, 07);
            DateTime end = new DateTime(2018, 09, 17);

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start, end).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRangeOfDatesToGenerateFeatureException))]
        public void InsufficientRangeOfDatesToGenerateFeature()
        {

        }


        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }

    }
}
