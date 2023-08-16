using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services.Interfaces
{
    public interface IPlayerService
    {
        public Task<Player?> SetNameAndGameType(string name, Guid gameTypeId);
        public Task<Player?> StartGameSearch(Player player, Guid gameTypeId);


    }
}
