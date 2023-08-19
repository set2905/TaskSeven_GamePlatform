using MudBlazor;
using TaskSeven_GamePlatform.Client.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services
{
    public class RockPaperScissorsClientService : GameClientServiceBase, IRockPaperScissorsClientService
    {
        public RockPaperScissorsClientService(HttpClient httpClient, ISnackbar snackbar) : base(httpClient, snackbar)
        {
            APIUrl="api/RockPaperScissors/";
        }
    }
}
