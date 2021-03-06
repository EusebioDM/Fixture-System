using System.Collections.Generic;
using System.Linq;
using SilverFixture.IServices.DTOs;
using SilverFixture.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilverFixture.IServices.Exceptions;
using SilverFixture.IServices.Services_Interfaces;
using SilverFixture.WebApi.Models;

namespace SilverFixture.WebApiTest
{
    [TestClass]
    public class SportControllerTest
    {
        private UserDTO mariano;
        private UserDTO rodolfo;
        private SportDTO football;
        private SportDTO tennis;
        private Mock<IEncounterQueryServices> encounterQueryServices;
        private Mock<IEncounterSimpleServices> encounterServicesMock;
        private Mock<ISportServices> sportServicesMock;
        private Mock<IPositionsServices> positionServicesMock;
        private Mock<ITeamServices> teamServicesMock;

        [TestInitialize]
        public void SetUp()
        {
            football = new SportDTO() { Name = "Futbol" };
            tennis = new SportDTO() { Name = "Tenis" };
            encounterQueryServices = new Mock<IEncounterQueryServices>();
            encounterServicesMock = new Mock<IEncounterSimpleServices>();
            sportServicesMock = new Mock<ISportServices>();
            positionServicesMock = new Mock<IPositionsServices>();
            teamServicesMock = new Mock<ITeamServices>();

            CreateUserMariano();
            CreateUserRodolfo();
        }

        private void CreateUserMariano()
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

        private void CreateUserRodolfo()
        {
            rodolfo = new UserDTO()
            {
                UserName = "Rodolfo",
                Name = "Rodolfo",
                Surname = "Rodolfo",
                Password = "user",
                Mail = "rofo@mail.com",
                IsAdmin = false
            };
        }

