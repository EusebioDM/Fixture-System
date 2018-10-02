using EirinDuran.Domain.Fixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.DomainTest
{
    [TestClass]
    public class FixtureTest
    {
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
            felix = new Team("Felix");
            liverpool = new Team("Liverpool");
            river = new Team("River Plate");
            cerro = new Team("Cerro");
            torque = new Team("Torque");
            danubio = new Team("Danubio");
            penhiarol = new Team("Peñarol");
            atenas = new Team("Atenas");
            wanderers = new Team("Wanderes");
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureWithEmptyTeams()
        {
            Sport football = new Sport("football");
            List<Team> teams = new List<Team>() {  };

            DateTime start = new DateTime(2018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobinFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureTest1()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);

            List<Team> teams = new List<Team>() { felix, liverpool };

            DateTime start = new DateTime(2018, 10, 07);
            
            IFixtureGenerator leagueFixture = new RoundRobinFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SampleAutoRoundRobinFixtureTest2()
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

            IFixtureGenerator leagueFixture = new RoundRobinFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();

            Assert.AreEqual(15, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ThereAreRepeatedTeamsException))]
        public void SampleAutoRoundRobinFixtureWithTheSameTeamTest()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);
            football.AddTeam(river);
            football.AddTeam(cerro);
            football.AddTeam(penhiarol);
            football.AddTeam(torque);

            List<Team> teams = new List<Team>() { felix, liverpool, river, cerro, penhiarol, torque, cerro };

            DateTime start = new DateTime(2018, 10, 07);

            IFixtureGenerator leagueFixture = new RoundRobinFixture(football);

            List<Encounter> result = leagueFixture.GenerateFixture(teams, start).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNumberOfTeamsException))]
        public void SampleAutoAllOnceFixtureImparNumberOfTeamsTest()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);
            football.AddTeam(river);

            List<Team> teams = new List<Team>() { felix, liverpool, river };

            DateTime start = new DateTime(2018, 10, 07);

            IFixtureGenerator allOnceFixture = new AllOnceFixture(football);

            List<Encounter> result = allOnceFixture.GenerateFixture(teams, start).ToList();
        }

        [TestMethod]
        public void SampleAutoAllOnceFixtureTest()
        {
            Sport football = new Sport("football");

            football.AddTeam(felix);
            football.AddTeam(liverpool);
            football.AddTeam(river);
            football.AddTeam(penhiarol);

            List<Team> teams = new List<Team>() { felix, liverpool, river, penhiarol };

            DateTime start = new DateTime(2018, 10, 07);

            IFixtureGenerator allOnceFixture = new AllOnceFixture(football);

            List<Encounter> result = allOnceFixture.GenerateFixture(teams, start).ToList();
            Assert.AreEqual(2, result.Count);
        }
    }
}
