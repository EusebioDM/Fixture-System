using EirinDuran.Domain.Fixture;
using EirinDuran.DataAccess;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EirinDuran.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace EirinDuran.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<SportEntity> Sports { get; set; }
        public DbSet<EncounterEntity> Encounters { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LogEntity>().HasKey(l => l.Id);
            builder.Entity<UserEntity>().HasKey(u => u.UserName);
            builder.Entity<TeamEntity>().HasKey(t => new {t.Name, t.SportName});
            builder.Entity<SportEntity>().HasKey(s => s.SportName);
            builder.Entity<EncounterEntity>().HasKey(e => e.Id);
            builder.Entity<EncounterEntity>().HasMany(e => e.Teams).WithOne();
            builder.Entity<EncounterEntity>().HasMany(e => e.Comments).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EncounterEntity>().HasOne(e => e.Sport).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CommentEntity>().HasKey(e => e.Id);
            builder.Entity<CommentEntity>().HasOne(c => c.User).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<TeamUser>().HasKey(tu => new {tu.TeamName, tu.SportName, tu.UserName});
            builder.Entity<TeamUser>().HasOne(tu => tu.User).WithMany(u => u.TeamUsers);
            //builder.Entity<TeamUser>().HasOne(t => t.Team).WithMany().HasForeignKey<TeamUser>(new string[] { "TeamName", "SportName" }).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<TeamUser>().HasOne(t => t.Team).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<EncounterTeam>().HasOne(et => et.Encounter).WithMany(e => e.Teams);
            builder.Entity<EncounterTeam>().HasKey("TeamName", "SportName", "EncounterId");
            builder.Entity<EncounterTeam>().HasOne(et => et.Team).WithMany().HasForeignKey("TeamNameFk", "SportNameFk").OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<TeamResult>().HasKey("TeamId", "EncounterId");
            builder.Entity<TeamResult>().HasOne(tr => tr.Team).WithMany().OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<EncounterEntity>().HasMany(e => e.Results).WithOne().OnDelete(DeleteBehavior.Cascade);
        }

        public void DeleteDataBase()
        {
            base.Database.EnsureDeleted();
        }
    }
}