using System.Threading.Tasks;
using DuoPoll.Dal.Dto;
using DuoPoll.Dal.Entities;

namespace DuoPoll.Dal.Service
{
    public class PollService
    {
        public DuoPollDbContext DbContext { get; }

        public PollService(DuoPollDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /*
    public async Task AddOrCreate(PollHeader pollHeader)
    {
        if(pollHeader.Id == 0)
        {
            // Új létrehozása
            var poll = new Poll
            {
                Name = pollHeader.Name,
                Public = pollHeader.Public,
                Open = pollHeader.Open,
                Close = pollHeader.Close,
                User = await _userManager.FindByNameAsync(this.User.GetDisplayName())
            };

            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();
        }
        else
        {
            // Módosítás
            var category = DbContext.Categories.Single(c => c.Id == categoryHeader.Id);

            category.Name = categoryHeader.Name;
            category.Order = categoryHeader.Order;
            category.ParentCategoryId = categoryHeader.ParentCategoryId;

            await DbContext.SaveChangesAsync();

    }
        }*/

        public class User
        {
        }
    }
}