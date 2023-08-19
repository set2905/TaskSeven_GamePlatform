using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces
{
    public interface IGameTypeRepo : IRepo<GameType>
    {
        public Task<GameType?> GetByName(string name);

    }
}
