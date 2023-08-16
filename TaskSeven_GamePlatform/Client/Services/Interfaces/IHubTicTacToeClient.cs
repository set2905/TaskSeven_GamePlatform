namespace TaskSeven_GamePlatform.Client.Services.Interfaces
{
    public interface IHubTicTacToeClient
    {
        public event OpponentFound? OnOpponentFound;
        public event GameStateUpdate? OnGameStateUpdate;
        public Task NotifyGameStateUpdate(string opponentConnId);
        public Task NotifyFoundYou(string opponentConnId, Guid playerId);


        public Task Initialize();

    }
}
