﻿using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Client.Services.Interfaces
{
    public interface ITicTacToeClientService
    {
        public Task<GameState?> GetGameState(Guid gameStateId);
        public Task<Player?> StartGameSearch(GameSearchRequestModel model);
        public Task ExitGame(Guid playerId);
        public Task<bool?> Move(MoveRequestModel model);
        public Task<Guid?> StartGame(GameStartRequestModel model);


    }
}
