using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Service
{
    public class AnswerService
    {
        private readonly DuoPollDbContext _dbContext;

        public AnswerService(DuoPollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Answer>> GetAnswers(string url)
        {
            var poll = await _dbContext.Polls
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Url == url);

            return await _dbContext.Answers
                .Where(a => a.PollId == poll.Id)
                .Include(a => a.Choices)
                .Include(a => a.Losses)
                .ToListAsync();
        }

        public async Task<Answer> GetAnswer(int id)
        {
            return await _dbContext.Answers
                .Where(a => a.Id == id)
                .Include(a => a.Poll)
                .FirstOrDefaultAsync();
        }

        public async Task<Answer> CreateAnswer(AnswerHeader answerHeader, int pollId)
        {
            var answer = new Answer
            {
                Title = answerHeader.Title,
                Media = answerHeader.Media,
                PollId = pollId
            };
            await _dbContext.Answers.AddAsync(answer);
            await _dbContext.SaveChangesAsync();
            return answer;
        }

        public async Task<Answer> UpdateAnswer(AnswerHeader answerHeader, int pollId)
        {
            var answer = await _dbContext.Answers.SingleAsync(a => a.Id == answerHeader.Id);

            answer.Title = answerHeader.Title;
            answer.Media = answerHeader.Media;

            _dbContext.Answers.Update(answer);
            await _dbContext.SaveChangesAsync();

            return answer;
        }

        public async Task DeleteAnswer(Answer answer)
        {
            var choices = await _dbContext.Choices
                .Where(c => c.AnswerId == answer.Id || c.LoserId == answer.Id)
                .ToListAsync();

            _dbContext.Choices.RemoveRange(choices);
            _dbContext.Answers.Remove(answer);
            await _dbContext.SaveChangesAsync();
        }
    }
}