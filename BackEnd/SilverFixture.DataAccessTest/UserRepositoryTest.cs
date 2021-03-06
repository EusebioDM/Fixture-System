using SilverFixture.DataAccess;
using SilverFixture.Domain.Fixture;
using SilverFixture.Domain.User;
using SilverFixture.IDataAccess;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SilverFixture.DataAccessTest
{
    using Helper = HelperFunctions<User>;
    [TestClass]
    public class UserEntityRepositoryTest
    {
        private UserRepository repo;
        private User macri;
        private User alvaro;
        private Sport football;

        [TestMethod]
        public void AddUserTest()
        {
            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro, macri };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void AddExistingUserTest()
        {
            repo.Add(CreateUserMacri());
        }

        [TestMethod]
        public void RemoveUserTest()
        {
            repo.Delete(macri.UserName);

            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void RemoveNonExistingUserTest()
        {
            repo.Delete("Cristina");
        }

        [TestMethod]
        public void GetUserTest()
        {
            User fromRepo = repo.Get("Gato");

            Assert.AreEqual(macri.Name, fromRepo.Name);
            Assert.AreEqual(macri.Password, fromRepo.Password);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
            Assert.AreEqual(macri.Role, fromRepo.Role);
            Assert.IsTrue(fromRepo.FollowedTeams.Contains(new Team("River", football)));
            Assert.IsTrue(fromRepo.FollowedTeams.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
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
            macri.AddFollowedTeam(new Team("Boca", football));

            repo.Update(macri);
            User fromRepo = repo.Get(macri.UserName);

            Assert.AreEqual(Role.Follower, fromRepo.Role);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
            Assert.IsTrue(macri.FollowedTeams.Contains(new Team("Boca", football)));
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

        [TestMethod]
        public void UpdateMultipleFollowedTeamsTest()
        {
            User alvaroFromRepo = repo.Get(alvaro.UserName);
            User macriFromRepo = repo.Get(macri.UserName);

            Assert.IsTrue(alvaroFromRepo.FollowedTeams.Contains(new Team("Boca", football)));
            Assert.IsTrue(alvaroFromRepo.FollowedTeams.Contains(new Team("River", football)));
            Assert.AreEqual(2, alvaroFromRepo.FollowedTeams.Count());
            Assert.IsTrue(macriFromRepo.FollowedTeams.Contains(new Team("River", football)));
            Assert.AreEqual(1, macriFromRepo.FollowedTeams.Count());
        }

        [TestMethod]
        public void RemoveMultipleFollowedTeamsTest()
        {
            alvaro.RemoveFollowedTeam(new Team("River", football));
            repo.Update(alvaro);

            User alvaroFromRepo = repo.Get(alvaro.UserName);
            User macriFromRepo = repo.Get(macri.UserName);

            Assert.IsTrue(alvaroFromRepo.FollowedTeams.Contains(new Team("Boca", football)));
            Assert.IsFalse(alvaroFromRepo.FollowedTeams.Contains(new Team("River", football)));
            Assert.AreEqual(1, alvaroFromRepo.FollowedTeams.Count());
            Assert.IsTrue(macriFromRepo.FollowedTeams.Contains(new Team("River", football)));
            Assert.AreEqual(1, macriFromRepo.FollowedTeams.Count());
        }

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetContextFactory());
            football = new Sport("Futbol");
            macri = CreateUserMacri();
            alvaro = CreateUserAlvaro();
            repo.Add(CreateUserAlvaro());
            repo.Add(CreateUserMacri());
        }

        private User CreateUserAlvaro()
        {
            User user = new User(Role.Administrator, "alvaro", "Alvaro", "Gomez", "pass1234", "gomez@gomez.uy");
            user.AddFollowedTeam(new Team("Boca", football));
            user.AddFollowedTeam(new Team("River", football));
            return user;
        }

        private User CreateUserMacri()
        {
            User user = new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
            user.AddFollowedTeam(new Team("River", football));
            return user;
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            return new InMemoryContextFactory();
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
