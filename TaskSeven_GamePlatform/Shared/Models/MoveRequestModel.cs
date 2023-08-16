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
        public MoveRequestModel(Guid playerId, Guid gameStateId, int position)
        {
            PlayerId=playerId;
            StateId=gameStateId;
            Position=position;
        }

        public Guid PlayerId { get; set; }
        public Guid StateId { get; set; }
        public int Position { get; set; }

    }
}
