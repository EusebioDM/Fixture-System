using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EirinDuran.IDataAccess;
using EirinDuran.Domain.User;
using EirinDuran.Services;
using EirinDuran.WebApi.Models;

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
        public ActionResult<List<User>> Get()
        {
            return userServices.GetAllUsers().ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(string id)
        {
            User user = userServices.GetUser(new User(id));
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public IActionResult Create(UserModelIn userModel)
        {
            if (ModelState.IsValid)
            {
                //Poner una fábrica acá
                User user = new User(userModel.Role, userModel.UserName, userModel.Name, userModel.Surname, userModel.Password, userModel.Mail);
                userServices.AddUser(user);

                var addedUser = new UserModelOut() { UserName = user.UserName, Name = user.Name, Surname = user.Surname, Mail = user.Mail, Role = user.Role };
                return CreatedAtRoute("GetUserName", new { id = addedUser.UserName }, addedUser);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
