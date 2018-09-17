using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
