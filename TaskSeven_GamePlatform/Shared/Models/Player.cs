using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatforms.Shared.Models
{
    public class Player
    {
        public Player()
        {
            Name=string.Empty;
        }

        public Player(string name)
        {
            Name=name;
            IsPlaying=false;
            WaitingForMove=false;
            LookingForOpponent=false;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Player? Opponent { get; set; }
        public bool IsPlaying { get; set; }
        public bool WaitingForMove { get; set; }
        public bool LookingForOpponent { get; set; }
        public DateTime GameStarted { get; set; }
        public string? ConnectionId { get; set; }
        public GameType CurrentGame { get; set; }
        public Guid CurrentGameId { get; set; }
    }
}
