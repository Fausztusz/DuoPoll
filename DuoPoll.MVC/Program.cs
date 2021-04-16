using System.Threading.Tasks;
using DuoPoll.Dal;
using DuoPoll.MVC.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DuoPoll.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            (await CreateHostBuilder(args)
                    .Build()
                    .MigrateDatabaseAsync<DuoPollDbContext>())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}