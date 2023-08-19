using MudBlazor;
using System.Text.Json;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class RockPaperScissorsSessionClientService : GameSessionClientServiceBase
    {
        public string[] cellValues = Enumerable.Repeat(string.Empty, 2).ToArray();
        public List<bool> moveButtonsDisabled = new() { false, false, false };


        public RockPaperScissorsSessionClientService(IPlayerClientService playerService,
                                                     IGameSessionHub hub,
                                                     ISnackbar snackbar,
                                                     IRockPaperScissorsClientService clientService) : base(playerService, hub, snackbar, clientService)
        {
        }
        public override async Task Restart()
        {
            moveButtonsDisabled = new() { false, false, false };
            cellValues = Enumerable.Repeat(string.Empty, 2).ToArray();

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
                string move1 = ((RockPaperScissorsMarker)field[0]).ToString();
                string move2 = ((RockPaperScissorsMarker)field[1]).ToString();
                if (currentGameState.Winner != null)
                    gameOverMessage = currentGameState.Winner.Id == player.Id ? $"Game over! You win!\n{move1} vs {move2}" : $"Game over! You lose!\n{move1} vs {move2}";

                if (currentGameState.IsDraw == true)
                    gameOverMessage = $"Game over! Draw!";
            }
            else
            {
                InvokeRestartTimer();
                return;
            }

            InvokeRestartTimer();
            InvokeStateChanged();
        }




    }
}
