using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepo playerRepo;

        public PlayerService(IPlayerRepo playerRepo)
        {
            this.playerRepo=playerRepo;
        }

        public async Task<Player> SetName(string name)
        {
            Player? player = await playerRepo.GetByName(name);
            if (player==null)
            {
                player=new(name);
                await playerRepo.Save(player);
            }
            return player;
        }
        public async Task<Player?> StartGameSearch(Player player, GameType gameType)
        {
            player.CurrentGame=gameType;

            Player? opponent = await playerRepo.FindOpponent(gameType);
            if (opponent==null)
            {
                player.LookingForOpponent = true;
                await playerRepo.Save(player);
                return null;
            }
            return opponent;
        }
    }
}
