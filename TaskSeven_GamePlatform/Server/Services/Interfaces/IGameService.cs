using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services.Interfaces
{
    public interface IGameService
    {
        public Task<bool> Play(Guid playerId, int position, GameState state);
        public Task<GameState?> UpdateGameState(Guid id);
        public Task<Guid?> StartGame(Guid playerId, Guid opponentId, string gameTypeName);
        public Task<bool> ExitGame(Guid playerId);
    }
}
