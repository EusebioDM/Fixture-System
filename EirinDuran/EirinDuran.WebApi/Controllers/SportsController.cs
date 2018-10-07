using System.Collections.Generic;
using System.Linq;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using EirinDuran.IServices.Exceptions;
using EirinDuran.WebApi.Models;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly ILoginServices loginServices;
        private readonly ISportServices sportServices;
        private readonly IEncounterServices encounterServices;

        public SportsController(ILoginServices loginServices, ISportServices sportServices, IEncounterServices encounterServices)
        {
            this.loginServices = loginServices;
            this.sportServices = sportServices;
            this.encounterServices = encounterServices;
        }

        [HttpGet]
        public ActionResult<List<SportDTO>> Get()
        {
            try
            {
                return sportServices.GetAllSports().ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<SportDTO> GetById(string sportId)
        {
            try
            {
                return sportServices.GetSport(sportId);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{sportId}/encounters")]
        public ActionResult<List<EncounterDTO>> GetEncounters(string sportId)
        {
            try
            {
                return encounterServices.GetEncountersBySport(sportId).ToList();
            }
            catch (ServicesException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(SportDTO sport)
        {
            CreateSession();
            try
            {
                return TryToCreate(sport);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToCreate(SportDTO sport)
        {
            try
            {
                sportServices.CreateSport(sport);
                return CreatedAtRoute("GetSport", new { id = sport.Name }, sport);
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{sportName}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string sportName)
        {
            CreateSession();
            try
            {
                return TryToDelete(sportName);
            }
            catch (InsufficientPermissionException)
            {
                return Unauthorized();
            }
        }

        private IActionResult TryToDelete(string sportId)
        {
            try
            {
                sportServices.DeleteSport(sportId);
                return Ok();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<EncounterDTO>> CreateFixture(FixtureModelIn fixture)
        {
            throw new System.NotImplementedException();
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
