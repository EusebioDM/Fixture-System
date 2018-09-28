using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class LoginServicesTest
    {
        private IRepository<User> userRepo;
        private IRepository<Team> teamRepo;

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
        [ExpectedException(typeof(IServices.Exceptions.InvalidaDataException))]
        public void UserTryToLogginDoesNotExists()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            login.CreateSession("pAntonio", "user");
        }

        [TestMethod]
        [ExpectedException(typeof(IServices.Exceptions.InvalidaDataException))]
        public void TryToLoginUserWithIncorrectPassword()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            login.CreateSession("sSanchez", "pass");
        }
    }
}
