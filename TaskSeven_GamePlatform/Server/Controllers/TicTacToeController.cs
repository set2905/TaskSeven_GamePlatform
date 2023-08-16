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
        public async Task<IActionResult> StartGameSearch(GameSearchRequestModel gameSearchRequestModel)
        {
            Player player = await playerService.SetNameAndGameType(gameSearchRequestModel.PlayerName, gameSearchRequestModel.GameTypeId);
            Player? opponent = await playerService.StartGameSearch(player, gameSearchRequestModel.GameTypeId);
            return new JsonResult(opponent);
        }

        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame(string playerName, GameType gameType)
        {
            
            return new JsonResult(opponent);
        }

        [HttpPost]
        [Route("Move")]
        public async Task<IActionResult> Move(MoveRequestModel moveModel)
        {
            GameState? gameState = await tttService.GetGameState(moveModel.StateId);
            if (gameState == null) return BadRequest("GameTypeId state with provided id not found");
            if (gameState.Player1.Id!=moveModel.Player.Id&&gameState.Player2.Id!=moveModel.Player.Id)
                return BadRequest("You dont belong here");
            TicTacToeMarker marker = TicTacToeMarker.O;
            if (gameState.Id==moveModel.Player.Id) marker=TicTacToeMarker.X;
            bool hasWinner = await tttService.Play(marker, moveModel.Position, gameState);
            return new JsonResult(hasWinner);
        }

    }
}
