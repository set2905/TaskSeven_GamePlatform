using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;

namespace TaskSeven_GamePlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RockPaperScissorsController : GameSessionControllerBase
    {
        public RockPaperScissorsController(IPlayerService playerService, IRockPaperScissorsService gameService) : base(playerService, gameService)
        {
        }
    }
}
