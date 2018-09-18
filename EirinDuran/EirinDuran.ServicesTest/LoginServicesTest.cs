using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.User;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class LoginServicesTest
    {
        private UserRepository repo;

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetContextFactory());
            repo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new InMemoryContextFactory(options);
        }

        [TestMethod]
        public void SimpleLoginOk()
        {
            LoginServices login = new LoginServices(repo);
            login.CreateSession("sSanchez", "user");
            Assert.AreEqual("sSanchez", login.LoggedUser.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(UserTryToLoginDoesNotExistsException))]
        public void UserTryToLogginDoesNotExists()
        {
            LoginServices login = new LoginServices(repo);
            login.CreateSession("pAntonio", "user");
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public void TryToLoginUserWithIncorrectPassword()
        {
            LoginServices login = new LoginServices(repo);
            login.CreateSession("sSanchez", "pass");
        }
    }
}
