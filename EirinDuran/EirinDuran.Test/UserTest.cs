using Microsoft.VisualStudio.TestTools.UnitTesting;
using EirinDuran.Domain.User;

namespace EirinDuran.Test
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void CreateEmptyUserOk()
        {
            User user  = new User();
            Assert.IsNotNull(user);
        }
    }
}
