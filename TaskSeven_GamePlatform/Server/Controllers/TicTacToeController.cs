using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TicTacToeController : ControllerBase
    {
        private readonly ITicTacToeService tttService;
        private readonly IPlayerService playerService;

        public TicTacToeController(ITicTacToeService tttService, IPlayerService playerService)
        {
            this.tttService=tttService;
            this.playerService=playerService;
        }

        [HttpPost]
        [Route("StartGameSearch")]
        public async Task<IActionResult> StartGameSearch(string playerName, GameType gameType)
        {
            Player player = await playerService.SetName(playerName);
            Player? opponent = await playerService.StartGameSearch(player, gameType);
            return new JsonResult(opponent);
        }

        [HttpPost]
        [Route("Move")]
        public async Task<IActionResult> Move(MoveRequestModel moveModel)
        {
            await tttService.Play(0, moveModel.Position, moveModel.StateBase);
            return new JsonResult(hasWinner);
        }

    }
}
