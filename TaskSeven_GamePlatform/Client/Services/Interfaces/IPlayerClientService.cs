﻿using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatform.Shared.Models.Requests;

namespace TaskSeven_GamePlatform.Client.Services.Interfaces
{
    public interface IPlayerClientService
    {
        public Task<Player?> GetPlayer(Guid playerId);

        public Task<Player?> SetPlayerName(SetNameRequestModel model);

        public Task SetPlayerConnectionId(SetConnIdRequestModel model);
    }
}
