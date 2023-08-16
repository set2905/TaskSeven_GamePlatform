using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class PlayerRepo : IPlayerRepo
    {
        public async Task<bool> Delete(Player entity)
        {
            throw new NotImplementedException();
        }

        public Task<Player?> FindOpponent(GameType game)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Player>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Player?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Player?> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Save(Player entity)
        {
            throw new NotImplementedException();
        }
    }
}
