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
    public class TicTacToeSessionService:GameSessionServiceBase
    {
        public event Changed OnStateChange;
        public event Changed OnRestartTimer;
        private readonly ITicTacToeClientService ClientService;
        private readonly IPlayerClientService PlayerService;
        private readonly IHubTicTacToeClient Hub;
        private readonly ISnackbar Snackbar;

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        public bool isLoading = false;
        public bool isGameOver = false;
        public string loadingMessage = "Looking for opponent...";
        public string gameOverMessage = "Game Over";

        public string[] cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();
        public GameState? currentGameState;
        private Guid gameTypeId;
        public Player? player;
        public Player opponent = new("Opponent");

        public int turnCounter;
        private DialogOptions nameDialogOptions = new() { FullWidth = true };
        public bool nameDialogVisible = true;
        public string playerName = string.Empty;

        public TicTacToeSessionService(ITicTacToeClientService clientService, IPlayerClientService playerService, IHubTicTacToeClient hub, ISnackbar snackbar)
        {
            ClientService=clientService;
            PlayerService=playerService;
            Hub=hub;
            Snackbar=snackbar;
        }

        public async Task Restart()
        {
            cellValues = Enumerable.Repeat(string.Empty, 9).ToArray();
            loadingMessage = "Looking for opponent...";
            isGameOver = false;
            isLoading = true;
            opponent=new("Opponent");
            currentGameState = null;
            await TryFindOpponent();
        }
        private async Task InitializePlayer(string name)
        {
            player = await PlayerService.SetPlayerName(new(name));
            string ConnectionId = await Hub.Initialize();
            Hub.OnGameStateUpdate += UpdateGameState;
            Hub.OnOpponentFoundYou += HandleOpponentFoundYou;
            Hub.OnGameStarted += HandleGameStart;
            await PlayerService.SetPlayerConnectionId(new(ConnectionId, player.Id));
        }
        private async Task TryFindOpponent()
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
        public async Task UpdateGameState()
        {
            if (currentGameState == null) return;
            await UpdateGameState(currentGameState.Id);
        }
        private async Task UpdateGameState(Guid gameStateId)
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
            OnRestartTimer.Invoke();
            OnStateChange.Invoke();

        }
        public async Task Move(int pos)
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
        private async Task HandleGameStart(Guid gameStateId)
        {
            isLoading = false;
            OnStateChange.Invoke();
            await UpdateGameState(gameStateId);
        }
        private async Task HandleOpponentFoundYou(Guid opponentId)
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
                OnStateChange.Invoke();

            }
        }
        public async Task OnSubmitName()
        {
            if (playerName.Length == 0) return;
            nameDialogVisible = false;
            isLoading = true;
            loadingMessage = "Initializing player...";
            OnStateChange.Invoke();

            await InitializePlayer(playerName);
            loadingMessage = "Looking for opponent...";
            OnStateChange.Invoke();
            gameTypeId = Guid.Parse("706C2E99-6F6C-4472-81A5-43C56E11637C");
            await TryFindOpponent();

        }
        private void ShowError(string message)
        {
            Snackbar.Add(message, Severity.Error);
        }
       public delegate void Changed();
    }

}
