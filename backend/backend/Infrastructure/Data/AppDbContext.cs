using backend.Domain.models;
using backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<TypingSession> TypingSessions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
