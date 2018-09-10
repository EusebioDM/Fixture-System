using Microsoft.EntityFrameworkCore;
using EirinDuran.Entities;
using System;

namespace EirinDuran.DataAccess
{
    public class Context: DbContext, IContext 
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public int SaveChanges()
        {
            return 0;
        }

        public DbSet<UserEntity> UserEntities { get; set; }
    }
}
