using Microsoft.AspNetCore.Mvc;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        protected readonly IPlayerService playerService;

        public PlayerController(IPlayerService playerService)
        {
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
    }
}
