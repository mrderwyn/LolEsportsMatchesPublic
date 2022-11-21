using LolEsportsMatches.DataAccess.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace LolEsportsMatches.DataAccess.EntityFrameworkCore.Context
{
    public class MatchHistoryContext : DbContext
    {
        public MatchHistoryContext()
        {
        }

        public MatchHistoryContext(DbContextOptions<MatchHistoryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameEntity>()
                .HasIndex(e => e.GameDate);
        }

        public DbSet<GameEntity> Games { get; set; } = null!;
        public DbSet<TeamEntity> Teams { get; set; } = null!;
        public DbSet<LeagueEntity> Leagues { get; set; } = null!;
    }
}
