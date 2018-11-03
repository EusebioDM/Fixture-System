using EirinDuran.DataAccess;
using EirinDuran.Domain.Fixture;
using EirinDuran.Domain.User;
using EirinDuran.IDataAccess;
using EirinDuran.IServices.Interfaces;
using EirinDuran.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EirinDuran.DataAccess.Entities;
using EirinDuran.WebApi.Controllers;

namespace EirinDuran.WebApi
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
            services.AddScoped<IDesignTimeDbContextFactory<Context>, DbContextFactory>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Sport>, SportRepository>();
            services.AddScoped<IRepository<Team>, TeamRepository>();
            services.AddScoped<IRepository<Encounter>, EncounterRepository>();
            services.AddScoped<IRepository<Log>, LogRepository>();
            services.AddScoped<IExtendedEncounterRepository, ExtendedEncounterRepository>();
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ISportServices, SportServices>();
            services.AddScoped<ITeamServices, TeamServices>();
            services.AddScoped<IEncounterSimpleServices, EncounterSimpleServices>();
            services.AddScoped<IEncounterQueryServices, EncounterQueryServices>();
            services.AddScoped<IFixtureServices, FixtureServices>();
            services.AddScoped<ILogger, DataBaseLogger>();
            services.AddScoped<ILoggerServices, LoggerServices>();
            services.AddScoped<IPositionsServices, PositionServices>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "http://localhost:5000",
                ValidAudience = "http://localhost:5000",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Secret"]))
            };
        });
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
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
