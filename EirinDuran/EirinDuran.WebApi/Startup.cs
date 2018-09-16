using EirinDuran.DataAccess;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EirinDuran.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IContextFactory>(provider => provider.GetService<ContextFactory>());
            services.AddScoped<IContextFactory, ContextFactory>();
            services.AddScoped<DbContextOptions<Context>>(CreateDbContextOptions);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private DbContextOptions<Context> CreateDbContextOptions(IServiceProvider serviceProvider)
        {
            DbContextOptionsBuilder<Context> builder = new DbContextOptionsBuilder<Context>().UseSqlServer(Configuration.GetConnectionString("EirinDuranDB"),
                                                                                       b => b.MigrationsAssembly("EirinDuran.WebApi")).UseLazyLoadingProxies();
            return builder.Options;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}
