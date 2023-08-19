using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

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
