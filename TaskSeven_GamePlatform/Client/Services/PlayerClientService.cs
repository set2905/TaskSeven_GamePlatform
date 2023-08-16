using MudBlazor;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class PlayerClientService : ClientAPIBase, IPlayerClientService
    {
        private readonly ISnackbar snackbar;
        public PlayerClientService(HttpClient httpClient, ISnackbar snackbar) : base(httpClient)
        {
            this.snackbar = snackbar;
        }

        public async Task<Player?> SetPlayerName(SetNameRequestModel model)
        {
            try
            {
                return await PostAsync<Player, SetNameRequestModel>("api/TicTacToe/SetPlayerName", model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }
        }
        public async Task SetPlayerConnectionId(SetConnIdRequestModel model)
        {
            try
            {
                await PostAsync("api/TicTacToe/SetConnectionId", model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
