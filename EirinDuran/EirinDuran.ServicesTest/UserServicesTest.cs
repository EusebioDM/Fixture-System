using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
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
        private IRepository<User> userRepo;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private IRepository<Encounter> encounterRepo;
        private UserDTO pepe;
        private UserDTO pablo;
        private Sport futbol;
        private Sport baskteball;

        [TestInitialize]
        public void TestInit()
        {

            userRepo = new UserRepository(GetContextFactory());
            sportRepo = new SportRepository(GetContextFactory());
            teamRepo = new TeamRepository(GetContextFactory());
            encounterRepo = new EncounterRepository(GetContextFactory());
            userRepo.Add(new User(Role.Administrator, "sSanchez", "Santiago", "Sanchez", "user", "sanchez@outlook.com"));
            userRepo.Add(new User(Role.Follower, "martinFowler", "Martin", "Fowler", "user", "fowler@fowler.com"));
            pepe = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Pepe",
                Surname = "Avila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeamsNames = new List<string>()
            };

            pablo = new UserDTO()
            {
                UserName = "pablo",
                Name = "pablo",
                Surname = "pablo",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = false,
                FollowedTeamsNames = new List<string>()
            };
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void AddUserWithoutPermissions()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("martinFowler", "user");
            services.CreateUser(pepe);
        }

        [TestMethod]
        public void AddUserSimpleOk()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("sSanchez", "user");
            services.CreateUser(pepe);

            User result = userRepo.Get("pepeAvila");

            Assert.AreEqual("pepeAvila", result.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void AddInvalidUserNameTest()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);
            login.CreateSession("sSanchez", "user");
            UserDTO user = new UserDTO()
            {
                UserName = ""
            };
            services.CreateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidaDataException))]
        public void AddInvalidMailTest()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);
            login.CreateSession("sSanchez", "user");
            UserDTO user = new UserDTO()
            {
                UserName = "Holanda",
                Mail = "esto NO es un mail"
            };
            services.CreateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void DeleteUserSimpleOk()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("sSanchez", "user");
            services.CreateUser(pepe);

            services.DeleteUser("pepeAvila");
            User result = userRepo.Get("pepeAvila");
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void DeleteUserWithoutSufficientPermissions()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("martinFowler", "user");
            userRepo.Add(new User(Role.Administrator, "pepeAvila", "Pepe", "Avila", "user", "pepeavila@mymail.com"));

            services.DeleteUser("pepeAvila");
            User result = userRepo.Get("pepeAvila");
        }

        [TestMethod]
        public void ModifyUserOk()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("sSanchez", "user");
            UserDTO user = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Angel",
                Surname = "Avila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeamsNames = new List<string>()
            };
            services.ModifyUser(user);

            User result = userRepo.Get("pepeAvila");

            Assert.AreEqual("ANGEL", result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientPermissionException))]
        public void ModifyUserwithoutSufficientPermissions()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            login.CreateSession("martinFowler", "user");
            UserDTO user = new UserDTO()
            {
                UserName = "pepeAvila",
                Name = "Angel",
                Surname = "Avila",
                Password = "user",
                Mail = "pepeavila@mymail.com",
                IsAdmin = true,
                FollowedTeamsNames = new List<string>()
            };
            services.ModifyUser(user);

            User result = userRepo.Get("pepeAvila");

            Assert.AreEqual("ANGEL", result.Name);
        }

        [TestMethod]
        public void FollowTeam()
        {
            ILoginServices login = new LoginServicesMock(pepe);

            ITeamServices teamServices = new TeamServices(login, teamRepo, sportRepo);

            TeamDTO team = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = EncondeImage(Image.FromFile(GetResourcePath("Cavaliers.jpg"))),
                SportName = "Baskteball"
            };
            teamServices.CreateTeam(team);

            login = new LoginServicesMock(pablo);
            IUserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);

            TeamDTO cavaliers = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = EncondeImage(Image.FromFile(GetResourcePath("Cavaliers.jpg"))),
                SportName = "Baskteball"
            };

            SportDTO basketball = new SportDTO()
            {
                Name = "Baskteball"
            };
            userRepo.Add(new User(Role.Follower, "pablo", "pablo", "pablo", "user", "pepeavila@mymail.com"));
            services.AddFollowedTeam("Cavaliers");

            User recovered = userRepo.Get("pablo");
            List<Team> followedTeams = recovered.FollowedTeams.ToList();
            Assert.IsTrue(followedTeams[0].Name == "Cavaliers");
        }

        [TestMethod]
        public void RecoverAllFollowedTeams()
        {
            LoginServices login = new LoginServices(userRepo, teamRepo);
            UserServices services = new UserServices(login, userRepo, teamRepo, sportRepo);
            ITeamServices teamServices = new TeamServices(login, teamRepo, sportRepo);

            login.CreateSession("sSanchez", "user");

            TeamDTO cavaliers = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = EncondeImage(Image.FromFile(GetResourcePath("Cavaliers.jpg"))),
                SportName = "Baskteball"
            };
            SportDTO basketball = new SportDTO()
            {
                Name = "Baskteball"
            };

            TeamDTO team = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = EncondeImage(Image.FromFile(GetResourcePath("Cavaliers.jpg"))),
                SportName = "Baskteball"
            };
            teamServices.CreateTeam(team);
            services.AddFollowedTeam("Cavaliers");

            IEnumerable<TeamDTO> followedTeams = services.GetAllFollowedTeams();
            Assert.AreEqual(cavaliers.Name, followedTeams.First().Name);
        }

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }

        private string EncondeImage(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = stream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }
}