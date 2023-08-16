namespace TaskSeven_GamePlatform.Client.Services.Interfaces
{
    public interface IHubTicTacToeClient
    {
        public event IdDelegate? OnOpponentFound;
        public event IdDelegate? OnGameStarted;
        public event GameStateUpdate? OnGameStateUpdate;
        public Task NotifyGameStateUpdate(string opponentConnId);
        public Task NotifyFoundYou(string opponentConnId, Guid playerId);
        public Task NotifyGameStarted(string opponentConnId, Guid gameStateId);


        public Task<string> Initialize();

    }
}
