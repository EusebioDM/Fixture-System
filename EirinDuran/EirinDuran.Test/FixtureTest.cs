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
        private Team torque;
        private Team danubio;
        private Team penhiarol;
        private Team atenas;
        private Team wanderers;

        [TestInitialize]
        public void SetUp()
        {
            image = GetImage(Resources.River);
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            felix = new Team("Felix", image);
            liverpool = new Team("Liverpool", image);
            river = new Team("River Plate", image);
            cerro = new Team("Cerro", image);
            torque = new Team("Torque", image);
            danubio = new Team("Danubio", image);
            penhiarol = new Team("Peñarol", image);
            atenas = new Team("Atenas", image);
            wanderers = new Team("Wanderes", image);
        }

        [TestMethod]
        public void SampleAutoFixtureLeagueTest1()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);

            List<Team> teams = new List<Team>() { felix, liverpool };

            DateTime start = new DateTime(2018, 10, 07);
            
            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SampleAutoFixtureLeagueTest2()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);
            football.AddTeam(river);
            football.AddTeam(cerro);
            football.AddTeam(penhiarol);
            football.AddTeam(torque);

            List<Team> teams = new List<Team>() { felix, liverpool, river, cerro, penhiarol, torque };

            DateTime start = new DateTime(2018, 10, 07);
            DateTime end = new DateTime(2018, 10, 17);

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(15, result.Count);
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

            IFixtureGenerator leagueFixture = new LeagueFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();
        }

        private Image GetImage(byte[] resource)
        {
            return new Bitmap(new MemoryStream(resource));
        }

    }
}
