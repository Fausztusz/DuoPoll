using System;
using DuoPoll.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DuoPoll.Dal
{
    public partial class DuoPollDbContext : DbContext
    {
        public DuoPollDbContext()
        {
        }

        public DuoPollDbContext(DbContextOptions<DuoPollDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(
                    "Server=tcp:sql-server-for-my-homework.database.windows.net,1433;Initial Catalog=DuoPoll;Persist Security Info=False;User ID=sqladmin;Password=nyvUGG53RKQvRhM#3Y#r;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(p => p.Public)
                    .HasDefaultValue(false);
                entity.Property(p => p.Status)
                    .HasDefaultValue(Poll.StatusType.Draft);
                entity.Property(p => p.Open)
                    .HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.Close)
                    .HasDefaultValueSql("DATEADD(week,2,GETDATE())");
            });

            modelBuilder.Entity<Choice>(entity =>
            {
                entity.HasOne(c => c.Answer)
                    .WithMany(a => a.Choices)
                    .HasForeignKey(c => c.AnswerId)
                    .HasPrincipalKey(c => c.Id)
                    .HasConstraintName("FK_Choice_Winner_Answer")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Loser)
                    .WithMany(a => a.Losses)
                    .HasForeignKey(c => c.LoserId)
                    .HasPrincipalKey(c => c.Id)
                    .HasConstraintName("FK_Choice_Loser_Answer")
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<User> Users { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Choice> Choices { get; set; }
    }
}