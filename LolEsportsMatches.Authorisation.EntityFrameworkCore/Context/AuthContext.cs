using LolEsportsMatches.Authorisation.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace LolEsportsMatches.Authorisation.EntityFrameworkCore.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext()
        {
        }

        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
