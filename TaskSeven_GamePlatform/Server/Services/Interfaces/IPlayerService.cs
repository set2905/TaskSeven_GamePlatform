using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services.Interfaces
{
    public interface IPlayerService
    {
        public Task<Player?> SetGameTypeToPlayer(Guid playerId, Guid gameTypeId);
        public Task<Player?> GetById(Guid playerId);
        public Task<Player> SetName(string name);
        public Task<bool> SetPlayerConnectionId(Guid playerId, string connId);
        public Task<Player?> StartGameSearch(Player player, Guid gameTypeId);


    }
}
