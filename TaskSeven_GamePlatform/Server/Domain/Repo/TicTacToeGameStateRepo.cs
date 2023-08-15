using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Domain.Repo
{
    public class TicTacToeGameStateRepo : IGameStateRepo
    {
        public async Task<bool> Delete(TicTacToeGameState entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TicTacToeGameState>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<TicTacToeGameState?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Save(TicTacToeGameState entity)
        {
            throw new NotImplementedException();
        }
    }
}
