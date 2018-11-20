using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Exceptions;
using EirinDuran.IServices.Services_Interfaces;
using EirinDuran.Services;
using EirinDuran.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private ILoginServices login;
        private ILoggerServices logger;

        public LogController(ILoginServices login, ILoggerServices logger)
        {
            this.login = login;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<LogDTO>> GetAllLogs([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                CreateSession();
                return TryToGetAllLogs(start, end);
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

        private ActionResult<List<LogDTO>> TryToGetAllLogs(DateTime start, DateTime end)
        {
            return logger.GetLogs(start, end).OrderByDescending(dto => dto.DateTime).ToList();
        }

        private void CreateSession()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();

            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();

            login.CreateSession(userName, password);
        }
    }
}