using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;

namespace TaskSeven_GamePlatform.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TicTacToeController : GameSessionControllerBase
    {
        public TicTacToeController(IPlayerService playerService, ITicTacToeService gameService) : base(playerService, gameService)
        {
        }
    }
}
