using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
using EirinDuran.DataAccessTest;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class UserServicesTest
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
        public void AdminAddUserOk()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            services.AddUser(new User(Role.Follower, "UserFollower", "User", "User", "user", "user@mail.com"));
            Assert.AreEqual("UserFollower", repo.Get(new User("UserFollower")).UserName);
        }
    }
}