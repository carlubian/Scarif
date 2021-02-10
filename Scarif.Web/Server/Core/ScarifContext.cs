using Microsoft.EntityFrameworkCore;
using Scarif.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Web.Server.Core
{
    public class ScarifContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=scarif-logs.postgres.database.azure.com;Database=scarif;Port=5432;User Id=scarif@scarif-logs;Password=&$p85wPR$Zh9awws@8jpK4ps;Ssl Mode=Require;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasKey(n => new { n.LogId, n.Key });
        }
    }
}
