using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.WebApi.Controllers;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class UsersControllerTest
    {
        private UserDTO juan;
        private UserDTO roberto;
        private UserDTO pablo;
        private Mock<IEncounterServices> encounterServices;

        [TestInitialize]
        public void SetUp()
        {
            encounterServices = new Mock<IEncounterServices>();
            CreateUserJuan();
            CreateUserRoberto();
            CreateUserPablo();
        }

        private void CreateUserJuan()
        {
            juan = new UserDTO()
            {
                UserName = "juanandres",
                Name = "Juan",
                Surname = "Perez",
                Password = "user",
                Mail = "juan@perez.org",
                IsAdmin = true
            };
        }

        private void CreateUserRoberto()
        {
            roberto = new UserDTO()
            {
                UserName = "robertoj",
                Name = "roberto",
                Surname = "juarez",
                Password = "mypass123",
                Mail = "rj@rj.com",
                IsAdmin = false
            };
        }

        private void CreateUserPablo()
        {
            pablo = new UserDTO()
            {
                UserName = "Pablo",
                Name = "Pablo",
                Surname = "Montero",
                Password = "cat123",
                Mail = "pm@gmail.com",
                IsAdmin = true,
                FollowedTeamsNames = new List<string>() { "Atletics", "Yankees" }
            };
        }

        [TestMethod]
        public void GetAllUsersOkUsersController()
        {
            var expectedUsers = new List<UserDTO> { juan, roberto };

            var userServicesMock = new Mock<IUserServices>();
            userServicesMock.Setup(us => us.GetAllUsers()).Returns(expectedUsers);
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.Get() as ActionResult<List<UserDTO>>;
            var value = obtainedResult.Value;

            userServicesMock.Verify(us => us.GetAllUsers(), Times.AtMostOnce);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(value);
            Assert.AreEqual(2, value.Count);
            Assert.AreEqual(juan.Name, value.ElementAt(0).Name);
            Assert.AreEqual(roberto.Name, value.ElementAt(1).Name);
        }

        [TestMethod]
        public void GetUserOkUsersController()
        {
            var expectedUser = juan;
            var mockUserService = new Mock<IUserServices>();

            mockUserService.Setup(bl => bl.GetUser(expectedUser.UserName)).Returns(expectedUser);
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, mockUserService.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            mockUserService.Verify(m => m.GetUser(expectedUser.UserName), Times.AtMostOnce());
            var obtainedResult = controller.GetById(expectedUser.UserName) as ActionResult<UserDTO>;

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUser);
        }

        [TestMethod]
        public void CreateUserOkUsersController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", IsAdmin = true };
            var fakeUser = new UserDTO() { UserName = "pepeAvila", Surname = "Avila", Name = "Pepe", Password = "user", Mail = "pepeavila@mymail.com", IsAdmin = true };

            var userServicesMock = new Mock<IUserServices>();
            userServicesMock.Setup(userService => userService.CreateUser(fakeUser));
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            userServicesMock.Verify(m => m.CreateUser(fakeUser), Times.AtMostOnce());
            var result = controller.Create(modelIn);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetUser", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.UserName, modelOut.UserName);
        }

        [TestMethod]
        public void CreateUserWithoutPermissionUsersController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", IsAdmin = false };

            var userServicesMock = new Mock<IUserServices>();

            userServicesMock.Setup(userService => userService.CreateUser(It.IsAny<UserDTO>())).Throws(new InsufficientPermissionException());
            ILoginServices loginServices = new LoginServicesMock(roberto);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Create(modelIn);
            var createdResult = result as UnauthorizedResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(401, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateBadModelInUsersController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userService.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            controller.ModelState.AddModelError("", "Error");
            var result = controller.Create(modelIn);

            var createdResult = result as BadRequestResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteUserOkUsersController()
        {
            var modelIn = new UserModelIn();
            var userServicesMock = new Mock<IUserServices>();

            string id = "pepe";

            userServicesMock.Setup(us => us.DeleteUser(id));
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            userServicesMock.Verify(m => m.DeleteUser(id), Times.AtMostOnce());
            var result = controller.Delete(id);
            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteUserDoesNotExistsUsersController()
        {
            var modelIn = new UserModelIn();

            var userServicesMock = new Mock<IUserServices>();

            string id = "pepe";

            userServicesMock.Setup(us => us.DeleteUser(id)).Throws(new ServicesException());
            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            userServicesMock.Verify(m => m.DeleteUser(id), Times.AtMostOnce());
            var result = controller.Delete(id);
            var createdResult = result as BadRequestResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void UpdateUserDataUsersController()
        {
            var modelIn = new UserModelIn();
            var userServicesMock = new Mock<IUserServices>();

            string id = "Pablo";

            userServicesMock.Setup(us => us.ModifyUser(pablo));

            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Modify(id, new UserUpdateModelIn()
            {
                Name = "UserTest",
                Surname = "UserTest",
                Password = "user",
                Mail = "user@gmail.com"
            });

            userServicesMock.Verify(m => m.ModifyUser(pablo), Times.AtMostOnce());
            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        public void UpdateUserDataWithoutExistsUsersController()
        {
            var modelIn = new UserModelIn();
            var userServicesMock = new Mock<IUserServices>();

            string id = "Pablo";

            userServicesMock.Setup(us => us.ModifyUser(It.IsAny<UserDTO>())).Throws(new ServicesException());

            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Modify(id, new UserUpdateModelIn()
            {
                Name = "UserTest",
                Surname = "UserTest",
                Password = "user",
                Mail = "user@gmail.com"
            });

            userServicesMock.Verify(m => m.ModifyUser(It.IsAny<UserDTO>()), Times.AtMostOnce());
            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void GetLogedUserFollowedTeams()
        {
            var userServicesMock = new Mock<IUserServices>();

            ILoginServices loginServices = new LoginServicesMock(pablo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new UsersController(loginServices, userServicesMock.Object, encounterServices.Object)
            {
                ControllerContext = controllerContext,
            };

            List<string> result = controller.GetFollowedTeams().Value;
            Assert.AreEqual("Atletics", result[0]);
            Assert.AreEqual("Yankees", result[1]);
        }
    }
}
