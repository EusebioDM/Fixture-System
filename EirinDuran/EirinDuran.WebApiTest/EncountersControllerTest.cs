using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IServices;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using EirinDuran.WebApi.Controllers;
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
        private Team river;
        private Team boca;
        private Sport football;

        [TestInitialize]
        public void SetUp()
        {
            football = new Sport("Futbol");
        }

        [TestMethod]
        public void GetAllEncountersOkEncountersController()
        {
            var enconunterServices = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            });

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            List<Team> teams = new List<Team>() { river, boca };
            DateTime encounterDate = new DateTime(2018, 12, 10);

            EncounterDTO enc = new EncounterDTO();
            enc.SportName = football.Name;
            enc.AwayTeamName = teams[0].Name;
            enc.AwayTeamName = teams[1].Name;
            enc.DateTime = encounterDate;

            List<EncounterDTO> encs = new List<EncounterDTO>() { enc };
            enconunterServices.Setup(m => m.GetAllEncounters()).Returns(encs);

            var controller = new EncountersController(loginServices, enconunterServices.Object) { ControllerContext = controllerContext, };

            var obtainedResult = controller.Get() as ActionResult<List<EncounterDTO>>;
            enconunterServices.Verify(m => m.GetAllEncounters(), Times.AtMostOnce());

            bool areEqual = obtainedResult.Value.ToList().All(encs.Contains);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void AddCommentToEncounter()
        {
            var enconunterServices = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(new UserDTO()
            {
                UserName = "Macri",
                Name = "Mauricio",
                Surname = "Macri",
                Password = "cat123",
                Mail = "mail@gmail.com",
                IsAdmin = true
            });

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

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

        public Guid IntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}





