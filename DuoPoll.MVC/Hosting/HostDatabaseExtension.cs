using System.Threading.Tasks;
using DuoPoll.Dal.SeedInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DuoPoll.MVC.Hosting
{
    public static class HostDataExtensions
    {
        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();

                // Role és User létrehozás.
                // var roleSeeder = serviceProvider.GetRequiredService<IRoleSeedService>();
                // await roleSeeder.SeedRoleAsync();

                // var userSeeder = serviceProvider.GetRequiredService<IUserSeedService>();
                // await userSeeder.SeedUserAsync();
            }
            return host;
        }

    }
}