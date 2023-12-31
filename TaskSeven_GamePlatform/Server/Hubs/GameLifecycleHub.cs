﻿using Microsoft.AspNetCore.SignalR;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Hubs
{
    public class GameLifecycleHub:Hub
    {
        //public async Task NotifyGameEnd(string opponentConnId)
        //{
        //    await Clients.Client(opponentConnId).SendAsync("NotifyGameEnd");
        //}
        public async Task NotifyFoundYou(string opponentConnId, Guid playerId)
        {
            await Clients.Client(opponentConnId).SendAsync("NotifyFoundYou", playerId);
        }
        public async Task NotifyGameStateUpdate(string opponentConnId)
        {
            await Clients.Client(opponentConnId).SendAsync("NotifyGameStateUpdate");
        }
        public async Task NotifyGameStarted(string opponentConnId, Guid gameStateId)
        {
            await Clients.Client(opponentConnId).SendAsync("NotifyGameStarted", gameStateId);
        }

    }

}
