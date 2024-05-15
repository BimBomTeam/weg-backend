using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEG.Domain.Entities;

namespace WEG.Application
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<DailyProgressStats> DailyProgresses { get; set; }
        public DbSet<GameDay> GameDays { get; set; }
        public DbSet<NpcRole> NpcRoles { get; set; }
        public DbSet<Word> Words { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DailyProgressStats>()
                .HasOne(ds => ds.User)
                .WithMany(u => u.DailyProgresses)
                .HasForeignKey(ds => ds.UserId);

            modelBuilder.Entity<DailyProgressStats>()
                .HasOne(ds => ds.Day)
                .WithMany()
                .HasForeignKey(ds => ds.DayId);

            modelBuilder.Entity<NpcRole>()
                .HasOne(nr => nr.Day)
                .WithMany(gd => gd.NpcRoles)
                .HasForeignKey(nr => nr.DayId);

            modelBuilder.Entity<Word>()
                .HasOne(w => w.DailyProgress)
                .WithMany(ds => ds.Words)
                .HasForeignKey(w => w.DailyProgressId);

            modelBuilder.Entity<Word>()
                .HasOne(w => w.Role)
                .WithMany(nr => nr.Words)
                .HasForeignKey(w => w.RoleId);
        }

    }
}
