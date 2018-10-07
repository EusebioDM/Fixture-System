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
        public ActionResult<List<EncounterDTO>> Get([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            CreateSession();
            try
            {
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

        private ActionResult<List<EncounterDTO>> TryToGetAllEncounters(DateTime start, DateTime end)
        {
            if (start.Equals(new DateTime()) || end.Equals(new DateTime()))
            {
                return encounterServices.GetAllEncounters().ToList();
            }
            else
            {
                return encounterServices.GetEncountersByDate(start, end).ToList();
            }
        }

        [HttpGet("{id}", Name = "GetEncounter")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<EncounterDTO> GetById(string id)
        {
            CreateSession();
            return BadRequest();
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
            encounterServices.CreateEncounter(encounter.ToServicesDTO());
            return CreatedAtRoute("GetEncounter", new { id = encounter.Id }, encounter);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(string id, [FromBody] EncounterModelIn encounterModel)
        {
            CreateSession();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            CreateSession();
            try
            {
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
            CreateSession();
            try
            {
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
            CreateSession();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var encounters = encounterServices.CreateFixture(fixtureModelIn.CreationAlgorithmName, fixtureModelIn.SportName, fixtureModelIn.StartingDate);
                return Ok(encounters);
            }
            catch (ServicesException ex)
            {
                return BadRequest(ex.Message);
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
