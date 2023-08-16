using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameState
    {
        public GameState()
        {
            Field=string.Empty;
        }

        public GameState(Player player1, Player player2, GameType gameType)
        {
            Player1=player1;
            Player2=player2;
            GameType=gameType;
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            };
            int[] field = new int[gameType.FieldSize];
            for (var i = 0; i < field.Length; i++)
            {
                field[i] = -1;
            }
            Field=JsonSerializer.Serialize(field, options);
        }

        public Guid Id { get; set; }
        public bool IsGameOver { get; set; }

        public bool IsDraw { get; set; }

        public Player? Player1 { get; set; }

        public Player? Player2 { get; set; }

        public string Field { get; set; }
        public int MovesLeft { get; set; }
        public int SecondsPerMove { get; set; }
        public DateTime LastMove { get; set; }
        public GameType GameType { get; set; }
    }
}
