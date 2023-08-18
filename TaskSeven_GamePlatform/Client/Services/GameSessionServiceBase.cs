using MudBlazor;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using static TaskSeven_GamePlatform.Client.Services.TicTacToeSessionService;

namespace TaskSeven_GamePlatform.Client.Services
{
    public abstract class GameSessionServiceBase
    {
        protected readonly IPlayerClientService PlayerService;
        protected readonly IGameSessionHub Hub;
        protected readonly ISnackbar Snackbar;

        public event Changed? OnStateChange;
        public event Changed? OnRestartTimer;

        public bool isLoading = false;
        public bool isGameOver = false;
        public string loadingMessage = "Looking for opponent...";
        public string gameOverMessage = "Game Over";
        public GameState? currentGameState;
        public Player? player;
        public Player opponent = new("Opponent");

        public int turnCounter;
        public bool nameDialogVisible = true;
        public string playerName = string.Empty;

        protected JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };
        protected Guid gameTypeId;
        protected DialogOptions nameDialogOptions = new() { FullWidth = true };

        protected GameSessionServiceBase(IPlayerClientService playerService, IGameSessionHub hub, ISnackbar snackbar)
        {
            PlayerService=playerService;
            Hub=hub;
            Snackbar=snackbar;
        }
        public virtual async Task Restart()
        {
            loadingMessage = "Looking for opponent...";
            isGameOver = false;
            isLoading = true;
            opponent=new("Opponent");
            currentGameState = null;
            await TryFindOpponent();
        }
        public async Task UpdateGameState()
        {
            if (currentGameState == null) return;
            await UpdateGameState(currentGameState.Id);
        }
        public abstract Task Move(int pos);

        protected abstract Task UpdateGameState(Guid gameStateId);

        protected async Task InitializePlayer(string name)
        {
            player = await PlayerService.SetPlayerName(new(name));
            string ConnectionId = await Hub.Initialize();
            Hub.OnGameStateUpdate += UpdateGameState;
            Hub.OnOpponentFoundYou += HandleOpponentFoundYou;
            Hub.OnGameStarted += HandleGameStart;
            await PlayerService.SetPlayerConnectionId(new(ConnectionId, player.Id));
        }
        protected abstract Task TryFindOpponent();

        private async Task HandleGameStart(Guid gameStateId)
        {
            isLoading = false;
            InvokeStateChanged();
                        await UpdateGameState(gameStateId);
        }
        protected abstract Task HandleOpponentFoundYou(Guid opponentId);

        public async Task OnSubmitName()
        {
            if (playerName.Length == 0) return;
            nameDialogVisible = false;
            isLoading = true;
            loadingMessage = "Initializing player...";
            InvokeStateChanged();

            await InitializePlayer(playerName);
            loadingMessage = "Looking for opponent...";
            InvokeStateChanged();
            gameTypeId = Guid.Parse("706C2E99-6F6C-4472-81A5-43C56E11637C");
            await TryFindOpponent();
        }
        protected void InvokeStateChanged()
        {
            if (OnStateChange!=null)
                OnStateChange.Invoke();
        }
        protected void InvokeRestartTimer()
        {
            if (OnRestartTimer!=null)
                OnRestartTimer.Invoke();
        }
        protected void ShowError(string message)
        {
            Snackbar.Add(message, Severity.Error);
        }

    }
}
