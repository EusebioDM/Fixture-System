using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EirinDuran.Domain.Fixture;
using EirinDuran.IServices.Infrastructure_Interfaces;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncountersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IEncounterSimpleServices encounterSimpleServices;
        private readonly IEncounterQueryServices encounterQueryServices;
        private readonly IFixtureServices fixtureServices;
        private readonly ITeamServices teamServices;
        private readonly ILogger logger;

        public EncountersController(ILoginServices loginServices, IEncounterSimpleServices encounterSimpleServices, ILogger logger, IEncounterQueryServices encounterQueryServices, IFixtureServices fixtureServices, ITeamServices teamServices)
        {
            this.encounterSimpleServices = encounterSimpleServices;
            this.logger = logger;
            this.encounterQueryServices = encounterQueryServices;
            this.fixtureServices = fixtureServices;
            this.teamServices = teamServices;
            this.loginServices = loginServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<EncounterModelOut>> GetAllEncounters([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                CreateSession();
                return TryToGetAllEncounters(start, end);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private ActionResult<List<EncounterModelOut>> TryToGetAllEncounters(DateTime start, DateTime end)
        {
            if (start.Equals(new DateTime()) || end.Equals(new DateTime()))
            {
                return encounterSimpleServices.GetAllEncounters().Select(e => new EncounterModelOut(e)).ToList();
            }
            else
            {
                return encounterQueryServices.GetEncountersByDate(start, end).Select(e => new EncounterModelOut(e)).ToList();
            }
        }

        [HttpGet("{id}", Name = "GetEncounter")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<EncounterModelOut> GetEncounterById(string id)
        {
            try
            {
                CreateSession();
                return new EncounterModelOut(encounterSimpleServices.GetEncounter(id));

            }
            catch(ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult CreateEncounter(EncounterModelIn encounter)
        {
            CreateSession();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return TryToAddEncounter(encounter);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToAddEncounter(EncounterModelIn encounter)
        {
            EncounterDTO createdEncounter = encounterSimpleServices.CreateEncounter(encounter.ToServicesDTO());
            return CreatedAtRoute("GetEncounter", new { id = createdEncounter.Id }, createdEncounter);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult ModifyEncounter(string id, [FromBody] EncounterUpdateModelIn encounterModel)
        {
            try
            {
                CreateSession();
                encounterModel.Id = Guid.Parse(id);
                return TryToPut(id, encounterModel);
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToPut(string id, EncounterUpdateModelIn encounterModel)
        {
            try
            {
                EncounterDTO toUpdate = GetUpdatedEncounter(encounterModel);
                encounterSimpleServices.UpdateEncounter(toUpdate);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private EncounterDTO GetUpdatedEncounter(EncounterUpdateModelIn encounterModel)
        {
            EncounterDTO toUpdate = encounterSimpleServices.GetEncounter(encounterModel.Id.ToString());
            encounterModel.UpdateServicesDTO(toUpdate, teamServices);
            return toUpdate;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteEncounter(string id)
        {
            try
            {
                CreateSession();
                return TryToDelete(id);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToDelete(string id)
        {
            encounterSimpleServices.DeleteEncounter(id);
            return Ok();
        }

        [HttpGet]
        [Route("{encounterId}/commentaries")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<CommentDTO>> GetEncounterComments(string encounterId)
        {
            CreateSession();
            try
            {
                return encounterQueryServices.GetAllCommentsToOneEncounter(encounterId).OrderByDescending(c => c.TimeStamp).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("{encounterId}/commentaries")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AddEncounterComment(string encounterId, [FromBody] string menssage)
        {
            try
            {
                CreateSession();
                encounterSimpleServices.AddComment(encounterId, menssage);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("fixture")]
        [Authorize]
        public ActionResult<List<string>> GetAvailableFixtureGenerators()
        {
            CreateSession();
            return fixtureServices.GetAvailableFixtureGenerators().ToList();
        }

        [HttpPost]
        [Route("fixture")]
        [Authorize]
        public IActionResult CreateFixture(FixtureModelIn fixtureModelIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                CreateSession();
                var encounters = fixtureServices.CreateFixture(fixtureModelIn.CreationAlgorithmName, fixtureModelIn.SportName, fixtureModelIn.StartingDate);
                logger.Log(loginServices.LoggedUser.UserName, $"Created a fixture using the {fixtureModelIn.CreationAlgorithmName} generator");
                return Ok(encounters);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private void CreateSession()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            loginServices.CreateSession(userName, password);
        }
    }
}
