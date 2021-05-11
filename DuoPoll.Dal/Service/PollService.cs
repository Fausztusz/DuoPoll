using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Service
{
    public class PollService
    {
        private readonly DuoPollDbContext _dbContext;

        public PollService(DuoPollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Poll> Create(PollHeader pollHeader)
        {
            var poll = new Poll
            {
                UserId = pollHeader.UserId,
                Name = pollHeader.Name,
                Public = pollHeader.Public,
                Status = pollHeader.Status,
                Open = pollHeader.Open,
                Close = pollHeader.Close,
            };
            await _dbContext.Polls.AddAsync(poll);
            await _dbContext.SaveChangesAsync();

            return poll;
        }

        public async Task<Poll> Update(PollHeader pollHeader)
        {
            var poll = await _dbContext.Polls
                .Where(p => p.Url == pollHeader.Url)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new DbUpdateConcurrencyException();

            poll.UserId = pollHeader.UserId;
            poll.Name = pollHeader.Name;
            poll.Public = pollHeader.Public;
            poll.Status = pollHeader.Status;
            poll.Open = pollHeader.Open;
            poll.Close = pollHeader.Close;

            _dbContext.Update(poll);
            await _dbContext.SaveChangesAsync();

            return poll;
        }
    }
}