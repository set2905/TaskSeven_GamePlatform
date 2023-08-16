using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepo playerRepo;
        private readonly IGameTypeRepo gameTypeRepo;

        public PlayerService(IPlayerRepo playerRepo, IGameTypeRepo gameTypeRepo)
        {
            this.playerRepo=playerRepo;
            this.gameTypeRepo=gameTypeRepo;
        }

        public async Task<Player?> SetNameAndGameType(string name, Guid gameTypeId)
        {
            Player? player = await playerRepo.GetByName(name);
            GameType? gameType = await gameTypeRepo.GetById(gameTypeId);
            if (gameType==null) return null;
            if (player==null)
            {
                player=new(name, gameType);
            }
            else
                player.CurrentGameType=gameType;

            await playerRepo.Save(player);
            return player;
        }
        public async Task<Player?> StartGameSearch(Player player, Guid gameTypeId)
        {
            GameType? gameType = await gameTypeRepo.GetById(gameTypeId);
            if (gameType==null) return null;
            player.CurrentGameType=gameType;

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
