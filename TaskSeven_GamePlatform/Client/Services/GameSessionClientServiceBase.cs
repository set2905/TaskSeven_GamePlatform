using MudBlazor;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using static TaskSeven_GamePlatform.Client.Services.TicTacToeSessionClientService;

namespace TaskSeven_GamePlatform.Client.Services
{
    public abstract class GameSessionClientServiceBase
    {
        protected readonly IPlayerClientService PlayerService;
        protected readonly IGameSessionHub Hub;
        protected readonly ISnackbar Snackbar;
        protected readonly IGameClientService ClientService;


        public event Changed? OnStateChange;
        public event Changed? OnRestartTimer;

        public bool isLoading = false;
        public bool isGameOver = false;
        public string waitingForOpponentMessage = string.Empty;
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
        protected string gameTypeName;
        protected DialogOptions nameDialogOptions = new() { FullWidth = true };

        protected GameSessionClientServiceBase(IPlayerClientService playerService, IGameSessionHub hub, ISnackbar snackbar, IGameClientService clientService)
        {
            PlayerService=playerService;
            Hub=hub;
            Snackbar=snackbar;
            ClientService=clientService;
        }
        public virtual async Task Restart()
        {
            loadingMessage = "Looking for opponent...";
            waitingForOpponentMessage=string.Empty;
            isGameOver = false;
            isLoading = true;
            opponent=new("Opponent");
            currentGameState = null;
            await TryFindOpponent();
        }
        public async Task UpdateGameState()
        {
            if (currentGameState == null) return;
            if (player!=null)
            {
                player=await PlayerService.GetPlayer(player.Id);
                if (player.WaitingForMove)
                    waitingForOpponentMessage="Opponnents turn! Waiting...";
                else
                    waitingForOpponentMessage=string.Empty;
            }

            await UpdateGameState(currentGameState.Id);
        }

        protected abstract Task UpdateGameState(Guid gameStateId);
        public async Task<bool> Move(int pos)
        {
            if (currentGameState == null||player==null||opponent.ConnectionId==null) return false;
            bool? result = await ClientService.Move(new(player.Id, currentGameState.Id, pos));
            if (result == null) return false;
            if (result == true)
            {
                await UpdateGameState();
                await Hub.NotifyGameStateUpdate(opponent.ConnectionId);
            }
            if (result==true)
            {
                waitingForOpponentMessage="Opponnents turn! Waiting...";
            }
            return (bool)result;
        }
        protected async Task TryFindOpponent()
        {
            while (opponent.Id == default)
            {
                Player? foundOpponent = await ClientService.StartGameSearch(new(player.Id, gameTypeName));
                if (foundOpponent != null)
                {
                    loadingMessage = "Opponent found! Waiting for game to start.";
                    await Hub.NotifyFoundYou(foundOpponent.ConnectionId, player.Id);
                    opponent = foundOpponent;
                }
                await Task.Delay(7000);
            }
        }
        protected async Task HandleOpponentFoundYou(Guid opponentId)
        {
            isLoading = false;
            Snackbar.Add("Opponent found!", Severity.Success);
            Guid? gameStateId = await ClientService.StartGame(new(player.Id, opponentId, gameTypeName));
            if (gameStateId!=null)
            {
                Player? found = await PlayerService.GetPlayer(opponentId);
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

        protected async Task InitializePlayer(string name)
        {
            player = await PlayerService.SetPlayerName(new(name));
            string ConnectionId = await Hub.Initialize();
            Hub.OnGameStateUpdate += UpdateGameState;
            Hub.OnOpponentFoundYou += HandleOpponentFoundYou;
            Hub.OnGameStarted += HandleGameStart;
            await PlayerService.SetPlayerConnectionId(new(ConnectionId, player.Id));
        }

        private async Task HandleGameStart(Guid gameStateId)
        {
            isLoading = false;
            InvokeStateChanged();
            await UpdateGameState(gameStateId);
        }


        public async Task OnSubmitName(string gameTypeName)
        {
            if (playerName.Length == 0) return;
            nameDialogVisible = false;
            isLoading = true;
            loadingMessage = "Initializing player...";
            InvokeStateChanged();

            await InitializePlayer(playerName);
            loadingMessage = "Looking for opponent...";
            InvokeStateChanged();
            this.gameTypeName = gameTypeName;
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
