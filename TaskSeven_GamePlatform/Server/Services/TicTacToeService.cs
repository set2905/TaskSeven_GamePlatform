using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Server.Controllers;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public class TicTacToeService : ITicTacToeService
    {
        private readonly IGameStateRepo stateRepo;
        private readonly IPlayerRepo playerRepo;
        private readonly IGameTypeRepo gameTypeRepo;
        JsonSerializerOptions options;

        public TicTacToeService(IGameStateRepo stateRepo,IPlayerRepo playerRepo, IGameTypeRepo gameTypeRepo)
        {
            this.stateRepo=stateRepo;
            options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            this.playerRepo=playerRepo;
            this.gameTypeRepo=gameTypeRepo;
        }
        public async Task<GameState?> GetGameState(Guid id)
        {
            return await stateRepo.GetById(id);
        }

        public async Task<Guid?> StartGame(Guid playerId, Guid opponentId, Guid gameTypeId)
        {
            Player? player1 = await playerRepo.GetById(playerId);
            Player? player2 = await playerRepo.GetById(opponentId);
            GameType? gameType = await gameTypeRepo.GetById(gameTypeId);

            if (player1 == null || player2 == null||gameType == null)
                return null;
            if (player1.IsPlaying||player2.IsPlaying||player1.CurrentGameTypeId!=gameTypeId||player2.CurrentGameTypeId!=gameTypeId)
                return null;

            GameState state = new(player1, player2, gameType);
            return await stateRepo.Save(state);

        }

        public async Task<bool> Play(TicTacToeMarker player, int position, GameState state)
        {
            if (state.IsGameOver)
                return false;

            state.MovesLeft -= 1;
            if (state.MovesLeft <= 0||(DateTime.Now-state.LastMove).Seconds>state.SecondsPerMove)
            {
                state.IsGameOver = true;
                state.IsDraw = true;
                await stateRepo.Save(state);
                return false;
            }
            int[]? field = JsonSerializer.Deserialize<int[]>(state.Field, options);

            if (!VerifyMove(position, field)) return false;

            PlaceMarker((int)player, position, field);
            state.IsGameOver = CheckWinner(field);
            state.Field=JsonSerializer.Serialize(field, options);
            await stateRepo.Save(state);
            return true;
        }
        private bool VerifyMove(int position, int[] field)
        {
            if (field[position] != -1)
                return false;
            if (position > field.Length)
                return false;
            return true;
        }

        /// <summary>
        /// Checks each different combination of marker placements and looks for a winner
        /// Each position is marked with an initial -1 which means no marker has yet been placed
        /// </summary>
        /// <returns>True if there is a winner</returns>
        private bool CheckWinner(int[] field)
        {
            for (int i = 0; i < 3; i++)
            {
                if (
                    ((field[i * 3] != -1 && field[(i * 3)] == field[(i * 3) + 1] && field[(i * 3)] == field[(i * 3) + 2]) ||
                     (field[i] != -1 && field[i] == field[i + 3] && field[i] == field[i + 6])))
                {
                    return true;
                }
            }
            if ((field[0] != -1 && field[0] == field[4] && field[0] == field[8]) || (field[2] != -1 && field[2] == field[4] && field[2] == field[6]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Places a marker at the given position for the given player as long as the position is marked as -1
        /// </summary>
        /// <param name="player">The player number should be 0 or 1</param>
        /// <param name="position">The position where to place the marker, should be between 0 and 9</param>
        /// <returns>True if the marker position was not already taken</returns>
        private void PlaceMarker(int player, int position, int[] field)
        {
            field[position] = player;
        }
    }
}

