using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Controllers
{
    public class RockPaperScissorsController : GameSessionControllerBase
    {
        public RockPaperScissorsController(IPlayerService playerService, IRockPaperScissorsService gameService) : base(playerService, gameService)
        {
        }
    }
}
