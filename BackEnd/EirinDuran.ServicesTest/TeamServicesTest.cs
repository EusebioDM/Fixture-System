using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilverFixture.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using EirinDuran.DataAccessTest;
using SilverFixture.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using System.Linq;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.DTOs;
using EirinDuran.Domain.User;
using SilverFixture.IServices.Services_Interfaces;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class TeamServicesTest
    {
        private ILoginServices login;
        private IRepository<User> userRepository;
        private IRepository<Sport> sportRepository;
        private IRepository<Team> teamRepository;
        private IRepository<Encounter> encounterRepository;
        private UserDTO macri;
        private UserDTO christina;

        [TestMethod]
        public void AddTeamOk()
        {
            TeamDTO boca = new TeamDTO
            {
                Name = "Boca",
                SportName = "Futbol"
            };
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            services.CreateTeam(boca);
            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(1, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void AddSameTeam()
        {
            TeamDTO boca = new TeamDTO()
            {
                Name = "Boca",
                SportName = "Futbol"
            };
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);

            services.CreateTeam(boca);
            services.CreateTeam(boca);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void AddTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(christina);

            TeamDTO boca = new TeamDTO()
            {
                Name = "Boca",
                SportName = "Futbol"
            };
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            services.CreateTeam(boca);
        }

        [TestMethod]
        public void DeleteTeamOk()
        {
            TeamDTO boca = new TeamDTO() { Name = "Boca", SportName = "Futbol" };

            SportServices sportServices = new SportServices(login, sportRepository);
            TeamServices teamServices = new TeamServices(login, teamRepository, sportRepository, userRepository);

            teamServices.CreateTeam(boca);

            teamServices.DeleteTeam("Boca_Futbol");

            IEnumerable<Team> recovered = teamRepository.GetAll();
            Assert.AreEqual(0, recovered.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void DeleteTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void DeleteTeamWithoutSufficientPermission()
        {
            login = new LoginServicesMock(christina);
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            TeamDTO boca = new TeamDTO() { Name = "Boca" , SportName = "Futbol" };
            services.CreateTeam(boca);
            services.DeleteTeam("Boca");
        }

        [TestMethod]
        public void GetTeamOk()
        {
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            Team boca = new Team("Boca", new Sport("Futbol"));
            teamRepository.Add(boca);

            TeamDTO recovered = services.GetTeam("Boca_Futbol");

            Assert.AreEqual((string)boca.Name, recovered.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void GetTeamDoesNotExists()
        {
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            TeamDTO recovered = services.GetTeam("Boca");
        }

        [TestMethod]
        public void GetAllTeams()
        {
            TeamServices services = new TeamServices(login, teamRepository, sportRepository, userRepository);
            Team boca = new Team("Boca", new Sport("Futbol"));
            Team river = new Team("River", new Sport("Futbol"));

            teamRepository.Add(boca);
            teamRepository.Add(river);

            IEnumerable<TeamDTO> recovered = services.GetAllTeams();
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
            sportRepository = new SportRepository(GetContextFactory());
            encounterRepository = new EncounterRepository(GetContextFactory());
            userRepository = new UserRepository(GetContextFactory());
            sportRepository.Add(new Sport("Futbol"));

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
