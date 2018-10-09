using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncountersController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly IEncounterServices encounterServices;

        public EncountersController(ILoginServices loginServices, IEncounterServices encounterServices)
        {
            this.encounterServices = encounterServices;
            this.loginServices = loginServices;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<EncounterModelOut>> Get([FromQuery] DateTime start, [FromQuery] DateTime end)
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
                return encounterServices.GetAllEncounters().Select(e => new EncounterModelOut(e)).ToList();
            }
            else
            {
                return encounterServices.GetEncountersByDate(start, end).Select(e => new EncounterModelOut(e)).ToList();
            }
        }

        [HttpGet("{id}", Name = "GetEncounter")]
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<EncounterModelOut> GetById(string id)
        {
            try
            {
                CreateSession();
                return new EncounterModelOut(encounterServices.GetEncounter(id));

            }
            catch(ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(EncounterModelIn encounter)
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
            EncounterDTO createdEncounter = encounterServices.CreateEncounter(encounter.ToServicesDTO());
            return CreatedAtRoute("GetEncounter", new { id = createdEncounter.Id }, createdEncounter);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] EncounterModelIn encounterModel)
        {
            try
            {
                CreateSession();
                return TryToPut(id, encounterModel.ToServicesDTO());
            }
            catch(InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToPut(string id, EncounterDTO encounterModel)
        {
            try
            {
                encounterServices.UpdateEncounter(encounterModel);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
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
            encounterServices.DeleteEncounter(id);
            return Ok();
        }

        [HttpGet]
        [Route("{encounterId}/commentaries")]
        [Authorize]
        public ActionResult<IEnumerable<CommentDTO>> GetEncounterComments(string encounterId)
        {
            CreateSession();
            try
            {
                return encounterServices.GetAllCommentsToOneEncounter(encounterId).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("{encounterId}/commentaries")]
        [Authorize]
        public IActionResult AddComment(string encounterId, [FromBody] string menssage)
        {
            try
            {
                CreateSession();
                encounterServices.AddComment(encounterId, menssage);
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
            return encounterServices.GetAvailableFixtureGenerators().ToList();
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
                var encounters = encounterServices.CreateFixture(fixtureModelIn.CreationAlgorithmName, fixtureModelIn.SportName, fixtureModelIn.StartingDate);
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
