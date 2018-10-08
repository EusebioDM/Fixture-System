using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Interfaces;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        [Authorize(Roles = "Administrator, Follower")]
        public ActionResult<List<SportModelOut>> GetAll()
        {
            try
            {
                return sportServices.GetAllSports().Select(s => new SportModelOut(s)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{sportId}")]
        [Authorize(Roles = "Administrator, Follower")]
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
        [Authorize]
        public ActionResult<List<EncounterModelOut>> GetEncounters(string sportId)
        {
            try
            {
                return encounterServices.GetEncountersBySport(sportId).Select(e => new EncounterModelOut(e)).ToList();
            }
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToCreate(SportDTO sport)
        {
            sportServices.CreateSport(sport);
            return CreatedAtRoute("GetSport", new { id = sport.Name }, sport);
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
            catch (ServicesException e)
            {
                return BadRequest(e.Message);
            }
        }

        private IActionResult TryToDelete(string sportId)
        {
            sportServices.DeleteSport(sportId);
            return Ok();
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
