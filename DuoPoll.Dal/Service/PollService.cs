using System.Collections.Generic;
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

        public async Task<List<Poll>> Index()
        {
            return await _dbContext.Polls
                .Where(p => p.Public)
                .Where(p => p.Status == Poll.StatusType.Open)
                .Where(p => p.Answers.Count > 0)
                .Include(p => p.Answers)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Poll> Details(string url)
        {
            return await _dbContext.Polls
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Url == url);
        }

        // public async Task<Poll> GetPollWithoutIncludesAndStuffWithoutTrackingOfc(string url)
        // {
        // }

        public async Task<Poll> GetPollWithAnswers(string url)
        {
            return await _dbContext.Polls
                .AsNoTracking()
                .Include(p => p.Answers)
                .FirstOrDefaultAsync(m => m.Url == url);
        }

        public async Task<Poll> Create(PollHeader pollHeader)
        {
            if (pollHeader.Open.CompareTo(pollHeader.Close) == 1)
            {
                var temp = pollHeader.Open;
                pollHeader.Open = pollHeader.Close;
                pollHeader.Close = temp;
            }

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
            if (pollHeader.Open.CompareTo(pollHeader.Close) == 1)
            {
                var temp = pollHeader.Open;
                pollHeader.Open = pollHeader.Close;
                pollHeader.Close = temp;
            }

            var poll = await _dbContext.Polls
                .Include(p => p.Answers)
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

        public async Task<List<Poll>> Statistics()
        {
            return await _dbContext.Polls
                .Where(p => p.Answers.Count > 0)
                .Where(p => p.Public)
                .Where(p => p.Status != Poll.StatusType.Draft)
                .Include(p => p.User)
                .Include(p => p.Answers)
                .ThenInclude(a => a.Choices)
                .ToListAsync();
        }
    }
}