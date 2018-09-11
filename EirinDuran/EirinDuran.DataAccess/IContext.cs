using EirinDuran.Domain.Fixture;
using EirinDuran.Entities;
using Microsoft.EntityFrameworkCore;
using System;


namespace EirinDuran.DataAccess
{
    public interface IContext
    {
        DbSet<UserEntity> Users { get; set; }

        DbSet<TeamEntity> Teams { get; set; }

        DbSet<SportEntity> Sports { get; set; }
        
        DbSet<EncounterEntity> Encounters { get; set; }

        int SaveChanges();
    }
}
