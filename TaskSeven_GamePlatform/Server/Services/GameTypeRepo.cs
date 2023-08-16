using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public class GameTypeRepo : IGameTypeRepo
    {
        public async Task<bool> Delete(GameType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GameType>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<GameType?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Save(GameType entity)
        {
            throw new NotImplementedException();
        }
    }
}
