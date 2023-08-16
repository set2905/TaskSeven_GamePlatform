using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSeven_GamePlatforms.Shared.Models;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public enum TicTacToeMarker
    {
        Empty = -1,
        O = 0,
        X = 1
    }
    public class MoveRequestModel
    {
        public MoveRequestModel(Player player, Guid gameStateId, int position)
        {
            Player=player;
            StateId=gameStateId;
            Position=position;
        }

        public Player Player { get; set; }
        public Guid StateId { get; set; }
        public int Position { get; set; }

    }
}
