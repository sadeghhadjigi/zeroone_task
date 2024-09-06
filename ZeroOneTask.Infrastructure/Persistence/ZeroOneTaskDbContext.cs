using Microsoft.EntityFrameworkCore;
using ZeroOneTask.Domain.Entities;

namespace ZeroOneTask.Infrastructure.Persistence
{
    public class ZeroOneTaskDbContext : DbContext
    {
        private const string connectionString = "Server=.; Database=ZeroOneTaskDB; Trusted_Connection=True; TrustServerCertificate=True";

        public ZeroOneTaskDbContext(DbContextOptions<ZeroOneTaskDbContext> options) : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
            .HasKey(s => new { s.AgencyId, s.OriginCityId, s.DestinationCityId });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}