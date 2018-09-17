using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;
using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class LoginServiceTest
    {
        private UserRepository repo;

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetContextFactory());
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new InMemoryContextFactory(options);
        }

        [TestMethod]
        public void SampleLoginOk()
        {
            LoginServices login = new LoginServices();
            login.CreateSession("sSanchez", "user");
            Assert.AreEqual("sSanchez", login.LoggedUser.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(UserTryToLogginDoesNotExistsException))]
        public void UserTryToLogginDoesNotExists()
        {
            LoginServices login = new LoginServices();
            login.CreateSession("pAntonio", "user");
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public void TryToLogginUserWithIncorrectPassword()
        {
            LoginServices login = new LoginServices();
            login.CreateSession("juanAndres", "pass");
        }
    }
}
