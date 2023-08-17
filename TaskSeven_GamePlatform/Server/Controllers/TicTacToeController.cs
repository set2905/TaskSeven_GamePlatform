using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

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
        [Route("Player")]
        public async Task<IActionResult> GetPlayer(GetByIdRequestModel model)
        {
            Player? player = await playerService.GetById(model.Id);
            if (player == null) return BadRequest("Player with provided id not found");
            return new JsonResult(player);
        }
        [HttpPost]
        [Route("GameState")]
        public async Task<IActionResult> GetGameState(GetByIdRequestModel model)
        {
            GameState? gameState = await tttService.GetGameState(model.Id);
            if (gameState == null) return BadRequest("Gamestate with provided id not found");
            return new JsonResult(gameState);
        }
        [HttpPost]
        [Route("SetConnectionId")]
        public async Task<IActionResult> SetPlayerConnectionId(SetConnIdRequestModel model)
        {
            if (!await playerService.SetPlayerConnectionId(model.PlayerId, model.ConnectionId))
                return BadRequest("Couldnt set player connection Id");
            return Ok();
        }
        [HttpPost]
        [Route("SetPlayerName")]
        public async Task<IActionResult> SetPlayerName(SetNameRequestModel model)
        {
            Player? player = await playerService.SetName(model.Name);
            return new JsonResult(player);
        }
        /// <param name="model"></param>
        /// <returns>Found opponent or null if opponent not found</returns>
        [HttpPost]
        [Route("StartGameSearch")]
        public async Task<IActionResult> StartGameSearch(GameSearchRequestModel model)
        {
            Player? player = await playerService.SetGameTypeToPlayer(model.PlayerId, model.GameTypeId);
            if (player==null) return BadRequest("Couldnt set game type");
            Player? opponent = await playerService.StartGameSearch(player, model.GameTypeId);
            return new JsonResult(opponent);
        }

        /// <param name="model"></param>
        /// <returns>Game state Id or null if game wasnt started</returns>
        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame(GameStartRequestModel model)
        {
            Guid? gameStateId = await tttService.StartGame(model.PlayerId, model.OpponentId, model.GameTypeId);
            if (gameStateId==null) return BadRequest("Couldnt start game");
            return new JsonResult(gameStateId);
        }

        [HttpPost]
        [Route("ExitGame")]
        public async Task<IActionResult> ExitGame(Guid playerId)
        {
            if (await tttService.ExitGame(playerId))
                return Ok();
            else
                return BadRequest("Couldnt exit game");
        }

        [HttpPost]
        [Route("Move")]
        public async Task<IActionResult> Move(MoveRequestModel model)
        {
            GameState? gameState = await tttService.GetGameState(model.StateId);
            if (gameState == null) return BadRequest("Gamestate with provided id not found");
            if (gameState.Player1.Id!=model.PlayerId&&gameState.Player2.Id!=model.PlayerId)
                return BadRequest("You dont belong here");
           
            bool success = await tttService.Play(model.PlayerId, model.Position, gameState);
            return new JsonResult(success);
        }

    }
}
