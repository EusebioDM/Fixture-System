
using Microsoft.EntityFrameworkCore;
using System;
using Entities;

namespace EirinDuran.DataAccess
{
        public class Context: DbContext 
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }
        
        public DbSet<UserEntity> UserEntities { get; set; }  
    }
}
