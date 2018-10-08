using System.Collections.Generic;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EirinDuran.IServices.Exceptions;
using EirinDuran.WebApi.Models;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class SportControllerTest
    {
        private UserDTO mariano;
        private UserDTO rodolfo;
        private SportDTO football;
        private SportDTO tennis;

        [TestInitialize]
        public void SetUp()
        {
            football = new SportDTO() { Name = "Futbol" };
            tennis = new SportDTO() { Name = "Tenis" };

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
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>()));
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.Create(footballIn);
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
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>())).Throws(new InsufficientPermissionException());
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices login = new LoginServicesMock(rodolfo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.Create(footballIn);
            sportServicesMock.Verify(s => s.CreateSport(It.IsAny<SportDTO>()), Times.AtMostOnce);
            var createdResult = result as UnauthorizedResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(401, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateSportAlreadyExistsSportsController()
        {
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.CreateSport(It.IsAny<SportDTO>())).Throws(new ServicesException());
            var encounterServicesMock = new Mock<IEncounterServices>();
            ILoginServices login = new LoginServicesMock(rodolfo);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };
            var result = controller.Create(footballIn);
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

            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.DeleteSport(name));
            var encounterServicesMock = new Mock<IEncounterServices>();
            
            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Delete(name);
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

            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.DeleteSport(name)).Throws(new ServicesException());
            var encounterServicesMock = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Delete(name);
            sportServicesMock.Verify(s => s.DeleteSport(name), Times.AtMostOnce);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public void DeleteSportWithoutPermissionSportsController()
        {
            string name = "Futbol";
            var modelIn = new SportDTO() { Name = name };

            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.DeleteSport(name)).Throws(new InsufficientPermissionException());
            var encounterServicesMock = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(loginServices, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Delete(name);
            sportServicesMock.Verify(s => s.DeleteSport(name), Times.AtMostOnce);
            var badRequestResult = result as UnauthorizedResult;

            Assert.AreEqual(401, badRequestResult.StatusCode);
        }

        [TestMethod]
        public void GetAllSportsOkSportsController()
        {
            var expectedSports = new List<SportDTO>() { football, tennis };
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.GetAllSports()).Returns(expectedSports);
            var encounterServicesMock = new Mock<IEncounterServices>();
            
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetAll() as ActionResult<List<SportModelOut>>;
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
            var encounterServicesMock = new Mock<IEncounterServices>();
            var sportServicesMock = new Mock<ISportServices>();
            encounterServicesMock.Setup(s => s.GetEncountersBySport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncounters(sportName) as ActionResult<List<EncounterModelOut>>;
            var val = obtainedResult.Value;

            encounterServicesMock.Verify(e => e.GetEncountersBySport(sportName), Times.AtMostOnce);
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(encounter.Id, obtainedResult.Value[0].Id);
            Assert.AreEqual(encounter.SportName, obtainedResult.Value[0].SportName);
            Assert.AreEqual(encounter.HomeTeamName, obtainedResult.Value[0].HomeTeamName);
            Assert.AreEqual(encounter.AwayTeamName, obtainedResult.Value[0].AwayTeamName);
            Assert.AreEqual(encounter.DateTime, obtainedResult.Value[0].DateTime);
        }

        private EncounterDTO CreateAEncounter(string sportId)
        {
            return new EncounterDTO() { SportName = sportId, AwayTeamName = "Manchester", HomeTeamName = "UsAtlanta" };
        }

        [TestMethod]
        public void GetAllEncountersOfASpecificSport2()
        {

            string sportName = "Tennis";
            var expectedEncounters = new List<EncounterDTO>() { };
            var encounterServicesMock = new Mock<IEncounterServices>();
            var sportServicesMock = new Mock<ISportServices>();
            encounterServicesMock.Setup(s => s.GetEncountersBySport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncounters("Tennis") as ActionResult<List<EncounterModelOut>>;
            var value = obtainedResult.Result;

            encounterServicesMock.VerifyAll();
            Assert.IsNull(value);
        }
        
        [TestMethod]
        public void GetSportsByNameOkSportsController()
        {
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.GetSport(football.Name)).Returns(football);
            var encounterServicesMock = new Mock<IEncounterServices>();

            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetById(football.Name) as ActionResult<SportModelOut>;
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
            var sportServicesMock = new Mock<ISportServices>();
            var encounterServicesMock = new Mock<IEncounterServices>();
            EncounterDTO encounter = CreateAEncounter(football.Name);
            List<EncounterDTO> encounters = new List<EncounterDTO>() { encounter };
            encounterServicesMock.Setup(e => e.GetEncountersBySport(football.Name)).Returns(encounters);

            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "";
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var controller = new SportsController(login, sportServicesMock.Object, encounterServicesMock.Object)
            {
                ControllerContext = controllerContext,
            };

            var obtainedResult = controller.GetEncounters(football.Name) as ActionResult<List<EncounterModelOut>>;
            encounterServicesMock.Verify(e => e.GetEncountersBySport(football.Name), Times.AtMostOnce);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            List<EncounterModelOut> encounterResult = obtainedResult.Value;
            Assert.AreEqual(encounter.Id, encounterResult[0].Id);
            Assert.AreEqual(encounter.SportName, encounterResult[0].SportName);
            Assert.AreEqual(encounter.HomeTeamName, encounterResult[0].HomeTeamName);
            Assert.AreEqual(encounter.AwayTeamName, encounterResult[0].AwayTeamName);
            Assert.AreEqual(encounter.DateTime, encounterResult[0].DateTime);
        }
    }
}
