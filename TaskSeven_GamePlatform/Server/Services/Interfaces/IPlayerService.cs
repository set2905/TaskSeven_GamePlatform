﻿using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services.Interfaces
{
    public interface IPlayerService
    {
        public Task<Player> SetName(string name);
        public Task<Player?> StartGameSearch(Player player, GameType gameType);


    }
}
