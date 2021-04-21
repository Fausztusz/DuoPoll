using System;
using Azure.Storage.Blobs;
using DuoPoll.Dal;
using DuoPoll.Dal.Entities;
using DuoPoll.MVC.Routes;
using DuoPoll.MVC.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Azure;

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

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration.GetConnectionString("BlobContext"))
                    .WithName("BlobService");
            });

            // SMTP szerver beállításokat felolvassa az appsettings.json-ból a MailSettings osztályba.
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, Services.EmailSender>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAntiforgery(o =>
            {
                o.FormFieldName = "_csrf";
                o.HeaderName = "X-CSRF-TOKEN";
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
            });

            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager,
            IAzureClientFactory<BlobServiceClient> blobClientFactory)
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

            app.UseAuthentication();
            app.UseAuthorization();

            Web.Route(app);
        }
    }
}