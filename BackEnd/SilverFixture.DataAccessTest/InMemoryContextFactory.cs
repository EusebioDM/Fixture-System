﻿using SilverFixture.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace SilverFixture.DataAccessTest
{
    public class InMemoryContextFactory : IDesignTimeDbContextFactory<Context>
    {
        private Guid guid;

        public InMemoryContextFactory()
        {
            guid = Guid.NewGuid();
        }

        public Context CreateDbContext(string[] args)
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(guid.ToString())
                .EnableSensitiveDataLogging()
                .UseLazyLoadingProxies()
                .Options;
            return new Context(options);
        }
    }
}