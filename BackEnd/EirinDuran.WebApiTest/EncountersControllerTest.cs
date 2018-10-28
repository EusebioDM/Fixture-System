using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
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
    public class encountersControllerTest
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
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
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
            enc.TeamIds = new List<string>() {river.Name, boca.Name};
            enc.DateTime = encounterDate;

            List<EncounterDTO> encs = new List<EncounterDTO>() { enc };
            encounterServicesMock.Setup(m => m.GetAllEncounters()).Returns(encs);

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Get(new DateTime(), new DateTime()) as ActionResult<List<EncounterModelOut>>;
            encounterServicesMock.Verify(e => e.GetAllEncounters(), Times.AtMostOnce());


            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(enc.Id, obtainedResult.Value[0].Id);
            Assert.AreEqual(enc.SportName, obtainedResult.Value[0].SportName);
            Assert.AreEqual(enc.TeamIds.First(), obtainedResult.Value[0].TeamIds.First());
        }

        [TestMethod]
        public void CatchServicesExceptionTryToGetAllEncounters()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            encounterServicesMock.Setup(m => m.GetAllEncounters()).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Get(new DateTime(), new DateTime()) as ActionResult<List<EncounterModelOut>>;
            encounterServicesMock.Verify(e => e.GetAllEncounters(), Times.AtMostOnce());
            var result = obtainedResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void CreateEncounterOkEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
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
                TeamIds =  new List<string>() {river.Name, boca.Name},
                DateTime = encounterDate
            };

            encounterServicesMock.Setup(m => m.CreateEncounter(enc)).Returns(enc);

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(new EncounterModelIn()
            {
                TeamIds =  enc.TeamIds,
                DateTime = enc.DateTime,
                SportName = enc.SportName
            }) as CreatedAtRouteResult;
            encounterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(201, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void CreateEncounterWithoutPermissionEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
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
                TeamIds =  new List<string>() {river.Name, boca.Name},
                DateTime = encounterDate
            };

            encounterServicesMock.Setup(m => m.CreateEncounter(enc)).Throws(new InsufficientPermissionException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(new EncounterModelIn()
            {
                TeamIds = enc.TeamIds,
                DateTime = enc.DateTime,
                SportName = enc.SportName
            }) as UnauthorizedResult;
            encounterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(401, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void TryToCreateEncounterWithInvalidTeamsEncountersControllers()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
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
            enc.TeamIds = new List<string>() {river.Name, boca.Name};
            enc.DateTime = encounterDate;

            encounterServicesMock.Setup(m => m.CreateEncounter(enc)).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Create(new EncounterModelIn()
            {
                TeamIds =  enc.TeamIds,
                DateTime = enc.DateTime,
                SportName = enc.SportName
            }) as BadRequestObjectResult;
            encounterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void CreateEncounterWithInvalidModelInEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
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
                TeamIds = new List<string>() {river.Name, boca.Name},
                DateTime = encounterDate
            };

            encounterServicesMock.Setup(m => m.CreateEncounter(enc));

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };
            controller.ModelState.AddModelError("", "");

            var obtainedResult = controller.Create(new EncounterModelIn()) as BadRequestObjectResult;
            encounterServicesMock.Verify(e => e.CreateEncounter(enc), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void AddCommentToEncounterEncountersController()
        {
            var encounterServices = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();

            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            EncounterDTO encounter = new EncounterDTO() { Id = IntToGuid(4), SportName = "Futbol", TeamIds = new List<string>(){"Peñarol", "Nacional"}};
            IEnumerable<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterServices.Setup(e => e.GetAllEncounters()).Returns(encounters);

            var controller = new EncountersController(loginServices, encounterServices.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.AddComment(4 + "", "This is a test comment in a mock!") as OkResult;
            encounterServices.Verify(m => m.GetAllEncounters(), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(200, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void AddCommentToEncounterDoesNotExistsEncountersController()
        {
            var encounterServices = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();

            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            EncounterDTO encounter = new EncounterDTO() { Id = IntToGuid(4), SportName = "Futbol", TeamIds = new List<string>(){"Peñarol", "Nacional"} };
            IEnumerable<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterServices.Setup(e => e.AddComment(4 + "", "This is a test comment in a mock!")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, encounterServices.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.AddComment(4 + "", "This is a test comment in a mock!") as BadRequestObjectResult;
            encounterServices.Verify(m => m.AddComment(4 + "", "This is a test comment in a mock!"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterOkEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            encounterServicesMock.Setup(m => m.DeleteEncounter("1"));

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as OkResult;
            encounterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(200, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterWithoutPermissionEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            encounterServicesMock.Setup(m => m.DeleteEncounter("1")).Throws(new InsufficientPermissionException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as UnauthorizedResult;
            encounterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(401, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteEncounterWithoutExistsEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            encounterServicesMock.Setup(m => m.DeleteEncounter("1")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Delete("1") as BadRequestObjectResult;
            encounterServicesMock.Verify(e => e.DeleteEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(400, obtainedResult.StatusCode);
        }

        [TestMethod]
        public void GetCommentsOfEncounterOkEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            CommentDTO comment = new CommentDTO() { UserName = santiago.UserName, Message = "Hi! This is a test comment." };
            List<CommentDTO> comments = new List<CommentDTO>() { comment };

            encounterQueryServices.Setup(m => m.GetAllCommentsToOneEncounter("1")).Returns(comments);

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.GetEncounterComments("1") as ActionResult<IEnumerable<CommentDTO>>;
            encounterQueryServices.Verify(e => e.GetAllCommentsToOneEncounter("1"), Times.AtMostOnce());

            Assert.IsNotNull(obtainedResult);
            Assert.AreEqual(comment.UserName, obtainedResult.Value.ToList()[0].UserName);
            Assert.AreEqual(comment.Message, obtainedResult.Value.ToList()[0].Message);
        }

        [TestMethod]
        public void GetCommentsOfEncounterWithoutExistsEncountersController()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            encounterQueryServices.Setup(m => m.GetAllCommentsToOneEncounter("1")).Throws(new ServicesException());

            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(),encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.GetEncounterComments("1") as ActionResult<IEnumerable<CommentDTO>>;
            encounterQueryServices.Verify(e => e.GetAllCommentsToOneEncounter("1"), Times.AtMostOnce());
            var result = obtainedResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public void GetAvailableFixtureGeneratorsTest()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<string> expected = new List<string>() { "RoundRobinFixture", "AllOnceFixture" };
            fixtureGeneratorServices.Setup(s => s.GetAvailableFixtureGenerators()).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };
            var actual = controller.GetAvailableFixtureGenerators();

            Assert.IsTrue(expected.Count() == actual.Value.Count);
            Assert.IsTrue(actual.Value.Contains("RoundRobinFixture"));
            Assert.IsTrue(actual.Value.Contains("AllOnceFixture"));
        }

        [TestMethod]
        public void GenerateFixtureOkTest()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<EncounterDTO> expected = new List<EncounterDTO>() { new EncounterDTO() { Id = IntToGuid(1) }, new EncounterDTO() { Id = IntToGuid(2) } };
            fixtureGeneratorServices.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };
            IActionResult actual = controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });

            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GenerateFixtureBadModelInTest()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<EncounterDTO> expected = new List<EncounterDTO>() { new EncounterDTO() { Id = IntToGuid(1) }, new EncounterDTO() { Id = IntToGuid(2) } };
            fixtureGeneratorServices.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };
            controller.ModelState.AddModelError("", "");
            IActionResult actual = controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });

            Assert.IsInstanceOfType(actual, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GenerateFixtureNotOkTest()
        {
            var encounterServicesMock = new Mock<IEncounterSimpleServices>();
            var encounterQueryServices = new Mock<IEncounterQueryServices>();
            var fixtureGeneratorServices = new Mock<IFixtureServices>();
            ILoginServices loginServices = new LoginServicesMock(santiago);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            List<EncounterDTO> expected = new List<EncounterDTO>() { new EncounterDTO() { Id = IntToGuid(1) }, new EncounterDTO() { Id = IntToGuid(2) } };
            fixtureGeneratorServices.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Returns(expected);
            var controller = new EncountersController(loginServices, encounterServicesMock.Object, new LoggerStub(), encounterQueryServices.Object, fixtureGeneratorServices.Object) { ControllerContext = controllerContext, };
            controller.CreateFixture(new FixtureModelIn() { CreationAlgorithmName = "RoundRobinFixture", SportName = "Futbol", StartingDate = new DateTime(3000, 10, 10) });
            fixtureGeneratorServices.Setup(s => s.CreateFixture("RoundRobinFixture", "Futbol", new DateTime(3000, 10, 10))).Throws(new ServicesException());
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





