using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class TeamsControllerTest
    {
        private UserDTO mariano;

        [TestInitialize]
        public void SetUp()
        {
            mariano = new UserDTO()
            {
                UserName = "Mariano",
                Name = "UserTest",
                Surname = "UserTest",
                Password = "UserTest",
                Mail = "mail@mail.com",
                IsAdmin = true
            };
        }

        [TestMethod]
        public void CreateTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new TeamsController(login, teamServicesMock.Object) { ControllerContext = controllerContext, };

            TeamDTO teamIn = new TeamDTO() { Name = "Cavaliers" };

            var result = controller.Create(teamIn);
            var createdResult = result as CreatedAtRouteResult;
            var teamOut = createdResult.Value as TeamDTO;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetTeam", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(teamIn.Name, teamOut.Name);
        }
    }
}
