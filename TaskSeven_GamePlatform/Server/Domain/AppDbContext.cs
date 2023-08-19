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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GameType>().HasData(
             new GameType()
             {
                 Id=new Guid("706C2E99-6F6C-4472-81A5-43C56E11637C"),
                 FieldSize=9,
                 Name="TicTacToe"

             },
             new GameType()
             {
                 Id=new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483"),
                 FieldSize=2,
                 Name="RockPaperScissors"
             }
            );
        }
        public DbSet<GameState> GameStates { get; set; }
        public DbSet<GameType> GameTypes { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
