using EirinDuran.DataAccess;
using EirinDuran.DataAccessTest;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.Services;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using EirinDuran.IServices.Infrastructure_Interfaces;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class UserServicesTest
    {
        private IRepository<User> userRepo;
        private IRepository<Sport> sportRepo;
        private IRepository<Team> teamRepo;
        private ILogger logger;
        private IRepository<Encounter> encounterRepo;
        private UserDTO pepe;
        private UserDTO pablo;

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

            Assert.AreEqual("pepeAvila", (string)result.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(ServicesException))]
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
        [ExpectedException(typeof(ServicesException))]
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
            services.CreateUser(user);
            services.ModifyUser(user);

            User result = userRepo.Get("pepeAvila");

            Assert.AreEqual("Angel", (string)result.Name);
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
            ILoginServices loginServices = new LoginServicesMock(pepe);
            ITeamServices teamServices = new TeamServices(loginServices, teamRepo, sportRepo, userRepo);
            ISportServices sportServices = new SportServices(loginServices, sportRepo);

            SportDTO basketball = new SportDTO()
            {
                Name = "Basketball"
            };

            sportServices.CreateSport(basketball);

            TeamDTO cavaliers = new TeamDTO()
            {
                Name = "Cavaliers",
                Logo = EncondeImage(Image.FromFile(GetResourcePath("Cavaliers.jpg"))),
                SportName = "Basketball"
            };
            teamServices.CreateTeam(cavaliers);
            userRepo.Add(new User()
            {
                UserName = pepe.UserName,
                Name = pepe.Name,
                Surname = pepe.Surname,
                Mail = pepe.Mail,
                Password = pepe.Password,
                Role = pepe.IsAdmin ? Role.Administrator : Role.Follower
            });
            userRepo.Add(new User()
            {
                UserName = pablo.UserName,
                Name = pablo.Name,
                Surname = pablo.Surname,
                Mail = pablo.Mail,
                Password = pablo.Password,
                Role = pablo.IsAdmin ? Role.Administrator : Role.Follower
            });
            loginServices = new LoginServicesMock(pablo);
            teamServices.AddFollowedTeam("Cavaliers_Basketball");

            User recovered = userRepo.Get(pepe.UserName);
            List<Team> followedTeams = recovered.FollowedTeams.ToList();
            Assert.IsTrue(followedTeams.First().Name == "Cavaliers");
        }

        [TestMethod]
        public void RecoverAllFollowedTeams()
        {
            ILoginServices loginServices = new LoginServices(userRepo, teamRepo);
            IUserServices userServices = new UserServices(loginServices, userRepo, teamRepo, sportRepo);
            ITeamServices teamServices = new TeamServices(loginServices, teamRepo, sportRepo, userRepo);
            ISportServices sportServices = new SportServices(loginServices, sportRepo);

            loginServices.CreateSession("sSanchez", "user");

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

            sportServices.CreateSport(basketball);
            teamServices.CreateTeam(team);
            teamServices.AddFollowedTeam("Cavaliers_Baskteball");

            IEnumerable<TeamDTO> followedTeams = userServices.GetFollowedTeams();
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