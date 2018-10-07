using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EirinDuran.DataAccess;
using EirinDuran.IServices.DTOs;
using EirinDuran.IServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestingController : Controller
    {
        private readonly IDesignTimeDbContextFactory<Context> contextFactory;
        private readonly IUserServices userServices;
        private const string ResetDataBaseConfirmationKey = "ConfirmDeleteDataBase";
        private readonly UserDTO InitialUser = new UserDTO()
        {
            UserName = "Admin",
            Name = "Admin",
            Password = "Admin",
            IsAdmin = true,
            Surname = "Admin",
            Mail = "Admin@admin.com"
        };

        public TestingController(IDesignTimeDbContextFactory<Context> contextFactory, IUserServices userServices)
        {
            this.contextFactory = contextFactory;
            this.userServices = userServices;
        }

        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            if (value == ResetDataBaseConfirmationKey)
            {
                using (Context context = contextFactory.CreateDbContext(new string[0]))
                {
                    context.DeleteDataBase();
                    userServices.CreateUser(InitialUser);
                }

                return Ok();
            }
            else
                return Unauthorized();
        }
    }
}