        [TestMethod]
        public void CreateSportOkSportsController()
        {
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>()));
            ILoginServices login = new LoginServicesMock(mariano);
            
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.CreateSport(footballIn);
            sportServicesMock.Verify(s => s.CreateSport(It.IsAny<SportDTO>()), Times.AtMostOnce);
            var createdResult = result as CreatedAtRouteResult;
            var footballOut = createdResult.Value as SportDTO;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetSport", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(footballIn.Name, footballOut.Name);
        }

        [TestMethod]
        public void CreateSportWithoutPermissionSportsController()
        {
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>())).Throws(new InsufficientPermissionException());
            ILoginServices login = new LoginServicesMock(rodolfo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.CreateSport(footballIn);
            sportServicesMock.Verify(s => s.CreateSport(It.IsAny<SportDTO>()), Times.AtMostOnce);
            var createdResult = result as UnauthorizedResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(401, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateSportAlreadyExistsSportsController()
        {
            
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>())).Throws(new ServicesException());
            
            
            ILoginServices login = new LoginServicesMock(rodolfo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.CreateSport(footballIn);
            sportServicesMock.Verify(s => s.CreateSport(It.IsAny<SportDTO>()), Times.AtMostOnce);
            var createdResult = result as BadRequestObjectResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteSportOkSportsController()
        {
            string name = "Tennis";
            var modelIn = new SportDTO() { Name = name };

            
            sportServicesMock.Setup(s => s.DeleteSport(name));
            
            
            
            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteSport(name);
            sportServicesMock.Verify(s => s.DeleteSport(name), Times.AtMostOnce);
            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteSportDoesNotExistsSportsController()
        {
            string name = "Water Polo";
            var modelIn = new SportDTO() { Name = name };

            
            sportServicesMock.Setup(s => s.DeleteSport(name)).Throws(new ServicesException());
            
            

            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteSport(name);
            sportServicesMock.Verify(s => s.DeleteSport(name), Times.AtMostOnce);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public void DeleteSportWithoutPermissionSportsController()
        {
            string name = "Futbol";
            var modelIn = new SportDTO() { Name = name };

            
            sportServicesMock.Setup(s => s.DeleteSport(name)).Throws(new InsufficientPermissionException());
            
            

            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.DeleteSport(name);
            sportServicesMock.Verify(s => s.DeleteSport(name), Times.AtMostOnce);
            var badRequestResult = result as UnauthorizedResult;

            Assert.AreEqual(401, badRequestResult.StatusCode);
        }

        [TestMethod]
        public void GetAllSportsOkSportsController()
        {
            var expectedSports = new List<SportDTO>() { football, tennis };
            
            sportServicesMock.Setup(s => s.GetAllSports()).Returns(expectedSports);
            
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetAllSports() as ActionResult<List<SportModelOut>>;
            var val = obtainedResult.Value;

            sportServicesMock.Verify(s=>s.GetAllSports(), Times.AtMostOnce);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(expectedSports[0].Name, obtainedResult.Value[0].Name);
        }

        [TestMethod]
        public void GetAllEncountersOfASpecificSport1()
        {
            string sportName = "Futbol";
            EncounterDTO encounter = CreateAEncounter(sportName);
            var expectedEncounters = new List<EncounterDTO>() { encounter };
            
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            
            encounterQueryServicesMock.Setup(s => s.GetEncountersBySport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncountersBySport(sportName) as ActionResult<List<EncounterModelOut>>;
            var val = obtainedResult.Value;

            encounterQueryServicesMock.Verify(e => e.GetEncountersBySport(sportName), Times.AtMostOnce);
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(encounter.Id, obtainedResult.Value[0].Id);
            Assert.AreEqual(encounter.SportName, obtainedResult.Value[0].SportName);
            Assert.AreEqual(encounter.TeamIds.First(), obtainedResult.Value[0].TeamIds.First());
            Assert.AreEqual(encounter.TeamIds.Last(), obtainedResult.Value[0].TeamIds.Last());
            Assert.AreEqual(encounter.DateTime, obtainedResult.Value[0].DateTime);
        }

        private EncounterDTO CreateAEncounter(string sportId)
        {
            return new EncounterDTO() { SportName = sportId, TeamIds = new List<string>(){"Manchester","UsAtlanta" }};
        }

        [TestMethod]
        public void GetAllEncountersOfASpecificSport2()
        {

            string sportName = "Tennis";
            var expectedEncounters = new List<EncounterDTO>() { };
            
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            
            encounterQueryServicesMock.Setup(s => s.GetEncountersBySport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncountersBySport("Tennis") as ActionResult<List<EncounterModelOut>>;
            var value = obtainedResult.Result;

            encounterServicesMock.VerifyAll();
            Assert.IsNull(value);
        }
        
        [TestMethod]
        public void GetSportsByNameOkSportsController()
        {
            
            sportServicesMock.Setup(s => s.GetSport(football.Name)).Returns(football);
            
            

            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServices.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetSportById(football.Name) as ActionResult<SportModelOut>;
            var val = obtainedResult.Value;

            sportServicesMock.Verify(s => s.GetSport(football.Name), Times.AtMostOnce);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            SportModelOut sport = obtainedResult.Value;
            Assert.AreEqual(football.Name, sport.Name);
        }

        [TestMethod]
        public void GetEncountersBySportOkSportsController()
        {
            
            
            var encounterQueryServicesMock = new Mock<IEncounterQueryServices>();
            EncounterDTO encounter = CreateAEncounter(football.Name);
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterQueryServicesMock.Setup(e => e.GetEncountersBySport(football.Name)).Returns(encounters);

            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object, encounterQueryServicesMock.Object, positionServicesMock.Object, teamServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncountersBySport(football.Name) as ActionResult<List<EncounterModelOut>>;
            encounterQueryServicesMock.Verify(e => e.GetEncountersBySport(football.Name), Times.AtMostOnce);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            List<EncounterModelOut> encounterResult = obtainedResult.Value;
            Assert.AreEqual(encounter.Id, encounterResult[0].Id);
            Assert.AreEqual(encounter.SportName, encounterResult[0].SportName);
            Assert.AreEqual(encounter.TeamIds.First(), encounterResult[0].TeamIds.First());
            Assert.AreEqual(encounter.TeamIds.Last(), encounterResult[0].TeamIds.Last());
            Assert.AreEqual(encounter.DateTime, encounterResult[0].DateTime);
        }
    }
}
