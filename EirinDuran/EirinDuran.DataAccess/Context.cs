using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EirinDuran.DataAccess.Entities;

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
            builder.Entity<EncounterEntity>().HasMany(e => e.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EncounterEntity>().HasOne(e => e.Sport).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CommentEntity>().HasKey(e => e.Id);
            builder.Entity<CommentEntity>().HasOne(c => c.User).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<TeamUserEntity>().HasKey(tu => new { tu.TeamName, tu.UserNamee });
            builder.Entity<TeamUserEntity>().HasOne(tu => tu.User).WithMany(u => u.TeamUsers);
            builder.Entity<TeamUserEntity>().HasOne(t => t.Team).WithOne().HasForeignKey<TeamUserEntity>(new string[] { "TeamName", "SportName" }).OnDelete(DeleteBehavior.Cascade);
        }

        public void DeleteDataBase()
        {
            base.Database.EnsureDeleted();
        }

    }
}
