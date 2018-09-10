
using EirinDuran.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EirinDuran.DataAccess
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<UserEntity> UserEntities { get; set; }
    }
}
