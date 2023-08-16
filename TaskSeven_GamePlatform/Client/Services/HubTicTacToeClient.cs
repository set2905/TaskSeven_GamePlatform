using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using TaskSeven_GamePlatform.Client.Services.Interfaces;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class HubTicTacToeClient: IHubTicTacToeClient
    {
        HubConnection? hubConnection;
        NavigationManager navigation;
        public event GameStateUpdate? OnGameStateUpdate;
        public event OpponentFound? OnOpponentFound;
        public HubTicTacToeClient(NavigationManager navigation)
        {
            this.navigation=navigation;
        }

        public async Task Initialize()
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
        }
        public async Task NotifyGameStateUpdate(string opponentConnId)
        {
            await hubConnection.SendAsync("NotifyGameStateUpdate", opponentConnId);
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
    public delegate Task OpponentFound(Guid playerId);
    public delegate Task GameStateUpdate();
}
