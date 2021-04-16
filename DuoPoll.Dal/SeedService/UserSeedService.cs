using System;
using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal.Entities;
using DuoPoll.Dal.SeedInterfaces;
using DuoPoll.Dal.Users;
using Microsoft.AspNetCore.Identity;

namespace DuoPoll.Dal.SeedService
{
    public class UserSeedService : IUserSeedService
    {
        private readonly UserManager<User> userManager;

        public UserSeedService( UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SeedUserAsync()
        {
            if ( !(await userManager.GetUsersInRoleAsync( Roles.Administrator )).Any() )
            {
                var user = new User
                {
                    Email = "admin@duopoll.hu",
                    NormalizedEmail = "Adminisztrátor Aladár",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin"
                };

                var createResult = await userManager.CreateAsync(user, "asdasdasdasdasd");
                var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Administrator);

                if (!createResult.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new ApplicationException("Administrator clould not be created:" +
                                                   String.Join(", ", createResult.Errors.Concat(addToRoleResult.Errors).Select(e => e.Description)));
                }
            }
        }
    }
}