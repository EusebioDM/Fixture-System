using System.Collections.Generic;
using System.Linq;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class SportControllerTest
    {
        private UserDTO mariano;
        private SportDTO football;
        private SportDTO tennis;

        [TestInitialize]
        public void SetUp()
        {
            football = new SportDTO() { Name = "Futbol" };
            tennis = new SportDTO() { Name = "Tenis" };

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
        public void GetAllSportsOkSportsController()
        {
            var expectedSports = new List<SportDTO>() { football, tennis };
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.GetAllSports()).Returns(expectedSports);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new SportsController(login, sportServicesMock.Object) { ControllerContext = controllerContext, };


            var obtainedResult = controller.Get() as ActionResult<List<SportDTO>>;
            var val = obtainedResult.Value;

            sportServicesMock.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);

            IEnumerable<SportDTO> sportList = obtainedResult.Value.ToList().Union(expectedSports.ToList());

            Assert.IsTrue(sportList.ToList().Count == 2);
        }

        [TestMethod]
        public void GetAllEncountersOfASpecificSport1()
        {
            string sportName = "Futbol";
            EncounterDTO encounter = CreateAEncounter(sportName);
            var expectedEncounters = new List<EncounterDTO>() { encounter };
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.GetAllEncountersOfASpecificSport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new SportsController(login, sportServicesMock.Object) { ControllerContext = controllerContext, };


            var obtainedResult = controller.GetEncounters(sportName) as ActionResult<List<EncounterDTO>>;
            var val = obtainedResult.Value;

            sportServicesMock.VerifyAll();
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);

            IEnumerable<EncounterDTO> sportList = obtainedResult.Value.ToList().Union(expectedEncounters.ToList());

            Assert.IsTrue(sportList.ToList().Count == 1);
        }

        [TestMethod]
        public void GetAllEncountersOfASpecificSport2()
        {

            string sportName = "Tennis";
            var expectedEncounters = new List<EncounterDTO>() {  };
            var sportServicesMock = new Mock<ISportServices>();
            sportServicesMock.Setup(s => s.GetAllEncountersOfASpecificSport(sportName)).Returns(expectedEncounters);
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new SportsController(login, sportServicesMock.Object) { ControllerContext = controllerContext, };


            var obtainedResult = controller.GetEncounters("Tennis") as ActionResult<List<EncounterDTO>>;
            var val = obtainedResult.Result;

            sportServicesMock.VerifyAll();
            Assert.IsNull(val);
        }


        private EncounterDTO CreateAEncounter(string sportId)
        {
            return new EncounterDTO() { SportName = sportId, AwayTeamName = "Manchester", HomeTeamName = "UsAtlanta" };
        }

        [TestMethod]
        public void CreateSportOkSportsController()
        {
            var sportServicesMock = new Mock<ISportServices>();
            ILoginServices login = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new SportsController(login, sportServicesMock.Object) { ControllerContext = controllerContext, };

            SportDTO footballIn = new SportDTO() { Name = "Futbol" };

            var result = controller.Create(footballIn);
            var createdResult = result as CreatedAtRouteResult;
            var footballOut = createdResult.Value as SportDTO;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetSport", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(footballIn.Name, footballOut.Name);
        }

        [TestMethod]
        public void DeleteSportOkSportsController()
        {
            var modelIn = new SportDTO() { Name = "Tennis" };

            var mockSportServices = new Mock<ISportServices>();

            mockSportServices.Setup(s => s.DeleteSport("Tennis"));

            ILoginServices loginServices = new LoginServicesMock(mariano);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new SportsController(loginServices, mockSportServices.Object) { ControllerContext = controllerContext, };

            var result = controller.Delete("Tennis");

            var createdResult = result as OkResult;

            Assert.IsNotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }
    }
}
