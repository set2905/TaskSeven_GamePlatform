using MudBlazor;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class TicTacToeClientService : ClientAPIBase, ITicTacToeClientService
    {
        private readonly ISnackbar snackbar;
        public TicTacToeClientService(HttpClient httpClient, ISnackbar snackbar) : base(httpClient)
        {
            this.snackbar = snackbar;
        }

        public async Task<GameState?> GetGameState(Guid gameStateId)
        {
            try
            {
                return await PostAsync<GameState?, Guid>("api/TicTacToe/GameState", gameStateId);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }
        }


        /// <param name="model"></param>
        /// <returns>Found opponent or null if opponent not found</returns>
        public async Task<Player?> StartGameSearch(GameSearchRequestModel model)
        {
            try
            {
                return await PostAsync<Player?, GameSearchRequestModel>("api/TicTacToe/StartGameSearch", model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }
        }

        /// <param name="model"></param>
        /// <returns>Game state Id or null if game wasnt started</returns>
        public async Task<Guid?> StartGame(GameStartRequestModel model)
        {
            try
            {
                return await PostAsync<Guid?, GameStartRequestModel>("api/TicTacToe/StartGame", model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }
        }
    }
}
