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

        public DbSet<TeamUserEntity> TeamUsers { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>().HasKey(u => u.UserName);
            builder.Entity<TeamEntity>().HasKey(t => new { t.Name, t.SportName });
            builder.Entity<SportEntity>().HasKey(s => s.SportName);
            builder.Entity<EncounterEntity>().HasKey(e => e.Id);
            builder.Entity<CommentEntity>().HasKey(e => e.Id);
            builder.Entity<TeamUserEntity>().HasKey(tu => new { tu.TeamName, tu.UserName });

            // builder.Entity<TeamUserEntity>()
            //     .HasOne(tu => tu.User)
            //     .WithMany(u => u.TeamUsers)
            //     .HasForeignKey(tu => tu.UserName);

            //            builder.Entity<TeamUserEntity>()
            //                .HasOne(tu => tu.Team)
            //                .WithMany(t => t.TeamUsers)
            //                .HasForeignKey(tu => tu.TeamName);
        }

    }
}
