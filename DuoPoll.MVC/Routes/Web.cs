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
                endpoints.MapRazorPages();
            });

            return app;
        }
    }
}