using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Moq;
using EirinDuran.WebApi.Controllers;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using System;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class AuthControllerTest
    {
        [TestMethod]
        public void CreateLoginOkAuthTest()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x[It.IsAny<string>()]).Returns("mySuperSecretKey");
            var macri = new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            };
            var controller = new AuthController(configuration.Object, new LoginServicesMock(macri));

            LoginModelIn loginModel = new LoginModelIn();
            loginModel.UserName = "Macri";
            loginModel.Password = "Macri";

            var result = controller.Login(loginModel);

            var createdResult = result as OkObjectResult;

            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateLoginInvalidModelInAuthTest()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x[It.IsAny<string>()]).Returns("mySuperSecretKey");
            var userTest = new UserDTO()
            {
                UserName = "UserTest",
                Name = "UserTest",
                Surname = "UserTest",
                Password = "cat123",
                Mail = "usertest@gmail.com",
                IsAdmin = true
            };
            var controller = new AuthController(configuration.Object, new LoginServicesMock(userTest));
            controller.ModelState.AddModelError("UserName is required", "");
            controller.ModelState.AddModelError("Password is required", "");

            LoginModelIn loginModel = new LoginModelIn();
    
            var result = controller.Login(loginModel);

            var createdResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateLoginExceptionAuthTest()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x[It.IsAny<string>()]).Returns("mySuperSecretKey");
            var macri = new UserDTO()
            {
                UserName = "UserTest",
                Name = "UserTest",
                Surname = "UserTest",
                Password = "cat123",
                Mail = "usertest@gmail.com",
                IsAdmin = true
            };

            var loginServicesMock = new Mock<ILoginServices>();
            loginServicesMock.Setup(l => l.CreateSession("UserTest", "UserTest")).Throws(new Exception());
            var controller = new AuthController(configuration.Object, loginServicesMock.Object);

            LoginModelIn loginModel = new LoginModelIn();
            loginModel.UserName = "UserTest";
            loginModel.Password = "UserTest";

            var result = controller.Login(loginModel);

            var createdResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, createdResult.StatusCode);
        }
    }
}
