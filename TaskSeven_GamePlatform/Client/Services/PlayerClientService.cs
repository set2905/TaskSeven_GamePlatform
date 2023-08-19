using MudBlazor;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatform.Shared.Models.Requests;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class PlayerClientService : ClientAPIBase, IPlayerClientService
    {
        public PlayerClientService(HttpClient httpClient, ISnackbar snackbar) : base(httpClient, snackbar)
        {
            APIUrl="api/Player/";
        }
        public async Task<Player?> GetPlayer(Guid playerId)
        {
            try
            {
                return await PostAsync<Player?, GetByIdRequestModel>($"{APIUrl}Player", new(playerId));
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
                return null;
            }
        }
        public async Task<Player?> SetPlayerName(SetNameRequestModel model)
        {
            try
            {
                return await PostAsync<Player, SetNameRequestModel>($"{APIUrl}SetPlayerName", model);
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
                await PostAsync($"{APIUrl}SetConnectionId", model);
            }
            catch (Exception ex)
            {
                snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
