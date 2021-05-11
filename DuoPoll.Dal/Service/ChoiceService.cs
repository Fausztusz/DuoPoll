using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Service
{
    public class ChoiceService
    {
        private readonly DuoPollDbContext _dbContext;

        public ChoiceService(DuoPollDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Choice>> GetChoices(string id)
        {
            return await _dbContext.Choices
                .Where(c => c.UserIdentity == id)
                .ToListAsync();
        }

        public async Task<Choice> CreateChoice(Choice choice)
        {
            await _dbContext.Choices.AddAsync(choice);
            await _dbContext.SaveChangesAsync();
            return choice;
        }
    }
}