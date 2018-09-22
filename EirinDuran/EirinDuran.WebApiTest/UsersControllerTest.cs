using EirinDuran.Domain.User;
using EirinDuran.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void GetAllUsersOkController()
        {
            var expectedUsers = new List<User> { new User(Role.Administrator, "juanandres", "Juan", "Perez", "user", "juan@perez.org"),
                                                 new User(Role.Follower, "robertoj", "roberto", "juarez", "mypass123", "rj@rj.com" ) };

            var mockUserService = new Mock<IUserServices>();
            mockUserService.Setup(bl => bl.GetAllUsers()).Returns(expectedUsers);

            var controller = new UsersController(mockUserService.Object);

            var obtainedResult = controller.Get() as ActionResult<List<User>>;
            var val = obtainedResult.Value;

            mockUserService.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);

            bool secuenceAreEqual = obtainedResult.Value.ToList().SequenceEqual(expectedUsers.ToList());

            Assert.IsTrue(secuenceAreEqual);
        }

        [TestMethod]
        public void GetUserOkController()
        {
            //Arrange: Construimos el mock y seteamos las expectativas
            var expectedUser = new User(Role.Administrator, "juanandres", "Juan", "Perez", "user", "juan@perez.org");
            var mockUserService = new Mock<IUserServices>();
            mockUserService.Setup(bl => bl.GetUser(expectedUser)).Returns(expectedUser);

            var controller = new UsersController(mockUserService.Object);

            //Act
            var obtainedResult = controller.GetById(expectedUser.UserName) as ActionResult<User>;

            //Assert
            mockUserService.Verify(m => m.GetUser(expectedUser), Times.AtMostOnce());
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUser);
        }
    }
}
