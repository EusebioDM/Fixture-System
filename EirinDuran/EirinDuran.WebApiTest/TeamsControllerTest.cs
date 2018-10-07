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

        [TestMethod]
        public void ServicesExceptionWhenTryToGetAllTeamsOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            teamServicesMock.Setup(t => t.GetAllTeams()).Throws(new ServicesException());
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
            var value = result as ActionResult<List<TeamDTO>>;
            var resultRequest = result.Result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

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
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Get(teamId);
            teamServicesMock.Verify(t => t.GetTeam(teamId), Times.AtMostOnce);
            var resultRequest = result as ActionResult<TeamDTO>;

            Assert.IsNotNull(resultRequest);
            Assert.IsNotNull(resultRequest.Value);
            TeamDTO teamRecover = resultRequest.Value;
            Assert.AreEqual(team1.Name, teamRecover.Name);
            Assert.AreEqual(team1.SportName, teamRecover.SportName);
        }

        [TestMethod]
        public void TryToGetWithoutExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

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
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Get(teamId);
            teamServicesMock.Verify(t => t.GetTeam(teamId), Times.AtMostOnce);
            var resultRequest = result.Result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void DeleteTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName));
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

            var result = controller.Delete(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as OkResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(200, resultRequest.StatusCode);
        }

        [TestMethod]
        public void DeleteTeamWithoutPermissionTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName)).Throws(new InsufficientPermissionException());
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

            var result = controller.Delete(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as UnauthorizedResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(401, resultRequest.StatusCode);
        }

        [TestMethod]
        public void TryToDeleteTeamWithoutExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            string teamName = "Team_Sport";

            teamServicesMock.Setup(t => t.DeleteTeam(teamName)).Throws(new ServicesException());
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

            var result = controller.Delete(teamName);
            teamServicesMock.Verify(t => t.DeleteTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }

        [TestMethod]
        public void GetEncountersByTeamOkTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            EncounterDTO encounter = new EncounterDTO() { AwayTeamName = "Team", HomeTeamName = "Team2", SportName = "Sport" };
            string teamName = "Team_Sport";
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterServicesMock.Setup(e => e.GetEncountersByTeam(teamName)).Returns(encounters);
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

            var result = controller.GetEncounters(teamName);
            encounterServicesMock.Verify(e => e.GetEncountersByTeam(teamName), Times.AtMostOnce);
            var resultRequest = result as ActionResult<List<EncounterDTO>>;
            List<EncounterDTO> obtaniedEncounters = resultRequest.Value;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(encounter.SportName, obtaniedEncounters[0].SportName);
            Assert.AreEqual(encounter.AwayTeamName, obtaniedEncounters[0].AwayTeamName);
            Assert.AreEqual(encounter.HomeTeamName, obtaniedEncounters[0].HomeTeamName);
        }

        [TestMethod]
        public void GetEncountersByTeamWithoutExistsTeamTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            string teamName = "Team_Sport";
            encounterServicesMock.Setup(e => e.GetEncountersByTeam(teamName)).Throws(new ServicesException());
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

            var result = controller.GetEncounters(teamName);
            encounterServicesMock.Verify(e => e.GetEncountersByTeam(teamName), Times.AtMostOnce);
            var value = result as ActionResult<List<EncounterDTO>>;
            var requestResult = value.Result as BadRequestObjectResult;

            Assert.IsNotNull(requestResult);
            Assert.AreEqual(400, requestResult.StatusCode);
        }

        [TestMethod]
        public void AddFollowerToLoggedUserTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            EncounterDTO encounter = new EncounterDTO() { AwayTeamName = "Team", HomeTeamName = "Team2", SportName = "Sport" };
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
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
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
            var encounterServicesMock = new Mock<IEncounterServices>();

            EncounterDTO encounter = new EncounterDTO() { AwayTeamName = "Team", HomeTeamName = "Team2", SportName = "Sport" };
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
            var controller = new TeamsController(login, teamServicesMock.Object, encounterServicesMock.Object)
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
            var encounterServicesMock = new Mock<IEncounterServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(team1));
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

            var result = controller.Put(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(team1), Times.AtMostOnce);
            var resultRequest = result as OkResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(200, resultRequest.StatusCode);
        }

        [TestMethod]
        public void UpdateTeamWithoutPermissionTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(team1)).Throws(new InsufficientPermissionException());
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

            var result = controller.Put(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(team1), Times.AtMostOnce);
            var resultRequest = result as UnauthorizedResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(401, resultRequest.StatusCode);
        }

        [TestMethod]
        public void UpdateTeamDoesNotExistsTeamsController()
        {
            var teamServicesMock = new Mock<ITeamServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();

            TeamDTO team1 = new TeamDTO() { Name = "Team1", SportName = "Futbol" };

            string teamId = "Team1_Futbol";

            teamServicesMock.Setup(t => t.UpdateTeam(team1)).Throws(new ServicesException());
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

            var result = controller.Put(teamId, team1);
            teamServicesMock.Verify(t => t.UpdateTeam(team1), Times.AtMostOnce);
            var resultRequest = result as BadRequestObjectResult;

            Assert.IsNotNull(resultRequest);
            Assert.AreEqual(400, resultRequest.StatusCode);
        }
    }
}
