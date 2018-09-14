using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
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

        [TestInitialize]
        public void TestInit()
        {
            repo = new UserRepository(GetTestContext());
            alvaro = GetUserAlvaro();
            macri = GetUserMacri();
        }

        private User GetUserAlvaro()
        {
            return new User(Role.Administrator, "A.Gómez", "Álvaro", "Gómez", "pass1234", "gomez@gomez.uy");
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
    }
}
