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
        public MoveRequestModel(Player player, GameState stateBase, int position)
        {
            Player=player;
            StateBase=stateBase;
            Position=position;
        }

        public Player Player { get; set; }
        public GameState StateBase { get; set; }
        public int Position { get; set; }

    }
}
