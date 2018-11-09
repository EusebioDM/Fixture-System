using EirinDuran.DataAccess;
using EirinDuran.DataAccess.Entities;
using EirinDuran.IServices.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EirinDuran.IServices.Services_Interfaces;

namespace EirinDuran.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestingController : Controller
    {
        private readonly IDesignTimeDbContextFactory<Context> contextFactory;
        private readonly IUserServices userServices;
        private const string ResetDataBaseConfirmationKey = "ConfirmDeleteDataBase";
        private readonly UserEntity InitialUser = new UserEntity()
        {
            UserName = "Admin",
            Name = "Admin",
            Password = "Admin",
            Role = Domain.User.Role.Administrator,
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
                }
                using (Context context = contextFactory.CreateDbContext(new string[0]))
                {
                    context.Users.Add(InitialUser);
                    context.SaveChanges();
                }

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
