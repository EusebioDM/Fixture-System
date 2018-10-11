using EirinDuran.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EirinDuran.WebApi
{
    public class DbContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            DbContextOptionsBuilder<Context> builder = new DbContextOptionsBuilder<Context>();
            string connectionString = configuration.GetConnectionString("EirinDuranDB");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("EirinDuran.WebApi"));
            builder.UseLazyLoadingProxies();
            return new Context(builder.Options);
        }
    }
}
