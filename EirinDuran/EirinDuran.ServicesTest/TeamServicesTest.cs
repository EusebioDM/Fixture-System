using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using EirinDuran.DataAccessTest;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.IServices.DTOs;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class TeamServicesTest
    {
        private ILoginServices login;
        private IRepository<Sport> sportRepository;
        private IRepository<Team> teamRepository;
        private IRepository<Encounter> encounterRepository;
        private UserDTO macri;
        private UserDTO christina;


       [TestMethod]
       public void AddTeamOk()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            services.AddTeam(boca);
            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(1, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void AddSameTeam()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);

            services.AddTeam(boca);
            services.AddTeam(boca);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void AddTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(christina);

            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            services.AddTeam(boca);
        }

        [TestMethod]
        public void DeleteTeamOk()
        {
            Team boca = new Team("Boca");
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            services.AddTeam(boca);
            services.DeleteTeam("Boca");

            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(0, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void DeleteTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void DeleteTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(christina);
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            Team boca = new Team("Boca");
            services.AddTeam(boca);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        public void GetTeamOk()
        {
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            Team boca = new Team("Boca");
            teamRepository.Add(boca);

            TeamDTO recovered = services.GetTeam("Boca");

            Assert.AreEqual(boca.Name, recovered.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void GetTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            TeamDTO recovered = services.GetTeam("Boca");
        }

        [TestMethod]
        public void GetAllTeams()
        {
            TeamServices services = new TeamServices(login, teamRepository, encounterRepository, sportRepository);
            Team boca = new Team("Boca");
            Team river = new Team("River");

            teamRepository.Add(boca);
            teamRepository.Add(river);

            IEnumerable<TeamDTO> recovered = services.GetAll();
            Assert.AreEqual(2, recovered.ToList().Count);

        }

        private ILoginServices CreateLoginServices()
        {
            return new LoginServicesMock(macri);
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestInitialize]
        public void TestInit()
        {
            sportRepository = new SportRepository(GetContextFactory());
            teamRepository = new TeamRepository(GetContextFactory());
            encounterRepository = new EncounterRepository(GetContextFactory());

            macri = new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            };

            christina = new UserDTO()
            {
                UserName = "Christina",
                Name = "christina",
                Surname = "christina",
                Password = "christina",
                Mail = "mail@gmail.com",
                IsAdmin = false
            };

            login = CreateLoginServices();
        }
    }
}
