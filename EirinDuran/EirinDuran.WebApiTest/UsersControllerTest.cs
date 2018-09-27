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
using Microsoft.AspNetCore.Http;

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

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, }; 

            var obtainedResult = controller.Get() as ActionResult<List<User>>;
            var val = obtainedResult.Value;

            userService.VerifyAll();
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

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext() { HttpContext = httpContext, };


            var controller = new UsersController(loginServices, mockUserService.Object) { ControllerContext = controllerContext, }; 

            var obtainedResult = controller.GetById(expectedUser.UserName) as ActionResult<User>;

            mockUserService.Verify(m => m.GetUser(expectedUser), Times.AtMostOnce());
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, expectedUser);
        }

        [TestMethod]
        public void CreateUserOkController()
        {
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", Role = Role.Follower };
            var fakeUser = new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com");

            var userServiceMock = new Mock<IUserServices>();

            userServiceMock.Setup(userService => userService.AddUser(fakeUser));
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

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
            var modelIn = new UserModelIn() { UserName = "Alberto", Name = "Alberto", Surname = "Lacaze", Mail = "albertito@mail.com", Password = "pass", Role = Role.Follower };
            var fakeUser = new User(Role.Administrator, "pepeAvila", "Pepe", "Ávila", "user", "pepeavila@mymail.com");

            var userServiceMock = new Mock<IUserServices>();

            userServiceMock.Setup(userService => userService.AddUser(fakeUser)).Throws(new InsufficientPermissionToPerformThisActionException());
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Follower, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJGb2xsb3dlciIsIlVzZXJOYW1lIjoianVhbmNhcmxvc3MiLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1MzgwNzg0OTYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t0LDLnYGnD-eDM9xvc_CJkzyDzFAoGJZYivDzKGrjeM";

            var controllerContext = new ControllerContext() { HttpContext = httpContext, };

            var controller = new UsersController(loginServices, userServiceMock.Object) { ControllerContext = controllerContext, };

            var result = controller.Create(modelIn);
            var createdResult = result as BadRequestResult;

            Assert.IsNotNull(createdResult);
            
            Assert.AreEqual(403, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateFailedUserController()
        {
            var modelIn = new UserModelIn();

            var userService = new Mock<IUserServices>();
            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

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

            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

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

            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

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

            User macri = new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com");

            userService.Setup(us => us.Modify(macri));

            ILoginServices loginServices = new LoginServicesMock(macri);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new UsersController(loginServices, userService.Object) { ControllerContext = controllerContext, };

            //var result = controller.Put(id, );

            //var createdResult = result as BadRequestResult;

            //Assert.IsNotNull(createdResult);
            //Assert.AreEqual(400, createdResult.StatusCode);
        }
    }
}
