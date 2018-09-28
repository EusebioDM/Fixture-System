using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using EirinDuran.Domain.User;
using Moq;
using EirinDuran.WebApi.Controllers;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using EirinDuran.Services;
using EirinDuran.IServices;
using EirinDuran.Domain.Fixture;

namespace EirinDuran.WebApiTest
{
    [TestClass]
    public class EnconuntersControllerTest
    {
        [TestMethod]
        public void GetAllEncountersOkEncountersController()
        {
            var enconunterServices = new Mock<EncounterServices>();

            ILoginServices loginServices = new LoginServicesMock(new User(Role.Administrator, "Macri", "Mauricio", "Macri", "cat123", "mail@gmail.com"));

            IEnumerable<Team> teams = new List<Team>() { new Team("River"), new Team("Boca") };
            DateTime encounterDate = new DateTime(2018, 12, 10);

            Encounter enc = new Encounter(new Sport("Futbol"), teams, encounterDate);
            List<Encounter> encs = new List<Encounter>() { enc };
            enconunterServices.Setup(m => m.GetAllEncounters()).Returns(encs);

            var controller = new EnconuntersController(loginServices, EncounterServices.Object);
            IEnumerable<Encounter> recovered = controller.GetAllEncounters();

            var obtainedResult = controller.GetAllEncounters() as ActionResult<User>;

            enconunterServices.Verify(m => m.GetAllEncounters(), Times.AtMostOnce());
            Assert.IsNotNull(obtainedResult);
            Assert.IsNotNull(obtainedResult.Value);
            Assert.AreEqual(obtainedResult.Value, encs);
        }
    }
}
