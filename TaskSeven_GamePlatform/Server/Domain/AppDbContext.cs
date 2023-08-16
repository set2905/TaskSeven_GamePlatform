using Microsoft.EntityFrameworkCore;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
