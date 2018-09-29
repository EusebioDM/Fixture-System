using EirinDuran.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Http;
using EirinDuran.IServices.Interfaces;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class UsersControllerTest
    {
        private UserDTO juan;
        private UserDTO roberto;
        private UserDTO macri;

        [TestMethod]
        public void GetAllUsersOkController()
        {
            var expectedUsers = new List<UserDTO> { juan, roberto };

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, }; 

            var obtainedResult = controller.Get() as ActionResult<List<UserDTO>>;
            var val = obtainedResult.Value;

            userService.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);

            IEnumerable<UserDTO> userList = obtainedResult.Value.ToList().Union(expectedUsers.ToList());

            Assert.IsTrue(userList.ToList().Count == 2);
        }

        [TestMethod]
        public void GetUserOkController()
        {
            var expectedUser = juan;
            var mockUserService = new Mock<IUserServices>();

            mockUserService.Setup(bl => bl.GetUser(expectedUser.UserName)).Returns(expectedUser);
            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext() { HttpContext = httpContext, };


            var controller = new UsersController(loginServices, mockUserService.Object) { ControllerContext = controllerContext, }; 

            var obtainedResult = controller.GetById(expectedUser.UserName) as ActionResult<UserDTO>;

            mockUserService.Verify(m => m.GetUser(expectedUser.UserName), Times.AtMostOnce());
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUser);
        }

        [TestMethod]
        public void CreateUserOkController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", IsAdmin = true };
            var fakeUser = new UserDTO() { UserName = "pepeAvila", Surname = "�vila", Name = "Pepe", Password = "user", Mail = "pepeavila@mymail.com", IsAdmin = true };

            var userServiceMock = new Mock<IUserServices>();

            userServiceMock.Setup(userService => userService.CreateUser(fakeUser));
            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc"; 
                                                                                                                                                                                                                                                                                                                                                                                                                           
            var controllerContext = new ControllerContext() { HttpContext = httpContext, };

            var controller = new UsersController(loginServices, userServiceMock.Object) { ControllerContext = controllerContext, }; 

            var result = controller.Create(modelIn);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetUser", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.UserName, modelOut.UserName);
        }

        [TestMethod]
        public void CreateUserWithoutPermissionController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", IsAdmin = false };

            var userServiceMock = new Mock<IUserServices>();

            userServiceMock.Setup(userService => userService.CreateUser(It.IsAny<UserDTO>())).Throws(new InsufficientPermissionException());
            ILoginServices loginServices = new LoginServicesMock(roberto);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJGb2xsb3dlciIsIlVzZXJOYW1lIjoianVhbmNhcmxvc3MiLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1MzgwNzg0OTYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t0LDLnYGnD-eDM9xvc_CJkzyDzFAoGJZYivDzKGrjeM";

            var controllerContext = new ControllerContext() { HttpContext = httpContext, };

            var controller = new UsersController(loginServices, userServiceMock.Object) { ControllerContext = controllerContext, };

            var result = controller.Create(modelIn);
            var createdResult = result as BadRequestResult;

            Assert.IsNotNull(createdResult);
            
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]

        public void CreateFailedUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, };

            controller.ModelState.AddModelError("", "Error");
            var result = controller.Create(modelIn);

            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteUserOkUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();

            string id = "pepe";

            userService.Setup(us => us.DeleteUser(id));

            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, };

            var result = controller.Delete(id);

            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteUserDoesNotExistsUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();

            string id = "pepe";

            userService.Setup(us => us.DeleteUser(id)).Throws(new UserTryToDeleteDoesNotExistsException());

            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, };

            var result = controller.Delete(id);

            var createdResult = result as BadRequestResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void UpdateUserDataUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();

            string id = "Macri";

            userService.Setup(us => us.Modify(macri));

            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, };

            var result = controller.Put(id, new UserModelIn() {
                UserName = "Macri",
                Name = "UserTest",
                Surname = "UserTest",
                Password = "user",
                Mail = "mail@gmail.com",
                IsAdmin = true
            } );

            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestInitialize]
        public void Init()
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

            roberto = new UserDTO()
            {
                UserName = "robertoj",
                Name = "roberto",
                Surname = "juarez",
                Password = "mypass123",
                Mail = "rj@rj.com",
                IsAdmin = false
            };
            macri = new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            };

        }
    }
}
