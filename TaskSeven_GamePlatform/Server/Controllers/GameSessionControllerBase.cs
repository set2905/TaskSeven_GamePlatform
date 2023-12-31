﻿using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Controllers
{
    public abstract class GameSessionControllerBase : ControllerBase
    {
        protected readonly IGameService gameService;
        protected readonly IPlayerService playerService;

        protected GameSessionControllerBase(IPlayerService playerService, IGameService gameService)
        {
            this.gameService=gameService;
            this.playerService=playerService;
        }
        [HttpPost]
        [Route("GameState")]
        public async Task<IActionResult> GetGameState(GetByIdRequestModel model)
        {
            GameState? gameState = await gameService.UpdateGameState(model.Id);
            if (gameState == null) return BadRequest("Gamestate with provided id not found");
            return new JsonResult(gameState);
        }

        /// <param name="model"></param>
        /// <returns>Found opponent or null if opponent not found</returns>
        [HttpPost]
        [Route("StartGameSearch")]
        public async Task<IActionResult> StartGameSearch(GameSearchRequestModel model)
        {
            Player? player = await playerService.SetGameTypeToPlayer(model.PlayerId, model.GameTypeName);
            if (player==null) return BadRequest("Couldnt set game type");
            Player? opponent = await playerService.StartGameSearch(player, model.GameTypeName);
            return new JsonResult(opponent);
        }


        /// <param name="model"></param>
        /// <returns>Game state Id or null if game wasnt started</returns>
        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame(GameStartRequestModel model)
        {
            Guid? gameStateId = await gameService.StartGame(model.PlayerId, model.OpponentId, model.GameTypeName);
            if (gameStateId==null) return BadRequest("Couldnt start game");
            return new JsonResult(gameStateId);
        }

        [HttpPost]
        [Route("ExitGame")]
        public async Task<IActionResult> ExitGame(Guid playerId)
        {
            if (await gameService.ExitGame(playerId))
                return Ok();
            else
                return BadRequest("Couldnt exit game");
        }

        [HttpPost]
        [Route("Move")]
        public async Task<IActionResult> Move(MoveRequestModel model)
        {
            GameState? gameState = await gameService.UpdateGameState(model.StateId);
            if (gameState == null) return BadRequest("Gamestate with provided id not found");
            if (gameState.Player1.Id!=model.PlayerId&&gameState.Player2.Id!=model.PlayerId)
                return BadRequest("You dont belong here");

            bool success = await gameService.Play(model.PlayerId, model.Position, gameState);
            return new JsonResult(success);
        }








    }
}
