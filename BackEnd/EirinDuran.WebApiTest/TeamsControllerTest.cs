using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Services_Interfaces;
using EirinDuran.WebApi.Models;

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
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            
            TeamModelIn teamIn = new TeamModelIn() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(teamIn.Map())).Returns(new TeamDTO() { Name = "Cavaliers", SportName = "Futbol" });
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.CreateTeam(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(teamIn.Map()), Times.AtMostOnce);
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
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            TeamModelIn teamIn = new TeamModelIn() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(It.IsAny<TeamDTO>())).Throws(new InsufficientPermissionException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext
            };

            var result = controller.CreateTeam(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(It.IsAny<TeamDTO>()), Times.AtMostOnce);
            var unauthorizedResult = result as UnauthorizedResult;

            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public void CreateAlreadyExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            TeamModelIn teamIn = new TeamModelIn() { Name = "Cavaliers", SportName = "Futbol" };
            teamServicesMock.Setup(t => t.CreateTeam(It.IsAny<TeamDTO>())).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.CreateTeam(teamIn);
            teamServicesMock.Verify(t => t.CreateTeam(It.IsAny<TeamDTO>()), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetAllTeamsOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

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
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetAllTeams();
            teamServicesMock.Verify(t => t.GetAllTeams(), Times.AtMostOnce);
            var resultRequest = result as ActionResult<List<TeamModelOut>>;

            Assert.IsNotNull(resultRequest);
            Assert.IsNotNull(resultRequest.Value);
            Assert.AreEqual(team1.SportName, resultRequest.Value[0].SportName);
            Assert.AreEqual(team1.Name, resultRequest.Value[0].Name);
            Assert.AreEqual(team2.SportName, resultRequest.Value[1].SportName);
            Assert.AreEqual(team2.Name, resultRequest.Value[1].Name);
        }

        [TestMethod]
        public void ServicesExceptionWhenTryToGetAllTeamsOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            teamServicesMock.Setup(t => t.GetAllTeams()).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetAllTeams();
            teamServicesMock.Verify(t => t.GetAllTeams(), Times.AtMostOnce);
            var value = result as ActionResult<List<TeamModelOut>>;
            var resultRequest = result.Result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.GetTeam(teamId)).Returns(team1);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetTeamById(teamId);
            teamServicesMock.Verify(t => t.GetTeam(teamId), Times.AtMostOnce);
            var resultRequest = result as ActionResult<TeamModelOut>;

            Assert.IsNotNull(resultRequest);
            Assert.IsNotNull(resultRequest.Value);
            TeamModelOut teamRecover = resultRequest.Value;
            Assert.AreEqual(team1.Name, teamRecover.Name);
            Assert.AreEqual(team1.SportName, teamRecover.SportName);
        }

        [TestMethod]
        public void TryToGetWithoutExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.GetTeam(teamId)).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetTeamById(teamId);
            teamServicesMock.Verify(t => t.GetTeam(teamId), Times.AtMostOnce);
            var resultRequest = result.Result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void DeleteTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName));
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteTeam(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as OkResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(200, resultRequest.StatusCode);
        }

        [TestMethod]
        public void DeleteTeamWithoutPermissionTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName)).Throws(new InsufficientPermissionException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteTeam(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as UnauthorizedResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(401, resultRequest.StatusCode);
        }

        [TestMethod]
        public void TryToDeleteTeamWithoutExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName)).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteTeam(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetEncountersByTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            EncounterDTO encounter = new EncounterDTO() { TeamIds = new List<string>() {"team1", "team2"}, SportName = "Sport" };
            string teamName = "Team_Sport";
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterQueryServicesMock.Setup(e => e.GetEncountersByTeam(teamName)).Returns(encounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetTeamEncounters(teamName);
            encounterQueryServicesMock.Verify(e => e.GetEncountersByTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as ActionResult<List<EncounterModelOut>>;
            List<EncounterModelOut> obtaniedEncounters = resultRequest.Value;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(encounter.SportName, obtaniedEncounters[0].SportName);
            Assert.AreEqual(encounter.TeamIds.First(), obtaniedEncounters[0].TeamIds.First());
            Assert.AreEqual(encounter.TeamIds.Last(), obtaniedEncounters[0].TeamIds.Last());
        }

        [TestMethod]
        public void GetEncountersByTeamWithoutExistsTeamTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            string teamName = "Team_Sport";
            encounterQueryServicesMock.Setup(e => e.GetEncountersByTeam(teamName)).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.GetTeamEncounters(teamName);
            encounterQueryServicesMock.Verify(e => e.GetEncountersByTeam(teamName), Times.AtMostOnce);
            var value = result as ActionResult<List<EncounterModelOut>>;
            var requestResult = value.Result as BadRequestObjectResult;

            Assert.IsNotNull(requestResult);
            Assert.AreEqual(400, requestResult.StatusCode);
        }

        [TestMethod]
        public void AddFollowerToLoggedUserTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            EncounterDTO encounter = new EncounterDTO() { TeamIds = new List<string>() {"team1", "team2"}, SportName = "Sport" };
            string teamName = "Team_Sport";
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            teamServicesMock.Setup(e => e.AddFollowedTeam(teamName));
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.AddFollowedTeamToLogedUser(teamName);
            teamServicesMock.Verify(e => e.AddFollowedTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as OkResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(200, resultRequest.StatusCode);
        }

        [TestMethod]
        public void TryToAddFollowerWihoutExistsToLoggedUser()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            EncounterDTO encounter = new EncounterDTO() { TeamIds = new List<string>() {"team1", "team2"}, SportName = "Sport" };
            string teamName = "Team_Sport";
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            teamServicesMock.Setup(e => e.AddFollowedTeam(teamName)).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.AddFollowedTeamToLogedUser(teamName);
            teamServicesMock.Verify(e => e.AddFollowedTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void UpdateTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            TeamModelIn team1 = new TeamModelIn() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(team1.Map()));
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.ModifyTeam(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(team1.Map()), Times.AtMostOnce);
            var resultRequest = result as OkResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(200, resultRequest.StatusCode);
        }

        [TestMethod]
        public void UpdateTeamWithoutPermissionTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            TeamModelIn team1 = new TeamModelIn() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(It.IsAny<TeamDTO>())).Throws(new InsufficientPermissionException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.ModifyTeam(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(It.IsAny<TeamDTO>()), Times.AtMostOnce);
            var resultRequest = result as UnauthorizedResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(401, resultRequest.StatusCode);
        }

        [TestMethod]
        public void UpdateTeamDoesNotExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();

            TeamModelIn team1 = new TeamModelIn() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(It.IsAny<TeamDTO>())).Throws(new ServicesException());
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object)
            {
                ControllerContext = controllerContext
            };

            var result = controller.ModifyTeam(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(It.IsAny<TeamDTO>()), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }
    }
}
