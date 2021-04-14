using DuoPoll.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal
{
    public class PollDbContext : DbContext
    {
        protected PollDbContext()
        {
        }

        public PollDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Poll>Polls{ get; set; }
    }
}