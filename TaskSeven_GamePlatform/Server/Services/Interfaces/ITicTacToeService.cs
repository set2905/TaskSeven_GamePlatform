using TaskSeven_GamePlatform.Server.Controllers;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services.Interfaces
{
    public interface ITicTacToeService
    {
        public Task<bool> Play(TicTacToeMarker player, int position, GameState state);
        public Task<GameState?> GetGameState(Guid id);
        public Task<Guid?> StartGame(Guid playerId, Guid opponentId, Guid gameTypeId);


    }
}
