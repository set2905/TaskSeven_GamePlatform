using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class TicTacToeGameState
    {
        public TicTacToeGameState()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            int[] field = new int[9];
            for (var i = 0; i < field.Length; i++)
            {
                field[i] = -1;
            }
            Field=JsonSerializer.Serialize(field, options);

            Player1=new();
            Player2=new();
        }
        public Guid Id { get; set; }
        public bool IsGameOver { get; set; }

        public bool IsDraw { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public string Field { get; set; }
        public int MovesLeft { get; set; }
        public int SecondsPerMove { get; set; }
        public DateTime LastMove { get; set; }
    }
}
