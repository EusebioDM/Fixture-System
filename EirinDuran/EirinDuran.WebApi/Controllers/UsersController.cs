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
        private readonly IRepository<User> userRepository;
        private readonly UserServices userServices;

        public UsersController(/*UserServices userServices*/IRepository<User> userRepository)
        {
            //this.userServices = userServices;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() {
            return userRepository.GetAll().ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<User> GetById(string id)
        {
            User user = userRepository.Get(new User(Role.Follower,id, "NOTSET","NOTSET","NOTSET","NOTSET@NOTSE.COM"));
            if(user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public IActionResult Create(User item)
        {
            userRepository.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Name }, item);
        }
    }
}
