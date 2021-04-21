using Microsoft.AspNetCore.Builder;
using SQLitePCL;

namespace DuoPoll.MVC.Routes
{
    public static class Web
    {
        static Web()
        {
        }

        public static IApplicationBuilder Route(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "users",
                    pattern: "{controller=Users}/{action=Users}/{id?}");
                endpoints.MapControllerRoute(
                    name: "poll.details",
                    pattern: "Test/Details/{url}",
                    constraints: new {url = "length(32)"},
                    defaults: new {controller = "Test", action = "Details"}
                );
                endpoints.MapControllerRoute(
                    name: "poll.update",
                    pattern: "Test/Update/{url}",
                    constraints: new {url = "length(32)"},
                    defaults: new {controller = "Test", action = "Update"}
                );

                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}