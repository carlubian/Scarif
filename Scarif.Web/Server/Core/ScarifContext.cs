using ConfigAdapter.Ini;
using Microsoft.EntityFrameworkCore;
using Scarif.Core.Model;

namespace Scarif.Web.Server.Core
{
    public class ScarifContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var Config = IniConfig.From("settings.conf");
            var Server = Config.Read("Postgres:Server");
            var Database = Config.Read("Postgres:Database");
            var Port = Config.Read("Postgres:Port");
            var User = Config.Read("Postgres:User");
            var Password = Config.Read("Postgres:Password");

            optionsBuilder.UseSqlServer($"Server=tcp:{Server},{Port};Initial Catalog={Database};Persist Security Info=False;User ID={User};Password={Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasKey(n => new { n.LogId, n.Key });
        }
    }
}
