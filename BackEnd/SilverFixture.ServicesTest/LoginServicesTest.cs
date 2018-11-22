using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilverFixture.Services;
using SilverFixture.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using SilverFixture.DataAccessTest;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using SilverFixture.Domain.Fixture;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Infrastructure_Interfaces;
using SilverFixture.IServices.Services_Interfaces;

namespace SilverFixture.ServicesTest
{
    [TestClass]
    public class LoginServicesTest
    {
        private IRepository<User> userRepo;
        private IRepository<Team> teamRepo;
        private ILogger logger;

        [TestInitialize]
        public void TestInit()
        {
            userRepo = new UserRepository(GetContextFactory());
            teamRepo = new TeamRepository(GetContextFactory());
            userRepo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestMethod]
        public void SimpleLoginOk()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            login.CreateSession("sSanchez", "user");
            Assert.AreEqual("sSanchez", login.LoggedUser.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
        public void UserTryToLogginDoesNotExists()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            login.CreateSession("pAntonio", "user");
        }

        [TestMethod]
        [ExpectedException(typeof(SilverFixture.IServices.Exceptions.InvalidaDataException))]
        public void TryToLoginUserWithIncorrectPassword()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            login.CreateSession("sSanchez", "pass");
        }
    }
}
