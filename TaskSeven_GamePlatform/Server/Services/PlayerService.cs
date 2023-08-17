using System.Security.AccessControl;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

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
        public async Task<Player?> GetById(Guid id)
        {
            Player? player = await playerRepo.GetById(id);
            return player;
        }
        public async Task<bool> SetPlayerConnectionId(Guid playerId, string connId)
        {
            Player? player = await playerRepo.GetById(playerId);
            if (player==null)
                return false;
            player.ConnectionId=connId;
            await playerRepo.Save(player);
            return true;
        }
        public async Task<Player?> SetGameTypeToPlayer(Guid playerId, Guid gameTypeId)
        {
            GameType? gameType = await gameTypeRepo.GetById(gameTypeId);
            if (gameType==null) return null;

            Player? player = await playerRepo.GetById(playerId);
            if (player==null)
                return null;
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
            player.GameSearchStarted=DateTime.Now;
            Player? opponent = await playerRepo.FindOpponent(gameType, player.Id);
            if (opponent==null) player.LookingForOpponent = true;
            await playerRepo.Save(player);
            return opponent;
        }
    }
}
