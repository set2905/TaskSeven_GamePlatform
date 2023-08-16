using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class GameStateRepo : IGameStateRepo
    {
        public async Task<bool> Delete(GameState entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GameState>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<GameState?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Save(GameState entity)
        {
            throw new NotImplementedException();
        }
    }
}
