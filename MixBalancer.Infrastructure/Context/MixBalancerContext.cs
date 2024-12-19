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
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchHistory> MatchHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento User -> Player (Um-para-Um)
            modelBuilder.Entity<Player>()
                .HasOne(p => p.User)
                .WithOne(u => u.Player)
                .HasForeignKey<Player>(p => p.UserId);

            // Relacionamento Player -> MatchHistory (Um-para-Muitos)
            modelBuilder.Entity<MatchHistory>()
                .HasOne(mh => mh.Player)
                .WithMany(p => p.MatchHistories)
                .HasForeignKey(mh => mh.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Team -> Players (Muitos-para-Muitos)
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithMany(p => p.Teams);

            // Relacionamento Team -> Matches (Um-para-Muitos)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamA)
                .WithMany(t => t.MatchesAsTeamA)
                .HasForeignKey(m => m.TeamAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamB)
                .WithMany(t => t.MatchesAsTeamB)
                .HasForeignKey(m => m.TeamBId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento opcional Team -> ManagedByUser
            modelBuilder.Entity<Team>()
                .HasOne(t => t.ManagedByUser)
                .WithMany()
                .HasForeignKey(t => t.ManagedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuração do MatchIdCS2
            modelBuilder.Entity<Match>()
                .Property(m => m.MatchIdCS2)
                .HasMaxLength(50);  // Definindo o tamanho máximo para o MatchIdCS2
        }
    }
}