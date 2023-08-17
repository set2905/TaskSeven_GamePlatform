using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameState
    {
        public GameState()
        {
            Field=string.Empty;
        }

        public GameState(Player player1, Player player2, GameType gameType, int secondsPerMove=30)
        {
            Player1=player1;
            Player2=player2;
            GameType=gameType;
            IsDraw=false;
            IsGameOver=false;
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            int[] field = new int[gameType.FieldSize];
            for (var i = 0; i < field.Length; i++)
            {
                field[i] = -1;
            }
            MovesLeft=field.Length;
            LastMove=DateTime.Now;
            SecondsPerMove=secondsPerMove;
            Field=JsonSerializer.Serialize(field, options);
        }

        public Guid Id { get; set; }
        public bool IsGameOver { get; set; }

        public bool IsDraw { get; set; }

        public virtual Player? Player1 { get; set; }

        public virtual Player? Player2 { get; set; }
        public virtual Player? Winner { get; set; }

        public string Field { get; set; }
        public int MovesLeft { get; set; }
        public int SecondsPerMove { get; set; }
        public DateTime LastMove { get; set; }
        public virtual GameType GameType { get; set; }
    }
}
