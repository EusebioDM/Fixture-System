using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EirinDuran.Domain.User;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.Services;
using EirinDuran.IServices;
using EirinDuran.Domain.Fixture;
using EirinDuran.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
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
            river = new Team("River");
            boca = new Team("Boca");

            football = new Sport("Futbol");
            football.AddTeam(river);
            football.AddTeam(boca);
        }

        [TestMethod]
        public void GetAllEncountersOkEncountersController()
        {
            var enconunterServices = new Mock<IEncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwiVXNlck5hbWUiOiJGcmFuY28iLCJQYXNzd29yZCI6InVzZXIiLCJleHAiOjE1Mzc5MTkxNTEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.t2Tm_mvehwOv20p8Wc1yFUeBa2yS-jfYKfiurNLawhc";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            IEnumerable<Team> teams = new List<Team>() { river, boca };
            DateTime encounterDate = new DateTime(2018, 12, 10);

            Encounter enc = new Encounter(football, teams, encounterDate);
            List<Encounter> encs = new List<Encounter>() { enc };
            enconunterServices.Setup(m => m.GetAllEncounters()).Returns(encs);

            var controller = new EncountersController(loginServices, enconunterServices.Object) { ControllerContext = controllerContext, }; 

            var obtainedResult = controller.Get() as ActionResult<List<Encounter>>;
            enconunterServices.Verify(m => m.GetAllEncounters(), Times.AtMostOnce());

            bool areEqual = obtainedResult.Value.ToList().All(encs.Contains);

            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.IsTrue(areEqual);
        }
    }
}
