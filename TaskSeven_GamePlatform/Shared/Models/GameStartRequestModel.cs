using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSeven_GamePlatform.Shared.Models
{
    public class GameStartRequestModel
    {
        public GameStartRequestModel(Guid playerId, Guid opponentId, Guid gameTypeId)
        {
            PlayerId=playerId;
            OpponentId=opponentId;
            GameTypeId=gameTypeId;
        }

        public Guid PlayerId { get; set; }
        public Guid OpponentId { get; set; }
        public Guid GameTypeId { get; set; }
    }
}
