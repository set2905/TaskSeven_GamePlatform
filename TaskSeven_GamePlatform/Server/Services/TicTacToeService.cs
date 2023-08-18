using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{
    public class TicTacToeService : GameServiceBase, ITicTacToeService
    {
        public TicTacToeService(IGameStateRepo stateRepo, IPlayerRepo playerRepo, IGameTypeRepo gameTypeRepo) : base(stateRepo, playerRepo, gameTypeRepo)
        {
        }
        public override async Task<bool> Play(Guid playerId, int position, GameState gameState)
        {
            if (gameState.IsGameOver) return false;
            if (gameState.Player1==null||gameState.Player2==null) throw new ArgumentNullException("One or more of the players in game state is null");
            Player[] players = new Player[] { gameState.Player1, gameState.Player2 };
            Player? opponent = players.SingleOrDefault(p => p.Id!=playerId);
            Player? player = players.SingleOrDefault(p => p.Id==playerId);
            if(player==null||opponent==null) throw new ArgumentNullException("One or more of the players in game state is null");

            if (player.WaitingForMove) return false;

            gameState.MovesLeft -= 1;
            int[]? field = JsonSerializer.Deserialize<int[]>(gameState.Field, options);
            if (field==null) throw new ArgumentNullException("Game field is null");
            if (!VerifyMove(position, field)) return false;
            TicTacToeMarker marker;
            marker=gameState.Player1.Id==playerId ? TicTacToeMarker.X : TicTacToeMarker.O;

            PlaceMarker((int)marker, position, field);
            if (CheckWinner(field))
            {
                gameState.IsGameOver = true;
                gameState.Winner=player;
                player.IsPlaying=false;
                opponent.IsPlaying=false;
            }
            else if (await CheckDraw(gameState))
                return false;
            gameState.Field=JsonSerializer.Serialize(field, options);
            gameState.LastMove=DateTime.Now;
            await stateRepo.Save(gameState);
            player.WaitingForMove=true;
            opponent.WaitingForMove=false;

            await playerRepo.Save(player);
            await playerRepo.Save(opponent);
            return true;
        }
        protected override async Task<bool> CheckDraw(GameState gameState)
        {
            if (gameState.Player1==null||gameState.Player2==null)
                throw new ArgumentNullException();
            if (gameState.MovesLeft <= 0||(DateTime.Now-gameState.LastMove).Seconds>gameState.SecondsPerMove)
            {
                gameState.IsGameOver = true;
                gameState.IsDraw = true;
                gameState.Player1.IsPlaying=false;
                gameState.Player2.IsPlaying=false;
                await playerRepo.Save(gameState.Player1);
                await playerRepo.Save(gameState.Player2);
                await stateRepo.Save(gameState);
                return true;
            }
            return false;
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

