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
        [HttpGet]
        [Route("GameState")]
        public async Task<IActionResult> GetGameState(Guid gameStateId)
        {
            GameState? gameState = await tttService.GetGameState(gameStateId);
            if (gameState == null) return BadRequest("GameTypeId state with provided id not found");
            return new JsonResult(gameState);
        }

        [HttpPost]
        [Route("StartGameSearch")]
        public async Task<IActionResult> StartGameSearch(GameSearchRequestModel model)
        {
            Player? player = await playerService.SetNameAndGameType(model.PlayerName, model.GameTypeId);
            if (player==null) return BadRequest("Couldnt set player name and game type");
            Player? opponent = await playerService.StartGameSearch(player, model.GameTypeId);
            return new JsonResult(opponent);
        }

        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame(GameStartRequestModel model)
        {
            Guid? gameStateId = await tttService.StartGame(model.PlayerId, model.OpponentId, model.GameTypeId);
            if (gameStateId==null) return BadRequest("Couldnt start game");
            return new JsonResult(gameStateId);
        }

        [HttpPost]
        [Route("Move")]
        public async Task<IActionResult> Move(MoveRequestModel model)
        {
            GameState? gameState = await tttService.GetGameState(model.StateId);
            if (gameState == null) return BadRequest("GameTypeId state with provided id not found");
            if (gameState.Player1.Id!=model.PlayerId&&gameState.Player2.Id!=model.PlayerId)
                return BadRequest("You dont belong here");
            TicTacToeMarker marker = TicTacToeMarker.O;
            if (gameState.Player1.Id==model.PlayerId) marker=TicTacToeMarker.X;
            bool hasWinner = await tttService.Play(marker, model.Position, gameState);
            return new JsonResult(hasWinner);
        }

    }
}
