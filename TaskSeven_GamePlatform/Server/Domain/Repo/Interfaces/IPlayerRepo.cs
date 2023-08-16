using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces
{
    public interface IPlayerRepo : IRepo<Player>
    {
        public Task<Player?> GetByName(string name);
        public Task<Player?> FindOpponent(GameType game);

    }
}
