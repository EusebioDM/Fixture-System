using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            repo.Delete(CreateUserMacri());

            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void RemoveNonExistingUserTest()
        {
            repo.Delete(new User("Cristina"));
        }

        [TestMethod]
        public void GetUserTest()
        {
            User fromRepo = repo.Get(new User( "Gato"));

            Assert.AreEqual(macri.Name, fromRepo.Name);
            Assert.AreEqual(macri.Password, fromRepo.Password);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
            Assert.AreEqual(macri.Role, fromRepo.Role);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void GetNonExistantUserTest()
        {
            repo.Get(new User("Cristina"));
        }

        [TestMethod]
        public void UpdateUserTest()
        {
            macri = repo.Get(new User("Gato"));
            macri.Role = Role.Follower;
            macri.Surname = "Rodriges";

            repo.Update(macri);
            User fromRepo = repo.Get(macri);

            Assert.AreEqual(Role.Follower, fromRepo.Role);
            Assert.AreEqual(macri.Surname, fromRepo.Surname);
        }

        [TestMethod]
        public void UpdateNonExistantUserTest()
        {
            User cristina = new User(Role.Follower, "Cristina", "Cristina", "kirchner", "mecaPeron", "cristi123@cri.com");
            repo.Update(cristina);
            User fromRepo = repo.Get(new User("Cristina"));
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
            return new User(Role.Administrator, "alvaro", "Álvaro", "Gómez", "pass1234", "gomez@gomez.uy");
        }

        private User CreateUserMacri()
        {
            return new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
        }

        private IDesignTimeDbContextFactory<Context> GetContextFactory()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new InMemoryContextFactory(options);
        }

        private void CleanUpRepo()
        {
            foreach (User user in repo.GetAll())
                repo.Delete(user);
        }

        private User GetAlvaro() => repo.Get(new User("alvaro"));
        private User GetMacri() => repo.Get(new User("Gato"));
    }
}
