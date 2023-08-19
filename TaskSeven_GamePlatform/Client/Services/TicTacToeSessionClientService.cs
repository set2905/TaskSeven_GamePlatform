using MudBlazor;
using System.Text.Json;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class TicTacToeSessionClientService : GameSessionClientServiceBase
    {


        public string[] cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();

        public TicTacToeSessionClientService(
            IPlayerClientService playerService,
            IGameSessionHub hub,
            ISnackbar snackbar,
            ITicTacToeClientService clientService) : base(playerService, hub, snackbar, clientService)
        {
        }

        public override async Task Restart()
        {
            cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();
            await base.Restart();
        }

        protected override async Task UpdateGameState(Guid gameStateId)
        {
            GameState? recievedState = await ClientService.GetGameState(gameStateId);
            if (recievedState == null) return;
            currentGameState = recievedState;
            int[]? field = JsonSerializer.Deserialize<int[]>(currentGameState.Field, options);
            for (int i = 0; i<field.Length; i++)
            {
                if (field[i] == 0) cellValues[i] = "o";
                if (field[i] == 1) cellValues[i] = "x";
            }
            if (currentGameState.IsGameOver)
            {
                isGameOver = true;
                if (currentGameState.Winner != null)
                    gameOverMessage = currentGameState.Winner.Id == player.Id ? "Game over! You win!" : "Game over! You lose!";

                if (currentGameState.IsDraw == true)
                    gameOverMessage = "Game over! Draw!";
            }
            InvokeRestartTimer();
            InvokeStateChanged();

        }


        public delegate void Changed();
    }

}
