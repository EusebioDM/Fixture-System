using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Domain.User;
using EirinDuran.Domain;
using System.Collections.Generic;
using EirinDuran.Domain.Fixture;
using System.Linq;

namespace EirinDuran.DomainTest
{
    
    [TestClass]
    public class UserTest
    {
        private User user;

        [TestInitialize]
        public void ConfigTests()
        {
            user = new User(Role.Administrator, "A.Gómez", "Álvaro", "Gómez", "pass1234", "gomez@gomez.uy");
        }

        [TestMethod]
        public void NewNotNullUserTest()
        {
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void UserNameOkUserTest()
        {
            Assert.AreEqual("A.Gómez", (string)user.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void UserNameEmptyUserTest()
        {
            user = new User(Role.Follower, "", "Álvaro", "Gómez", "pass1234", "agomez@mail.com");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void NameEmptyUserTest()
        {
            user.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void NameInvalidCharactersUserTest()
        {
            user.Name = "Alva334ro";
        }

        [TestMethod]
        public void NameOkUserTest()
        {
            Assert.AreEqual("Álvaro", (string)user.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void SurnameEmptyUserTest()
        {
            user.Surname = "";
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void SurnameInvalidCharactersUserTest()
        {
            user.Surname = "+Gomez";
        }

        [TestMethod]
        public void SurnameOkUserTest()
        {
            Assert.AreEqual("Gómez", (string)user.Surname);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void PasswordEmptyUserTest()
        {
            user.Password = "";
        }

        [TestMethod]
        public void PasswordOkUserTest()
        {
            Assert.AreEqual("pass1234", (string)user.Password);
        }

        [TestMethod]
        public void MailOkUserTest()
        {
            Assert.AreEqual("gomez@gomez.uy", (string)user.Mail);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void InvalidMailFormatTest()
        {
            user.Mail = "gomez@gomez";
        }

        [TestMethod]
        public void RoleVerificationTest()
        {
            bool result = user.Role == Role.Administrator;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddFollowedTeamsTest()
        {
            Sport futbol = new Sport("Futbol");
            Team boca = new Team("Boca",futbol);
            Team river = new Team("River", futbol);
            IEnumerable<Team> expected = new List<Team>() { boca, river };

            foreach (Team team in expected)
                user.AddFollowedTeam(team);

            IEnumerable<Team> actual = user.FollowedTeams;

            Assert.IsTrue(actual.Contains(boca));
            Assert.IsTrue(actual.Contains(river));
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestMethod]
        public void RemoveFollowedTeamsTest()
        {
            Sport futbol = new Sport("Futbol");
            Team boca = new Team("Boca",futbol);
            Team river = new Team("River", futbol);
            IEnumerable<Team> expected = new List<Team>() { boca };

            user.AddFollowedTeam(boca);
            user.AddFollowedTeam(river);
            user.RemoveFollowedTeam(river);

            IEnumerable<Team> actual = user.FollowedTeams;

            Assert.IsTrue(actual.Contains(boca));
            Assert.IsFalse(actual.Contains(river));
            Assert.AreEqual(expected.Count(), actual.Count());
        }
    }
}
