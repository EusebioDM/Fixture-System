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
using EirinDuran.IServices.DTOs;

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

    }
}
