using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EirinDuran.DataAccessTest
{
    using Helper = HelperFunctions<User>;
    [TestClass]
    public class UserEntityRepositoryTest
    {
        private UserRepository repo;
        private User macri;
        private User alvaro;

        [TestMethod]
        public void AddUserTest()
        {
            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro, macri };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
        public void AddExistingUserTest()
        {
            repo.Add(CreateUserMacri());
        }

        [TestMethod]
        public void RemoveUserTest()
        {
            repo.Delete(macri.Name);

            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void RemoveNonExistingUserTest()
        {
            repo.Delete("Cristina");
        }

        [TestMethod]
        public void GetUserTest()
        {
            User fromRepo = repo.Get(new User("Gato"));

            Assert.AreEqual(macri.Name, fromRepo.Name);
            Assert.AreEqual(macri.Password, fromRepo.Password);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
            Assert.AreEqual(macri.Role, fromRepo.Role);
            Assert.IsTrue(fromRepo.FollowedTeams.Contains(new Team("River")));
            Assert.IsTrue(fromRepo.FollowedTeams.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void GetNonExistantUserTest()
        {
            repo.Get("Cristina");
        }

        [TestMethod]
        public void UpdateUserTest()
        {
            macri = repo.Get("Gato");
            macri.Role = Role.Follower;
            macri.Surname = "Rodriges";
            macri.AddFollowedTeam(new Team("Boca"));

            repo.Update(macri);
            User fromRepo = repo.Get(macri.Name);

            Assert.AreEqual(Role.Follower, fromRepo.Role);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
            Assert.IsTrue(macri.FollowedTeams.Contains(new Team("Boca")));
        }

        [TestMethod]
        public void UpdateNonExistantUserTest()
        {
            User cristina = new User(Role.Follower, "Cristina", "Cristina", "kirchner", "mecaPeron", "cristi123@cri.com");
            repo.Update(cristina);
            User fromRepo = repo.Get("Cristina");
            Assert.AreEqual(cristina.Name, fromRepo.Name);
            Assert.AreEqual(cristina.Password, fromRepo.Password);
            Assert.AreEqual(cristina.Mail, fromRepo.Mail);
            Assert.AreEqual(cristina.Surname, fromRepo.Surname);
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetContextFactory());
            macri = CreateUserMacri();
            alvaro = CreateUserAlvaro();
            repo.Add(CreateUserAlvaro());
            repo.Add(CreateUserMacri());
        }

        private User CreateUserAlvaro()
        {
            User user = new User(Role.Administrator, "alvaro", "Alvaro", "Gomez", "pass1234", "gomez@gomez.uy");
            user.AddFollowedTeam(new Team("Boca"));
            user.AddFollowedTeam(new Team("River"));
            return user;
        }

        private User CreateUserMacri()
        {
            User user = new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
            user.AddFollowedTeam(new Team("River"));
            return user;
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).UseLazyLoadingProxies().Options;
            return new InMemoryContextFactory(options);
        }

        private void CleanUpRepo()
        {
            foreach (User user in repo.GetAll())
                repo.Delete(user.Name);
        }

        private User GetAlvaro() => repo.Get("alvaro");
        private User GetMacri() => repo.Get("Gato");

        private string GetResourcePath(string resourceName)
        {
            string current = Directory.GetCurrentDirectory();
            string resourcesFolder = Directory.EnumerateDirectories(current).First(d => d.EndsWith("Resources"));
            return Directory.EnumerateFiles(resourcesFolder).First(f => f.EndsWith(resourceName));
        }
    }
}
