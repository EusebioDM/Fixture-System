using EirinDuran.Domain.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.FixtureGenerators.AllOnce;
using EirinDuran.FixtureGenerators.RoundRobin;

namespace EirinDuran.DomainTest
{
    [TestClass]
    public class FixtureTest
    {
        private Sport futbol;
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
            InitializeTeams();
        }

        private void InitializeTeams()
        {
            futbol = new Sport("Futbol");
            felix = new Team("Felix", futbol);
            liverpool = new Team("Liverpool", futbol);
            river = new Team("River Plate", futbol);
            cerro = new Team("Cerro", futbol);
            torque = new Team("Torque", futbol);
            danubio = new Team("Danubio", futbol);
            penhiarol = new Team("Peñarol", futbol);
            atenas = new Team("Atenas", futbol);
            wanderers = new Team("Wanderes", futbol);
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureWithEmptyTeams()
        {
            Sport football = new Sport("football");
            List<Team> teams = new List<Team>() { };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobin();

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureTest1()
        {
            List<Team> teams = new List<Team>() { felix, liverpool };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobin();

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureTest2()
        {
            List<Team> teams = new List<Team>() { felix, liverpool, river, cerro, penhiarol, torque };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobin();

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(15, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ThereAreRepeatedTeamsException))]
        public void SampleAutoRoundRobinFixtureWithTheSameTeamTest()
        {
            List<Team> teams = new List<Team>() { felix, liverpool, river, cerro, penhiarol, torque, cerro };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobin();

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void SampleAutoAllOnceFixtureImparNumberOfTeamsTest()
        {
            List<Team> teams = new List<Team>() { felix, liverpool, river };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator allOnceFixture = new AllOnce();

            List<Encounter> result = allOnceFixture.GenerateFixture(teams, start).ToList();
        }

        [TestMethod]
        public void SampleAutoAllOnceFixtureTest()
        {
            List<Team> teams = new List<Team>() { felix, liverpool, river, penhiarol };

            DateTime start = new DateTime(3018, 10, 07);

            IFixtureGenerator allOnceFixture = new AllOnce();

            List<Encounter> result = allOnceFixture.GenerateFixture(teams, start).ToList();
            Assert.AreEqual(2, result.Count);
        }
    }
}
