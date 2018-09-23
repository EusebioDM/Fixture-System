using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class UserServicesTest
    {
        private UserRepository repo;
        private UserDTO pepe;

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetContextFactory());
            repo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            repo.Add(new User(Role.Follower, "martinFowler", "Martín", "Fowler", "user", "fowler@fowler.com"));
            pepe = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Pepe",
                Surname = "Ávila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeams = new List<TeamDTO>()
            };
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
            services.CreateUser(pepe);
        }

        [TestMethod]
        public void AddUserSimpleOk()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("sSanchez", "user");
            services.CreateUser(pepe);

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
            services.CreateUser(pepe);

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
            UserDTO user = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Angel",
                Surname = "Ávila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeams = new List<TeamDTO>()
            };
            services.Modify(user);

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
            UserDTO user = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Angel",
                Surname = "Ávila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeams = new List<TeamDTO>()
            };
            services.Modify(user);

            User result = repo.Get(new User("pepeAvila"));

            Assert.AreEqual("ANGEL", result.Name);
        }

        [TestMethod]
        public void FollowTeam()
        {
            LoginServices login = new LoginServices(repo);
            UserServices services = new UserServices(repo, login);

            login.CreateSession("martinFowler", "user");

            TeamDTO cavaliers = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = Image.FromFile(GetResourcePath("Cavaliers.jpg"))
            };
            SportDTO basketball = new SportDTO()
            {
                Name = "Baskteball"
            };

            basketball.Teams = new List<TeamDTO>() { cavaliers };

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

            TeamDTO cavaliers = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = Image.FromFile(GetResourcePath("Cavaliers.jpg"))
            };
            SportDTO basketball = new SportDTO()
            {
                Name = "Baskteball"
            };

            basketball.Teams = new List<TeamDTO>() { cavaliers };

            services.AddFollowedTeam(cavaliers);

            List<Team> followedTeams = services.GetAllFollowedTeams().Select(t => new Team(t.Name)).ToList();
            Assert.AreEqual(cavaliers.Name, followedTeams[0].Name);
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}