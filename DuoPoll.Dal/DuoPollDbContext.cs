using DuoPoll.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal
{
    public partial class DuoPollDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<Poll> Polls { get; set; }

        public DuoPollDbContext()
        {
        }

        public DuoPollDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Media)
                    .HasMaxLength(256);

                entity.HasOne(d => d.Poll)
                    .WithMany(p => p.Answers)
                    .IsRequired()
                    .HasForeignKey(d=>d.PollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Polls");

                entity.HasMany<Choice>(d => d.Choices)
                    .WithOne(c => c.Answer)
                    .HasForeignKey(c => c.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Choices");
            });

            modelBuilder.Entity<Choice>(entity =>
            {
            });

            modelBuilder.Entity<Poll>(entity =>
            {

            });
        }
    }
}