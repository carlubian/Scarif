using Microsoft.EntityFrameworkCore;
using Scarif.Core.Model;

namespace Scarif.Server.Server.Persistence
{
    public class ScarifContext : DbContext
    {
        public DbSet<App> Apps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = TestDb.scarif");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasKey(n => new { n.LogId, n.Key });
        }
    }
}
