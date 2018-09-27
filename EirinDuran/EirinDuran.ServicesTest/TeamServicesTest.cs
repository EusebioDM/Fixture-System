using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class TeamServicesTest
    {
        private ILoginServices login;
        private IRepository<Team> teamRepository;


       [TestMethod]
       public void AddTeamOk()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository);
            services.AddTeam(boca);
            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(1, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamTryToAddAlreadyExistsException))]
        public void AddSameTeam()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository);

            services.AddTeam(boca);
            services.AddTeam(boca);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void AddTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(new User(Role.Follower, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository);
            services.AddTeam(boca);
        }

        [TestMethod]
        public void DeleteTeamOk()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository);
            services.AddTeam(boca);
            services.DeleteTeam("Boca");

            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(0, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamTryToDeleteDoesNotExistsException))]
        public void DeleteTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void DeleteTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(new User(Role.Follower, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));
            TeamServices services = new TeamServices(login, teamRepository);
            Team boca = new Team("Boca");
            services.AddTeam(boca);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        public void GetTeamOk()
        {
            TeamServices services = new TeamServices(login, teamRepository);
            Team boca = new Team("Boca");
            teamRepository.Add(boca);

            Team recovered = services.GetTeam("Boca");

            Assert.AreEqual(boca, recovered);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamTryToRecoverDoesNotExistException))]
        public void GetTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository);
            Team recovered = services.GetTeam("Boca");
        }

        [TestMethod]
        public void GetAllTeams()
        {
            TeamServices services = new TeamServices(login, teamRepository);
            Team boca = new Team("Boca");
            Team river = new Team("River");

            teamRepository.Add(boca);
            teamRepository.Add(river);

            IEnumerable<Team> recovered = services.GetAll();
            Assert.AreEqual(2, recovered.ToList().Count);

        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestInitialize]
        public void TestInit()
        {
            teamRepository = new TeamRepository(GetContextFactory());
            login = CreateLoginServices();
        }
    }
}
