using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.User;
using EirinDuran.Services;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices userServices;

        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() {
            return userServices.GetAllUsers().ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(string id)
        {
            User user = userServices.GetUser(new User(Role.Follower, id, "NOTSET", "NOTSET", "NOTSET", "NOTSET@NOTSE.COM"));
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            userServices.AddUser(user);
            return CreatedAtRoute("GetUser", new { id = user.Name }, user);
        }
    }
}
