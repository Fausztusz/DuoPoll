using System;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;
using DuoPoll.Dal.Users;
using DuoPoll.MVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DuoPoll.MVC.Areas.Identity.IdentityHostingStartup))]
namespace DuoPoll.MVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityDataContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString(nameof(DuoPollDbContext))));

                // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                // .AddEntityFrameworkStores<IdentityDataContext>();

                services.Configure<IdentityOptions>(o =>
                {
                    // Password requirements are stupid
                    // Its length whats matter - That's what she said
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequiredLength = 10;
                    o.Password.RequiredUniqueChars = 3;

                    o.SignIn.RequireConfirmedAccount = false;
                    o.SignIn.RequireConfirmedEmail = false;
                    o.SignIn.RequireConfirmedPhoneNumber = false;
                });

                // services.AddDefaultIdentity<IdentityUser>()
                //     .AddEntityFrameworkStores<DuoPollDbContext>()
                //     .AddDefaultTokenProviders();

                services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<DuoPollDbContext>()
                .AddDefaultTokenProviders();

            });
        }
    }
}