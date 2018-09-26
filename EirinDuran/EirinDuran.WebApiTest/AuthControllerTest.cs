using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.User;
using Moq;
using EirinDuran.WebApi.Controllers;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

            var controller = new AuthController(configuration.Object, new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com")));

            LoginModelIn loginModel = new LoginModelIn();
            loginModel.UserName = "Macri";
            loginModel.Password = "Macri";

            var result = controller.Login(loginModel);

            var createdResult = result as OkObjectResult;

            Assert.AreEqual(200, createdResult.StatusCode);
        }

    }
}
