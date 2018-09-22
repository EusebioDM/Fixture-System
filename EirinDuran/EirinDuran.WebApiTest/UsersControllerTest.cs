using EirinDuran.Domain.User;
using EirinDuran.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using EirinDuran.WebApi.Controllers;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void GetAllUsersOk()
        {
            //Arrange: Construimos el mock y seteamos las expectativas
            /*var expectedUsers = new List<User> { new User(Role.Administrator, "juanandres", "Juan", "Perez", "user", "juan@perez.org"),
                                                 new User(Role.Follower, "robertoj", "roberto", "juarez", "mypass123", "rj@rj.com" ) };

            var mockUserService = new Mock<UserServices>();
            mockUserService.Setup(bl => bl.GetAll()).Returns(expectedUsers);

            var controller = new UsersController(mockUserService.Object);

            //Act
            var obtainedResult = controller.Get() as ActionResult<List<User>>;

            //Assert
            mockUserService.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUsers);*/
        }
    }
}
