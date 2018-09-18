using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class EncounterServicesTest
    {
        private LoginServices login;
        private UserRepository userRepository;
        private EncounterRepository encounterRepository;

        [TestInitialize]
        public void TestInit()
        {
            userRepository = new UserRepository(GetContextFactory());
            encounterRepository = new EncounterRepository(GetContextFactory());
            userRepository.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            login = new LoginServices(userRepository);
            login.CreateSession("sSanchez", "user");
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new InMemoryContextFactory(options);
        }

        [TestMethod]
        public void CreateSimpleEncounter()
        {
            EncounterServices encounterServices = new EncounterServices(encounterRepository, login);
            IEnumerable<Team> teams = new List<Team> { new Team("Chelsea"), new Team("Sevilla") };
            DateTime date = new DateTime();

            encounterServices.CreateEncounter();

        }
    }
}