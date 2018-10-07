using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IServices;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace EirinDuran.WebApiTest
{

    [TestClass]
    public class EnconuntersControllerTest
    {
        private TeamDTO river;
        private TeamDTO boca;
        private SportDTO football;
        private UserDTO santiago;

        [TestInitialize]
        public void SetUp()
        {
            football = new SportDTO() { Name = "Futbol" };
            river = new TeamDTO() { Name = "River", SportName = "Futbol" };
            boca = new TeamDTO() { Name = "Boca", SportName = "Futbol" };

            santiago = new UserDTO()
            {
                UserName = "SMauricio",
                Name = "Mauricio",
                Surname = "Santiago",
                Password = "cat123",
                Mail = "sm@gmail.com",
                IsAdmin = true
            };
        }

        [TestMethod]
        public void GetAllEncountersOkEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            DateTime encounterDate = new DateTime(2018, 12, 10);

            EncounterDTO enc = new EncounterDTO();
            enc.SportName = football.Name;
            enc.AwayTeamName = river.Name;
            enc.HomeTeamName = boca.Name;
            enc.DateTime = encounterDate;

            List<EncounterDTO> encs = new List<EncounterDTO>() { enc };
            enconunterServicesMock.Setup(m => m.GetAllEncounters()).Returns(encs);

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Get(new DateTime(), new DateTime()) as ActionResult<List<EncounterDTO>>;
            enconunterServicesMock.Verify(e => e.GetAllEncounters(), Times.AtMostOnce());

            bool areEqual = obtainedResult.Value.ToList().All(encs.Contains);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void CatchServicesExceptionTryToGetAllEncounters()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            enconunterServicesMock.Setup(m => m.GetAllEncounters()).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Get(new DateTime(), new DateTime()) as ActionResult<List<EncounterDTO>>;
            enconunterServicesMock.Verify(e => e.GetAllEncounters(), Times.AtMostOnce());
            var result = obtainedResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void CreateEncounterOkEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            DateTime encounterDate = new DateTime(2018, 12, 10);

            EncounterDTO enc = new EncounterDTO
            {
                SportName = football.Name,
                AwayTeamName = river.Name,
                HomeTeamName = boca.Name,
                DateTime = encounterDate
            };

            enconunterServicesMock.Setup(m => m.CreateEncounter(enc));

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(enc) as CreatedAtRouteResult;
            enconunterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(201, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void CreateEncounterWithoutPermissionEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            DateTime encounterDate = new DateTime(2018, 12, 10);

            EncounterDTO enc = new EncounterDTO();
            enc.SportName = football.Name;
            enc.AwayTeamName = river.Name;
            enc.HomeTeamName = boca.Name;
            enc.DateTime = encounterDate;

            enconunterServicesMock.Setup(m => m.CreateEncounter(enc)).Throws(new InsufficientPermissionException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(enc) as UnauthorizedResult;
            enconunterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(401, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void TryToCreateEncounterWithInvalidTeamsEncountersControllers()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            DateTime encounterDate = new DateTime(2018, 12, 10);

            EncounterDTO enc = new EncounterDTO();
            enc.SportName = football.Name;
            enc.AwayTeamName = river.Name;
            enc.HomeTeamName = boca.Name;
            enc.DateTime = encounterDate;

            enconunterServicesMock.Setup(m => m.CreateEncounter(enc)).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(enc) as BadRequestObjectResult;
            enconunterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void AddCommentToEncounterEncountersController()
        {
            var enconunterServices = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            EncounterDTO encounter = new EncounterDTO() { Id = IntToGuid(4), SportName = "Futbol", AwayTeamName = "Pe�arol", HomeTeamName = "Nacional" };
            IEnumerable<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            enconunterServices.Setup(e => e.GetAllEncounters()).Returns(encounters);

            var controller = new EncountersController(loginServices, enconunterServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.AddComment(4 + "", "This is a test comment in a mock!") as OkResult;
            enconunterServices.Verify(m => m.GetAllEncounters(), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(200, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void AddCommentToEncounterDoesNotExistsEncountersController()
        {
            var enconunterServices = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            EncounterDTO encounter = new EncounterDTO() { Id = IntToGuid(4), SportName = "Futbol", AwayTeamName = "Pe�arol", HomeTeamName = "Nacional" };
            IEnumerable<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            enconunterServices.Setup(e => e.AddComment(4 + "", "This is a test comment in a mock!")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, enconunterServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.AddComment(4 + "", "This is a test comment in a mock!") as BadRequestObjectResult;
            enconunterServices.Verify(m => m.AddComment(4 + "", "This is a test comment in a mock!"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterOkEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            enconunterServicesMock.Setup(m => m.DeleteEncounter("1"));

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as OkResult;
            enconunterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(200, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterWithoutPermissionEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            enconunterServicesMock.Setup(m => m.DeleteEncounter("1")).Throws(new InsufficientPermissionException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as UnauthorizedResult;
            enconunterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(401, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterWithoutExistsEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            enconunterServicesMock.Setup(m => m.DeleteEncounter("1")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as BadRequestObjectResult;
            enconunterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void GetCommentsOfEncounterOkEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            CommentDTO comment = new CommentDTO() { UserName = santiago.UserName, Message = "Hi! This is a test comment." };
            List<CommentDTO> comments = new List<CommentDTO>() { comment };

            enconunterServicesMock.Setup(m => m.GetAllCommentsToOneEncounter("1")).Returns(comments);

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.GetEncounterComments("1") as ActionResult<IEnumerable<CommentDTO>>;
            enconunterServicesMock.Verify(e => e.GetAllCommentsToOneEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(comment.UserName, obtainedResult.Value.ToList()[0].UserName);
            Assert.AreEqual(comment.Message, obtainedResult.Value.ToList()[0].Message);
        }

        [TestMethod]
        public void GetCommentsOfEncounterWithoutExistsEncountersController()
        {
            var enconunterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            enconunterServicesMock.Setup(m => m.GetAllCommentsToOneEncounter("1")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, enconunterServicesMock.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.GetEncounterComments("1") as ActionResult<IEnumerable<CommentDTO>>;
            enconunterServicesMock.Verify(e => e.GetAllCommentsToOneEncounter("1"), Times.AtMostOnce());
            var result = obtainedResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void GetAvailableFixtureGeneratorsTest()
        {
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<string> expected = new List<string>() { "RoundRobinFixture", "AllOnceFixture" };
            encounterServicesMock.Setup(s => s.GetAvailableFixtureGenerators()).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object) { ControllerContext = controllerContext, };
            var actual = controller.GetAvailableFixtureGenerators();

            Assert.IsTrue(expected.Count() == actual.Value.Count);
            Assert.IsTrue(actual.Value.Contains("RoundRobinFixture"));
            Assert.IsTrue(actual.Value.Contains("AllOnceFixture"));
        }

        [TestMethod]
        public void GenerateFixtureOkTest()
        {
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<EncounterDTO> expected = new List<EncounterDTO>() { new EncounterDTO() { Id = IntToGuid(1) }, new EncounterDTO() { Id = IntToGuid(2) } };
            encounterServicesMock.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object) { ControllerContext = controllerContext, };
            IActionResult actual = controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });

            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GenerateFixtureNotOkTest()
        {
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<EncounterDTO> expected = new List<EncounterDTO>() { new EncounterDTO() { Id = IntToGuid(1) }, new EncounterDTO() { Id = IntToGuid(2) } };
            encounterServicesMock.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object) { ControllerContext = controllerContext, };
            controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });
            encounterServicesMock.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Throws(new ServicesException());
            var actual = controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });

            Assert.IsInstanceOfType(actual, typeof(BadRequestObjectResult));
        }

        public Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}





