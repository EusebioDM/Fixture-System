using EirinDuran.Domain.User;
using EirinDuran.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EirinDuran.WebApi.Models;
using EirinDuran.IServices;

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
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var controller = new UsersController(loginServices, mockUserService.Object);

            var obtainedResult = controller.Get() as ActionResult<List<User>>;
            var val = obtainedResult.Value;

            mockUserService.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);

            IEnumerable<User> userList = obtainedResult.Value.ToList().Union(expectedUsers.ToList());

            Assert.IsTrue(userList.ToList().Count == 2);
        }

        [TestMethod]
        public void GetUserOkController()
        {
            var expectedUser = new User(Role.Administrator, "juanandres", "Juan", "Perez", "user", "juan@perez.org");
            var mockUserService = new Mock<IUserServices>();

            mockUserService.Setup(bl => bl.GetUser(expectedUser)).Returns(expectedUser);
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var controller = new UsersController(loginServices, mockUserService.Object);

            var obtainedResult = controller.GetById(expectedUser.UserName) as ActionResult<User>;

            mockUserService.Verify(m => m.GetUser(expectedUser), Times.AtMostOnce());
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUser);
        }

        [TestMethod]
        public void CreateValidUserController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", Role = Role.Follower };
            var fakeUser = new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com");

            var userServiceMock = new Mock<IUserServices>();
           
            userServiceMock.Setup(userService => userService.AddUser(fakeUser));
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var controller = new UsersController(loginServices, userServiceMock.Object);

            var result = controller.Create(modelIn);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetUserName", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.UserName, modelOut.UserName);
        }

        [TestMethod]
        public void CreateFailedUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));
            var controller = new UsersController(loginServices, userService.Object);

            controller.ModelState.AddModelError("", "Error");
            var result = controller.Create(modelIn);

            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }
    }
}
