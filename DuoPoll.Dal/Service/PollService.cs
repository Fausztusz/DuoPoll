using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Service
{
    public class PollService
    {
        public DuoPollDbContext DbContext { get; }
        private UserManager<User> _userManager;


        public PollService(UserManager<User> userManager, DuoPollDbContext dbContext)
        {
            DbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Poll> GetPoll(string url)
        {
            return await DbContext.Polls.Where(p => p.Url == url).SingleOrDefaultAsync();
        }

        public async Task AddOrCreate(PollHeader pollHeader, User user)
        {
            if (pollHeader.Id == 0)
            {
                // Új létrehozása
                var poll = new Poll
                {
                    Name = pollHeader.Name,
                    Public = pollHeader.Public,
                    Open = pollHeader.Open,
                    Close = pollHeader.Close,
                    // User = await _userManager.FindByNameAsync(this.User.GetDisplayName())
                };
            }
        }
    }
}