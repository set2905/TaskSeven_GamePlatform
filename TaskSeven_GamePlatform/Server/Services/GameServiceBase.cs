﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public abstract class GameServiceBase
    {
        protected readonly IGameStateRepo stateRepo;
        protected readonly IPlayerRepo playerRepo;
        protected readonly IGameTypeRepo gameTypeRepo;
        protected JsonSerializerOptions options;

        public GameServiceBase(IGameStateRepo stateRepo, IPlayerRepo playerRepo, IGameTypeRepo gameTypeRepo)
        {
            this.stateRepo=stateRepo;
            options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            this.playerRepo=playerRepo;
            this.gameTypeRepo=gameTypeRepo;
        }
        public abstract Task<bool> Play(Guid playerId, int position, GameState gameState);
        protected abstract Task<bool> TrySetDraw(GameState gameState);

        public virtual async Task<GameState?> UpdateGameState(Guid id)
        {
            GameState? gameState = await stateRepo.GetById(id);
            if (gameState!=null&&!gameState.IsGameOver)
            {
                await TrySetDraw(gameState);
            }
            return gameState;
        }
        public virtual async Task<bool> ExitGame(Guid playerId)
        {
            Player? player = await playerRepo.GetById(playerId);
            if (player == null) return false;
            SetPlayerGameEnd(player);
            await playerRepo.Save(player);
            return true;
        }
        public virtual async Task<Guid?> StartGame(Guid playerId, Guid opponentId, string gameTypeName)
        {
            Player? player1 = await playerRepo.GetById(playerId);
            Player? player2 = await playerRepo.GetById(opponentId);
            GameType? gameType = await gameTypeRepo.GetByName(gameTypeName);

            if (player1 == null || player2 == null||gameType == null)
                return null;
            if (player1.IsPlaying||player2.IsPlaying||player1.CurrentGameTypeId!=gameType.Id||player2.CurrentGameTypeId!=gameType.Id)
                return null;

            SetPlayerGameStart(player1, gameType);
            SetPlayerGameStart(player2, gameType);
            player1.WaitingForMove=true;
            player2.WaitingForMove=false;
            await playerRepo.Save(player1);
            await playerRepo.Save(player2);

            GameState state = new(player1, player2, gameType);
            return await stateRepo.Save(state);

        }
       


        protected void SetPlayerGameStart(Player player, GameType gameType)
        {
            player.LookingForOpponent=false;
            player.CurrentGameType=gameType;
            player.GameStarted=DateTime.Now;
            player.IsPlaying=true;
        }
        protected void SetPlayerGameEnd(Player player)
        {
            player.LookingForOpponent=false;
            player.CurrentGameType=null;
            player.IsPlaying=false;
        }
    }
}
