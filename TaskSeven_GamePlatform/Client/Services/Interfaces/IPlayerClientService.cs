using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services.Interfaces
{
    public interface IPlayerClientService
    {
        public Task<Player?> SetPlayerName(SetNameRequestModel model);

        public Task SetPlayerConnectionId(SetConnIdRequestModel model);
    }
}
