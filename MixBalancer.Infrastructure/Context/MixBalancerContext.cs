using Microsoft.EntityFrameworkCore;
using MixBalancer.Domain.Entities;

namespace MixBalancer.Infrastructure.Context
{
    public class MixBalancerContext : DbContext
    {
        public MixBalancerContext(DbContextOptions<MixBalancerContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}