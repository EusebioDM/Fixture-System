using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Services;

namespace EirinDuran.ServicesTest
{
    [TestClass]
    public class LoginServiceTest
    {
        [TestMethod]
        public void SampleLoginOk()
        {
            LoginServices login = new LoginServices();
            login.CreateSession("sSanchez", "user");
            Assert.AreEqual("sSanchez", login.LoggedUser.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(UserTryToLogginDoesNotExists))]
        public void UserTryToLogginDoesNotExists()
        {
            LoginServices login = new LoginServices();
            login.CreateSession("pAntonio", "user");
        }
    }
}
