using System;
using DuoPoll.Dal;
using DuoPoll.MVC.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Identity.Web.UI;

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

            // services.ConfigureApplicationCookie(options =>
            // {
            //     options.AccessDeniedPath = PathString.FromUriComponent("/Identity/Account/AccessDenied"); ;
            //     options.LoginPath = PathString.FromUriComponent("/Identity/Account/Login");
            //     options.LogoutPath = PathString.FromUriComponent("/Identity/Account/Logout");
            // });

            // services.AddAuthentication(IISDefaults.AuthenticationScheme);

            // services.AddAuthorization(options =>
            // {
            //     options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole(Roles.Administrator));
            // });

            // services.AddDatabaseDeveloperPageExceptionFilter();

            // services.AddScoped<IRoleSeedService, RoleSeedService>()
            //     .AddScoped<IUserSeedService, UserSeedService>();

            // services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //     .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            // services.AddControllersWithViews(options =>
            // {
            //     var policy = new AuthorizationPolicyBuilder()
            //         .RequireAuthenticatedUser()
            //         .Build();
            //     options.Filters.Add(new AuthorizeFilter(policy));
            // });
            /* OAuth */

            services.AddIdentityServer(o =>
            {
                o.Events.RaiseErrorEvents = true;
                o.Events.RaiseFailureEvents = true;
                o.Events.RaiseInformationEvents = true;
                o.Events.RaiseSuccessEvents = true;
            }).AddConfigurationStore(o =>
            {
                o.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString(nameof(DuoPollDbContext)));
                };
            }).AddOperationalStore(o =>
            {
                o.ConfigureDbContext = opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString(nameof(DuoPollDbContext)));
                };
            }).AddAspNetIdentity<IdentityUser>();

            // SMTP szerver beállításokat felolvassa az appsettings.json-ból a MailSettings osztályba.
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, Services.EmailSender>();
            // var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            // var callBackUrl = Configuration.GetValue<string>("CallBackUrl");
            var identityUrl = "https://ktkaccount.typhoon.ktk.bme.hu/";
            var callBackUrl = "https://localhost:44303/Login/Callback";
            var sessionCookieLifetime = Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

            // Add Authentication services

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = identityUrl.ToString();
                    options.SignedOutRedirectUri = callBackUrl.ToString();
                    options.ClientId = "40";
                    options.ClientSecret = "BQoassHlOPvJsrkwcwdHw3tDxqoSDBxbt7mgbzDD";
                    options.ResponseType = "id_token";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.RequireHttpsMetadata = false;
                    options.Scope.Add("read:user");
                    options.Scope.Add("read:user.username");
                    options.Scope.Add("read:user.firstname");
                    options.Scope.Add("read:user.lastname");
                });

            services.AddRazorPages().AddMicrosoftIdentityUI();
            // services.AddControllersWithViews();
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

            app.UseIdentityServer();

            app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            // app = Web.Route(app);
        }
    }
}