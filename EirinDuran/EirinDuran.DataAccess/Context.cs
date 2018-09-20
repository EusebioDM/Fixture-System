using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EirinDuran.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<SportEntity> Sports { get; set; }
        public DbSet<EncounterEntity> Encounters { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>().HasKey(u => u.UserName);
            builder.Entity<TeamEntity>().HasKey(t => t.Name);
            builder.Entity<SportEntity>().HasKey(s => s.Name);
            builder.Entity<EncounterEntity>().HasKey(e => e.Id);
            builder.Entity<CommentEntity>().HasKey(e => e.Id);
        }

    }
}
