
using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace EirinDuran.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<SportEntity> Sports { get; set; }
        public DbSet<EncounterEntity> Encounters { get; set; }

        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
