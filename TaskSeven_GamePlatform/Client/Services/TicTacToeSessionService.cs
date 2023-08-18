using Microsoft.Extensions.Options;
using MudBlazor;
using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class TicTacToeSessionService : GameSessionServiceBase
    {
        private readonly ITicTacToeClientService ClientService;


        public string[] cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();

        public TicTacToeSessionService(IPlayerClientService playerService, IGameSessionHub hub, ISnackbar snackbar, ITicTacToeClientService clientService) : base(playerService, hub, snackbar)
        {
            ClientService=clientService;
        }

        public override async Task Restart()
        {
            await base.Restart();
            cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();
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
        public override async Task Move(int pos)
        {
            if (currentGameState == null||player==null||opponent.ConnectionId==null) return;
            bool? result = await ClientService.Move(new(player.Id, currentGameState.Id, pos));
            if (result == null) return;
            if (result == true)
            {
                await UpdateGameState();
                await Hub.NotifyGameStateUpdate(opponent.ConnectionId);
            }
        }
        protected override async Task TryFindOpponent()
        {
            while (opponent.Id == default)
            {
                Player? foundOpponent = await ClientService.StartGameSearch(new(player.Id, gameTypeId));
                if (foundOpponent != null)
                {
                    loadingMessage = "Opponent found! Waiting for game to start.";
                    await Hub.NotifyFoundYou(foundOpponent.ConnectionId, player.Id);
                    opponent = foundOpponent;
                }
                await Task.Delay(7000);
            }
        }
        protected override async Task HandleOpponentFoundYou(Guid opponentId)
        {
            isLoading = false;
            Snackbar.Add("Opponent found!", Severity.Success);
            Guid? gameStateId = await ClientService.StartGame(new(player.Id, opponentId, gameTypeId));
            if (gameStateId!=null)
            {
                Player? found = await ClientService.GetPlayer(opponentId);
                if (found==null)
                {
                    ShowError("Opponent found, but opponent id is not correct!");
                    return;
                }
                opponent = found;
                if (opponent.ConnectionId==null)
                {
                    ShowError("Opponent found, but opponent connection id is null!");
                    return;
                }
                await Hub.NotifyGameStarted(opponent.ConnectionId, (Guid)gameStateId);
                await UpdateGameState((Guid)gameStateId);

                InvokeStateChanged();
            }
        }

        public delegate void Changed();
    }

}
