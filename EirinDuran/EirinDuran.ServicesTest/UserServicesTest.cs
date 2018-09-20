using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
using EirinDuran.DataAccessTest;
using EirinDuran.Services;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.Fixture;
using System.Collections.Generic;
using System.Linq;

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
            repo.Add(new User(Role.Follower, "martinFowler", "Martín", "Fowler", "user", "fowler@fowler.com"));
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void AddUserWithoutPermissions()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");
            services.AddUser(new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com"));
        }

        [TestMethod]
        public void AddUserSimpleOk()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("sSanchez", "user");
            services.AddUser(new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com"));

            User result = repo.Get(new User("pepeAvila"));

            Assert.AreEqual("pepeAvila", result.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void DeleteUserSimpleOk()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("sSanchez", "user");
            services.AddUser(new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com"));

            services.DeleteUser("pepeAvila");
            User result = repo.Get(new User("pepeAvila"));
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void DeleteUserWithoutSufficientPermissions()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");
            repo.Add(new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com"));

            services.DeleteUser("pepeAvila");
            User result = repo.Get(new User("pepeAvila"));
        }

        [TestMethod]
        public void ModifyUserOk()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("sSanchez", "user");
            services.Modify(new User(Role.Administrator, "pepeAvila", "Angel", "Ávila", "user", "pepeavila@mymail.com"));

            User result = repo.Get(new User("pepeAvila"));

            Assert.AreEqual("ANGEL", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionToPerformThisActionException))]
        public void ModifyUserwithoutSufficientPermissions()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");
            services.Modify(new User(Role.Administrator, "pepeAvila", "Angel", "Ávila", "user", "pepeavila@mymail.com"));

            User result = repo.Get(new User("pepeAvila"));

            Assert.AreEqual("ANGEL", result.Name);
        }

        [TestMethod]
        public void FollowTeam()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");

            Team cavaliers = new Team("Cavaliers");
            Sport basketball = new Sport("Baskteball");

            basketball.AddTeam(cavaliers);

            services.AddFollowedTeam(cavaliers);

            User recovered = repo.Get(new User("martinFowler"));
            List<Team> followedTeams = recovered.FollowedTeams.ToList();
            Assert.IsTrue(followedTeams[0].Name == "Cavaliers");
        }

        [TestMethod]
        public void RecoverAllFollowedTeams()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");

            Team cavaliers = new Team("Cavaliers");
            Sport basketball = new Sport("Baskteball");

            basketball.AddTeam(cavaliers);

            services.AddFollowedTeam(cavaliers);

            List<Team> followedTeams = services.GetAllFollowedTeams().ToList();
            Assert.AreEqual(cavaliers, followedTeams[0]);
        }

    }
}