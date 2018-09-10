using Microsoft.EntityFrameworkCore;
using EirinDuran.Entities;
using System;

namespace EirinDuran.DataAccess
{
    public interface IContext
    {
        DbSet<UserEntity> UserEntities { get; set; }

        int SaveChanges();
    }
}