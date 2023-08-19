using System.Text.Json;
using TaskSeven_GamePlatform.Helpers;
using TaskSeven_GamePlatform.Server.Domain.Repo.Interfaces;
using TaskSeven_GamePlatform.Server.Services.Interfaces;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Server.Services
{

    public class RockPaperScissorsService : GameServiceBase, IRockPaperScissorsService
    {
        public RockPaperScissorsService(IGameStateRepo stateRepo, IPlayerRepo playerRepo, IGameTypeRepo gameTypeRepo) : base(stateRepo, playerRepo, gameTypeRepo)
        {
        }
        private Dictionary<int, int> Moves = new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 1 },
            { 2, 2 },
        };
        public override async Task<GameState?> UpdateGameState(Guid id)
        {
            GameState? gameState = await stateRepo.GetById(id);
            if (gameState==null||gameState.Player1==null||gameState.Player2==null) return null;
            if (!gameState.IsGameOver)
            {
                // if (!gameState.Player1.WaitingForMove||!gameState.Player2.WaitingForMove) return null;
                await TrySetDraw(gameState);
            }
            return gameState;
        }
        public override async Task<bool> Play(Guid playerId, int move, GameState gameState)
        {
            if (gameState.IsGameOver) return false;
            if (gameState.Player1==null||gameState.Player2==null) throw new ArgumentNullException("One or more of the players in game state is null");
            List<Player> players = new() { gameState.Player1, gameState.Player2 };
            Player? opponent = players.SingleOrDefault(p => p.Id!=playerId);
            Player? player = players.SingleOrDefault(p => p.Id==playerId);
            if (player==null||opponent==null) throw new ArgumentNullException("One or more of the players in game state is null");

            if (player.WaitingForMove) return false;

            gameState.MovesLeft -= 1;
            int[]? field = JsonSerializer.Deserialize<int[]>(gameState.Field, options);
            if (field==null) throw new ArgumentNullException("Game field is null");

            int playerIndex = players.IndexOf(player);
            int opponentIndex = players.IndexOf(opponent);
            field[playerIndex] = move;
            if (await TrySetDraw(gameState, field))
            {

                return true;
            }

            await CheckWinner(gameState, player, opponent, playerIndex, opponentIndex, field);
            gameState.Field=JsonSerializer.Serialize(field, options);
            gameState.LastMove=DateTime.Now;
            await stateRepo.Save(gameState);

            player.WaitingForMove=true;
            await playerRepo.Save(player);
            await playerRepo.Save(opponent);

            return true;

        }

        private async Task<bool> CheckWinner(GameState gameState, Player player, Player opponent, int playerIndex, int opponentIndex, int[] field)
        {
            if (field.Any(x => x==-1)) return false;
            if (field.Length!=2) return false;
            if (field[0]==field[1]) return false;
            int half = (Moves.Count-1)/2;
            for (int i = 1; i<=half; i++)
            {
                int circular = CollectionHelper.GetForwardCircularIndex(Moves.Count, field[playerIndex]+i);
                if (circular==field[opponentIndex])
                {
                    gameState.IsGameOver = true;
                    gameState.Winner=opponent;
                    opponent.IsPlaying=false;
                    player.IsPlaying=false;
                    await stateRepo.Save(gameState);
                    await playerRepo.Save(opponent);
                    await playerRepo.Save(player);

                    return true;
                }
            }
            gameState.IsGameOver = true;
            gameState.Winner=player;
            opponent.IsPlaying=false;
            player.IsPlaying=false;
            return true;
        }

        protected async Task<bool> TrySetDraw(GameState gameState, int[] field)
        {
            if (await TrySetDraw(gameState)) return true;
            if (field.Any(x => x==-1)) return false;
            if (field[0]==field[1])
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


        protected override async Task<bool> TrySetDraw(GameState gameState)
        {
            if (gameState.Player1==null||gameState.Player2==null)
                throw new ArgumentNullException();

            if ((DateTime.Now-gameState.LastMove).Seconds>gameState.SecondsPerMove)
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
        public override async Task<Guid?> StartGame(Guid playerId, Guid opponentId, string gameTypeName)
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
            player1.WaitingForMove=false;
            player2.WaitingForMove=false;
            await playerRepo.Save(player1);
            await playerRepo.Save(player2);

            GameState state = new(player1, player2, gameType);
            return await stateRepo.Save(state);

        }

    }
}
