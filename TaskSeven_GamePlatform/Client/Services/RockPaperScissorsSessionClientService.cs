using MudBlazor;
using System.Text.Json;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class RockPaperScissorsSessionClientService : GameSessionClientServiceBase
    {
        public string[] cellValues = Enumerable.Repeat(string.Empty, 2).ToArray();

        public RockPaperScissorsSessionClientService(IPlayerClientService playerService,
                                                     IGameSessionHub hub,
                                                     ISnackbar snackbar,
                                                     IRockPaperScissorsClientService clientService) : base(playerService, hub, snackbar, clientService)
        {
        }
        public override async Task Restart()
        {
            await base.Restart();
            cellValues = Enumerable.Repeat(string.Empty, 2).ToArray();
        }
        protected override async Task UpdateGameState(Guid gameStateId)
        {
            GameState? recievedState = await ClientService.GetGameState(gameStateId);
            if (recievedState == null) return;
            currentGameState = recievedState;
            int[]? field = JsonSerializer.Deserialize<int[]>(currentGameState.Field, options);
            for (int i = 0; i<field.Length; i++)
            {
                switch (field[i])
                {
                    case 0:
                        cellValues[i] = "r";
                        break;
                    case 1:
                        cellValues[i] = "p";
                        break;
                    case 2:
                        cellValues[i] = "s";
                        break;
                    default:
                        cellValues[i] = string.Empty;
                        break;
                }
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




    }
}
