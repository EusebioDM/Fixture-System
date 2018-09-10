using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Net;
using EirinDuran.WebApi.Controllers;
using EirinDuran.IDataAccess;
using EirinDuran.DataAccess;
using EirinDuran.Domain.User;

namespace EirinDuran.Test
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void PostUserReturnSameUserOk()
        {
            IRepository<User> userRepository = new UserRepository(new TestContext());
            var controller = new UsersController(userRepository);
        
            User item = GetDemoUser();

            var result = controller.Create(item) as CreatedAtRouteNegotiatedContentResult<User>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "IAmAnUserTest");
        }

        User GetDemoUser()
        {
            return new User() { UserName = "IAmAnUserTest", Name = "PleaseIgnoreMe", Surname = "UserTest", Password = "pass", Mail = "test@test.com", Role = Role.Administrator };
        }
    }
}
