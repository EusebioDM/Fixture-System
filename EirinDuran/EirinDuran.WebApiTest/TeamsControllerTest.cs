using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EirinDuran.IServices.Exceptions;

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
            var encounterServicesMock = new Mock<IEncounterServices>();
            TeamDTO teamIn = new TeamDTO() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(teamIn));
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Create(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(teamIn), Times.AtMostOnce);
            var createdResult = result as CreatedAtRouteResult;
            var teamOut = createdResult.Value as TeamDTO;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetTeam", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(teamIn.Name, teamOut.Name);
        }

        [TestMethod]
        public void CreateTeamWithoutPermissionTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();
            TeamDTO teamIn = new TeamDTO() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(teamIn)).Throws(new InsufficientPermissionException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Create(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(teamIn), Times.AtMostOnce);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public void CreateAlreadyExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();
            TeamDTO teamIn = new TeamDTO() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(teamIn)).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Create(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(teamIn), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetAllTeamsOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };
            TeamDTO team2 = new TeamDTO() { Name = "Team2", SportName = "Futbol" };
            List<TeamDTO> teams = new List<TeamDTO>() { team1, team2 };
            teamServicesMock.Setup(t => t.GetAllTeams()).Returns(teams);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Get();
            teamServicesMock.Verify(t => t.GetAllTeams(), Times.AtMostOnce);
            var resultRequest = result as ActionResult<List<TeamDTO>>;

            Assert.IsNotNull(resultRequest);
            Assert.IsNotNull(resultRequest.Value);
            IEnumerable<TeamDTO> sportList = resultRequest.Value.ToList().Union(teams);
            Assert.IsTrue(sportList.ToList().Count == 2);
        }
    }
}
