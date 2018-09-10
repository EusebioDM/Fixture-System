﻿using Microsoft.EntityFrameworkCore;
using System;
using Entities;

namespace EirinDuran.DataAccess
{
        public class Context: DbContext, IContext 
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }
         protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<UserEntity> UserEntities { get; set; }  
    }
}
