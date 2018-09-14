using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using Microsoft.EntityFrameworkCore;
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
        UserRepository repo;
        IContext context;
        private User alvaro;
        private User macri;

        [TestMethod]
        public void AddUserTest()
        {
            repo.Add(macri);

            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { macri };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectAlreadyExistsInDataBaseException))]
        public void AddExistingUserTest()
        {
            repo.Add(macri);
            repo.Add(macri);
        }

        [TestMethod]
        public void RemoveUserTest()
        {
            repo.Add(macri);
            repo.Add(alvaro);
            repo.Delete(macri);

            IEnumerable<User> actual = repo.GetAll();
            IEnumerable<User> expected = new List<User> { alvaro };

            Assert.IsTrue(Helper.CollectionsHaveSameElements(actual, expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesntExistsInDataBaseException))]
        public void RemoveNonExistingUserTest()
        {
            repo.Delete(macri);
        }

        [TestMethod]
        public void GetUserTest()
        {
            repo.Add(macri);
            User fromRepo = repo.Get(macri.UserName);
            Assert.AreEqual(macri, fromRepo);
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
            repo.Add(macri);
            macri.Role = Role.Follower;
            macri.Surname = "Rodriges";

            repo.Update(macri);
            User fromRepo = repo.Get(macri.UserName);

            Assert.AreEqual(Role.Follower, fromRepo.Role);
            Assert.AreEqual("Rodriges", fromRepo.Surname);
        }

        [TestInitialize]
        public void TestInit()
        {
            context = GetTestContext();
            repo = new UserRepository(context);
            CleanUpRepo();
            alvaro = GetUserAlvaro();
            macri = GetUserMacri();
        }

        private User GetUserAlvaro()
        {
            return new User(Role.Administrator, "A.G�mez", "�lvaro", "G�mez", "pass1234", "gomez@gomez.uy");
        }

        private User GetUserMacri()
        {
            return new User(Role.Administrator, "Gato", "Mauricio", "Macri", "gato123", "macri@gmail.com");
        }

        private IContext GetTestContext()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase("In Memory Test DB").Options;
            return new Context(options);
        }

        private void CleanUpRepo()
        {
            foreach (User user in repo.GetAll())
                repo.Delete(user);
        }


    }
}