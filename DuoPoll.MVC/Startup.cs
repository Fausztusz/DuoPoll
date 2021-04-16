using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.Dal.SeedInterfaces;
using DuoPoll.Dal.SeedService;
using DuoPoll.Dal.Users;
using DuoPoll.MVC.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DuoPoll.MVC
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
            services.AddDbContext<DuoPollDbContext>(
                o => o.UseSqlServer(Configuration.GetConnectionString(nameof(DuoPollDbContext)))
            );

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = PathString.FromUriComponent("/Identity/Account/AccessDenied"); ;
                options.LoginPath = PathString.FromUriComponent("/Identity/Account/Login");
                options.LogoutPath = PathString.FromUriComponent("/Identity/Account/Logout");
            });

            // services.AddAuthentication(IISDefaults.AuthenticationScheme);

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole(Roles.Administrator));
            // });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IRoleSeedService, RoleSeedService>()
                .AddScoped<IUserSeedService, UserSeedService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();


            app = Web.Route(app);
        }
    }
}