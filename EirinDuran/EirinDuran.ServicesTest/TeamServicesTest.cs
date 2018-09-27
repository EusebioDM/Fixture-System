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
            Assert.IsTrue(recovered.ToList().Count == 1);
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
