using System.Threading.Tasks;
using DuoPoll.Dal.SeedInterfaces;
using DuoPoll.Dal.Users;
using Microsoft.AspNetCore.Identity;

namespace DuoPoll.Dal.SeedService
{
    public class RoleSeedService : IRoleSeedService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleSeedService( RoleManager<IdentityRole<int>> roleManager)
        {
            this._roleManager = roleManager;
        }

        public async Task SeedRoleAsync()
        {
            if( !await _roleManager.RoleExistsAsync( Roles.Administrator ) )
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Administrator });
        }
    }
}
