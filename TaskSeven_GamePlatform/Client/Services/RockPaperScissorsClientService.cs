using MudBlazor;
using TaskSeven_GamePlatform.Client.Services.Interfaces;

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
