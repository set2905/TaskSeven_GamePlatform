using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSeven_GamePlatform.Shared.Models;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public enum RockPaperScissorsMarker
    {
        Empty = -1,
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }
    public enum TicTacToeMarker
    {
        Empty = -1,
        O = 0,
        X = 1
    }
    public class MoveRequestModel
    {
        public MoveRequestModel(Guid playerId, Guid stateId, int position)
        {
            PlayerId=playerId;
            StateId=stateId;
            Position=position;
        }

        public Guid PlayerId { get; set; }
        public Guid StateId { get; set; }
        public int Position { get; set; }

    }
}
