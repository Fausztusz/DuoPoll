using IHateThisSh_t.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace IHateThisSh_t.Dal
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