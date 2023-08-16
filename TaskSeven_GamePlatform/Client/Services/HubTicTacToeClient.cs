using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TaskSeven_GamePlatform.Client.Services.Interfaces;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class HubTicTacToeClient : IHubTicTacToeClient
    {
        HubConnection? hubConnection;
        NavigationManager navigation;
        public event GameStateUpdate? OnGameStateUpdate;
        public event IdDelegate? OnOpponentFound;
        public event IdDelegate? OnGameStarted;
        public HubTicTacToeClient(NavigationManager navigation)
        {
            this.navigation=navigation;
        }

        public async Task<string> Initialize()
        {
            if (hubConnection == null)
            {
                var uri = navigation.ToAbsoluteUri("/GameLifecycleHub");
                hubConnection = new HubConnectionBuilder().WithUrl(uri).Build();
            }
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
            hubConnection.On("NotifyGameStateUpdate", () =>
            {
                if (OnGameStateUpdate!=null)
                    OnGameStateUpdate.Invoke();
            });

            hubConnection.On<Guid>("NotifyFoundYou", (playerId) =>
            {
                if (OnOpponentFound!=null)
                    OnOpponentFound.Invoke(playerId);
            });
            hubConnection.On<Guid>("NotifyGameStarted", (gameStateId) =>
            {
                if (OnGameStarted!=null)
                    OnGameStarted.Invoke(gameStateId);
            });
            return hubConnection.ConnectionId;
        }
        public async Task NotifyGameStateUpdate(string opponentConnId)
        {
            await hubConnection.SendAsync("NotifyGameStateUpdate", opponentConnId);
        }      
        public async Task NotifyGameStarted(string opponentConnId, Guid gameStateId)
        {
            await hubConnection.SendAsync("NotifyGameStarted", opponentConnId, gameStateId);
        }
        public async Task NotifyFoundYou(string opponentConnId, Guid playerId)
        {
            await hubConnection.SendAsync("NotifyFoundYou", opponentConnId, playerId);
        }
        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
    public delegate Task IdDelegate(Guid id);
    public delegate Task GameStateUpdate();
}
